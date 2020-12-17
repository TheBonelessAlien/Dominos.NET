using System;
using System.Collections.Generic;
using System.Text;
using DominosNET.Stores;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DominosNET.Menu
{
   /// <summary>
   /// TIP: You can also search for coupons, since im such a nice guy tryna save you money.
   /// </summary>
    public class Menu
    {
        public JObject MenuJSON;
        public string Country;

        public static Menu FromStore(string storeid, string country)
        {
            async Task<string> GetMenuJSONString()
            {
                if (country == "ca")
                {
                    var httpClient = new HttpClient();
                    string URL = urls.urls.ca["menu_url"].Replace("{store_id}", storeid).Replace("{lang}", "en");

                    var content = await httpClient.GetStringAsync(URL);
                    return content;

                }
                else
                {
                    var httpClient = new HttpClient();
                    string URL = urls.urls.us["menu_url"].Replace("{store_id}", storeid).Replace("{lang}", "en");


                    var content = await httpClient.GetStringAsync(URL);
                    return content;

                }
            }
            JObject jsonData = JObject.Parse(GetMenuJSONString().Result);
            return new Menu(country, jsonData);
        }
        public void Search(string searchTerm)
        {
            JObject predefinedproducts = JObject.Parse(MenuJSON["Variants"].ToString());

            foreach (var predefinedproduct in predefinedproducts)
            {
               
                if (JObject.Parse(predefinedproduct.Value.ToString())["Code"].ToString().ToLower().Contains(searchTerm.ToLower()) && JObject.Parse(predefinedproduct.Value.ToString())["Price"] != null)
                {

                    Console.WriteLine(JObject.Parse(predefinedproduct.Value.ToString())["Code"].ToString() + "  " + JObject.Parse(predefinedproduct.Value.ToString())["Name"].ToString() + "  " + "$" + JObject.Parse(predefinedproduct.Value.ToString())["Price"]);
                }
                else if (JObject.Parse(predefinedproduct.Value.ToString())["Name"].ToString().ToLower().Contains(searchTerm.ToLower()) && JObject.Parse(predefinedproduct.Value.ToString())["Price"] != null)
                {
                    Console.WriteLine(JObject.Parse(predefinedproduct.Value.ToString())["Code"].ToString() + "  " + JObject.Parse(predefinedproduct.Value.ToString())["Name"].ToString() + "  " + JObject.Parse(predefinedproduct.Value.ToString())["Price"]);
                }
                else if (JObject.Parse(predefinedproduct.Value.ToString())["Name"].ToString().ToLower().Contains(searchTerm.ToLower()) && JObject.Parse(predefinedproduct.Value.ToString())["Price"] != null)
                {
                    Console.WriteLine(JObject.Parse(predefinedproduct.Value.ToString())["Code"].ToString() + "  " + JObject.Parse(predefinedproduct.Value.ToString())["Name"].ToString() + "  " + JObject.Parse(predefinedproduct.Value.ToString())["Price"]);
                }
            }
        }
        /// <summary>
        /// Searches the menu for coupons to get products for a discount (e.g add a 6-piece Marbled Cookie Brownie for 3.99.) 
        /// </summary>

        public void SearchCoupons(string searchTerm)
        {
            JObject predefinedproducts = JObject.Parse(MenuJSON["Coupons"].ToString());

            foreach (var predefinedproduct in predefinedproducts)
            {

                if (JObject.Parse(predefinedproduct.Value.ToString())["Code"].ToString().ToLower().Contains(searchTerm.ToLower()) && JObject.Parse(predefinedproduct.Value.ToString())["Price"] != null)
                {

                    Console.WriteLine(JObject.Parse(predefinedproduct.Value.ToString())["Code"].ToString() + "  " + JObject.Parse(predefinedproduct.Value.ToString())["Name"].ToString() + "  " + "$" + JObject.Parse(predefinedproduct.Value.ToString())["Price"]);
                }
                else if (JObject.Parse(predefinedproduct.Value.ToString())["Name"].ToString().ToLower().Contains(searchTerm.ToLower()) && JObject.Parse(predefinedproduct.Value.ToString())["Price"] != null)
                {
                    Console.WriteLine(JObject.Parse(predefinedproduct.Value.ToString())["Code"].ToString() + "  " + JObject.Parse(predefinedproduct.Value.ToString())["Name"].ToString() + "  " + JObject.Parse(predefinedproduct.Value.ToString())["Price"]);
                }
                else if (JObject.Parse(predefinedproduct.Value.ToString())["Name"].ToString().ToLower().Contains(searchTerm.ToLower()) && JObject.Parse(predefinedproduct.Value.ToString())["Price"] != null)
                {
                    Console.WriteLine(JObject.Parse(predefinedproduct.Value.ToString())["Code"].ToString() + "  " + JObject.Parse(predefinedproduct.Value.ToString())["Name"].ToString() + "  " + JObject.Parse(predefinedproduct.Value.ToString())["Price"]);
                }
            }
        }
        public Menu(string c, JObject j)
        {
            Country = c;
            MenuJSON = j;
        }
        
    }
}
