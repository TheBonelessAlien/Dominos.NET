using System;
using System.Collections.Generic;
using System.Text;
using DominosNET.Customer;
using DominosNET.Menu;
using DominosNET.Stores;
using DominosNET.Payment;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DominosNET.Order
{
    /// <summary>
    /// Class for creating and placing an order.
    /// NOTE: Coupons are applied when you place the order, they do not affect the price variable.
    /// </summary>
    public class Order
    {
        [Serializable]
        private class InvalidItemCodeException : Exception
        {

            public InvalidItemCodeException() { }
            public InvalidItemCodeException(string message) : base(message) { }
            public InvalidItemCodeException(string message, Exception inner) : base(message, inner) { }
            protected InvalidItemCodeException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
        public JObject Data;
        public JObject menuJSON;
        public Customer.Customer customer;
        public Address.Address address;
        public string Country;
        public Store store;
        /// <summary>
        /// Multiply this by your tax rate to get the actual price
        /// </summary>
        public double price = 0;
        public Order(Store s, Customer.Customer c, Address.Address a, string co)
        {
            Country = co;
            menuJSON = Menu.Menu.FromStore(s.id, co).MenuJSON;
            store = s;
            customer = c;
            address = a;
            Data = JObject.Parse(@"
            {
    ""Address"": {
        ""Street"": """",
        ""City"": """",
        ""Region"": """",
        ""PostalCode"": """",
        ""Type"": """"
    },
    ""Coupons"": [],
    ""CustomerID"": """",
    ""Extension"": """",
    ""OrderChannel"": ""OLO"",
    ""OrderID"": """",
    ""NoCombine"": ""true"",
    ""OrderMethod"": ""Web"",
    ""OrderTaker"": ""null"",
    ""Payments"": [],
    ""Products"": [],
    ""Market"": """",
    ""Currency"": """",
    ""ServiceMethod"": ""Delivery"",
    ""Tags"": {},
    ""Version"": ""1.0"",
    ""SourceOrganizationURI"": ""order.dominos.com"",
    ""LanguageCode"": ""en"",
    ""Partners"": {},
    ""NewUser"": true,
    ""metaData"": {},
    ""Amounts"": {},
    ""BusinessDate"": """",
    ""EstimatedWaitMinutes"": """",
    ""PriceOrderTime"": """",
    ""AmountsBreakdown"": {}
}
                                ");
            JObject addressData = (JObject)Data["Address"];
            Data["ServiceMethod"] = address.serviceType.ToString();
            addressData["Street"] = ((string)a.street);
            addressData["City"] = ((string)a.city);
            addressData["Region"] = ((string)a.region);
            addressData["PostalCode"] = ((string)a.zip);
            if (a.street.ToLower().Contains("apartment") || a.street.ToLower().Contains("apt") || a.street.ToLower().Contains("#"))
            {
                addressData["Type"] = ((string)"Apartment");
            }
            else
            {
                addressData["Type"] = ((string)"House");
            }


        }
        public void add_item(int quantity, string itemCode)
        {
            if ((JObject)menuJSON["Variants"][itemCode] == null)
            {
                throw new InvalidItemCodeException("Invalid item code, please make sure you are using the item code, not the item name. e.g (use 500DIETC instead of Diet Coke 500ml).");
            }
            for (int i = 0; i < quantity; i++)
            {
                JObject item = (JObject)menuJSON["Variants"][itemCode];
                JArray a = (JArray)Data["Products"];
                a.Add((JToken)item);
                price += item["Price"].ToObject<double>();
            }

        }
        public void remove_item(int quantityToRemove, string itemCode)
        {

            if ((JObject)menuJSON["Variants"][itemCode] == null)
            {
                throw new InvalidItemCodeException("Invalid item code, please make sure you are using the item code, not the item name. e.g (use 500DIETC instead of Diet Coke 500ml).");
            }
            for (int i = 0; i < quantityToRemove; i++)
            {

                JObject item = (JObject)menuJSON["Variants"][itemCode];
                JArray a = (JArray)Data["Products"];
                a.Remove((JToken)item);
                price -= item["Price"].ToObject<double>();

            }

        }
        private JObject send(string URL, bool Merge, string content)
        {

            Data["StoreID"] = store.id;
            Data["Email"] = customer.email;
            Data["FirstName"] = customer.first_name;
            Data["LastName"] = customer.last_name;
            Data["Phone"] = customer.phone_number;
            HttpClient c = new HttpClient();
            Console.WriteLine(Data.ToString());
            StringContent stringContent = null;
            if (content == null)
            {
                stringContent = new StringContent(" { " + @"""Order"" : " + Data.ToString() + " } ", Encoding.UTF8, "application/json");
            }
            else
            {
                stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            }
            c.DefaultRequestHeaders.Add("Referer", "https://order.dominos.com/en/pages/order/");

            Task<HttpResponseMessage> m = c.PostAsync(URL, stringContent);

           
            JObject jsonResponse = JObject.Parse(m.Result.Content.ReadAsStringAsync().Result.Replace(@"^""}]}", ""));

            if (Merge)
            {
                foreach (var keyValuePair in jsonResponse)
                {

                    Data[keyValuePair.Key] = keyValuePair.Value;

                }
            }
            
            return jsonResponse;

        }
        public void place(string type)
        {
            bool isAcceptable = false;
            JArray acceptablePaymentTypes = (JArray)store.Data["AcceptablePaymentTypes"];
            foreach (var v in acceptablePaymentTypes.Children())
            {
                if (v.ToString() == type)
                {
                    isAcceptable = true;
                }
            }
            if (!isAcceptable)
            {
                throw new Exception("Store does not support type " + type + ".");
            }
            JArray paymentArray = JArray.Parse(Data["Payments"].ToString());
            JObject typeObj = new JObject();
            typeObj.Add("Type", type);
            paymentArray.Add(typeObj);
            if (Country == "ca")
            {
                send(urls.urls.ca["place_url"], false, send(urls.urls.ca["price_url"], true, null).ToString());
            }
            else
            {
                send(urls.urls.ca["place_url"], false, send(urls.urls.us["price_url"], true, null).ToString());
            }


        }
        public void place(PaymentObject o)
        {
            bool isAcceptableCard = false;
            bool canPayWithCard = false;

            JArray acceptableCards = (JArray)store.Data["AcceptableCreditCards"];
            JArray acceptablePaymentTypes = (JArray)store.Data["AcceptablePaymentTypes"];
            foreach (var v in acceptableCards.Children())
            {
                if (v.ToString() == o.type.ToString())
                {
                    isAcceptableCard = true;
                }
            }
            foreach (var v in acceptablePaymentTypes.Children())
            {
                if (v.ToString() == "CreditCard")
                {
                    isAcceptableCard = true;
                }
            }
            if (canPayWithCard == false)
            {
                throw new Exception("Store does not support credit cards.");
            }
            if (isAcceptableCard)
            {
                JArray paymentArray = JArray.Parse(Data["Payments"].ToString());
                JObject typeObj = new JObject();
                typeObj.Add("Type", "CreditCard");
                typeObj.Add("Expiration", o.expiration);
                typeObj.Add("Amount", 0);
                typeObj.Add("CardType", o.type.ToString());
                typeObj.Add("Number", int.Parse(o.number));
                typeObj.Add("SecurityCode", int.Parse(o.cvv));
                typeObj.Add("PostalCode", int.Parse(o.zip));
                paymentArray.Add(typeObj);
            }
            else
            {
                throw new Exception("Card unsupported.");
            }
            if (Country == "ca")
            {
                send(urls.urls.ca["place_url"], false, send(urls.urls.ca["price_url"], true, null).ToString());
            }
            else
            {
                send(urls.urls.ca["place_url"], false, send(urls.urls.us["price_url"], true, null).ToString());
                
            }
        }
    }
}

/* this code may be useful later so im savin it here
 JArray acceptableCards = (JArray)store.Data["AcceptableCreditCards"];
            JArray acceptablePaymentTypes = (JArray)store.Data["AcceptablePaymentTypes"];
            if (acceptablePaymentTypes["CreditCard"] == null)
            {
                throw new Exception("Store does not support credit cards.");
            }
            if (acceptableCards[o.ToString()] != null)
            {
                JArray paymentArray = JArray.Parse(Data["Payments"].ToString());
                JObject typeObj = new JObject();
                typeObj.Add("Type", "CreditCard");
                typeObj.Add("Expiration", o.expiration);
                typeObj.Add("Amount", 0);
                typeObj.Add("CardType", o.type.ToString());
                typeObj.Add("Number", int.Parse(o.number));
                typeObj.Add("SecurityCode", int.Parse(o.cvv));
                typeObj.Add("PostalCode", int.Parse(o.zip));
                paymentArray.Add(typeObj);
                HttpClient c = new HttpClient();
                if (Country == "ca")
                {
                    
                   
                   
                    var stringContent = new StringContent(@"""Order"": " + Data.ToString(), Encoding.UTF8, "application/json");
                    c.DefaultRequestHeaders.Add("Referer", "https://order.dominos.com/en/pages/order/");
                    c.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    Task<HttpResponseMessage> m = c.PostAsync(urls.urls.ca["price_url"], stringContent);
                    
                    Console.WriteLine("Response: " + m.Result.Content.ReadAsStringAsync().Result);
                    
                }
                else
                {
                    var stringContent = new StringContent(@"""Order"": " + Data.ToString(), Encoding.UTF8, "application/json");
                    c.DefaultRequestHeaders.Add("Referer", "https://order.dominos.com/en/pages/order/");
                    c.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    Task<HttpResponseMessage> m = c.PostAsync(urls.urls.us["price_url"], stringContent);
                    Console.WriteLine("Response: " + m.Result.Content.ReadAsStringAsync().Result);
                }
            }
            else
            {
                throw new Exception("Credit card type unsupported.");
            }

            /// <summary>
            /// Make sure the type has only the first letter uppercase (e.g cAsH should be Cash)
            /// </summary>
*/
