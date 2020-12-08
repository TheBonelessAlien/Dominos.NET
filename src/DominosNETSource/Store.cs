using System;
using System.Collections.Generic;
using System.Text;
using DominosNET.urls;
using System.Net.Http;
using System.Threading.Tasks;
using DominosNET.Menu;
using Newtonsoft.Json.Linq;

namespace DominosNET.Stores
{
   //TODO: Add a method which lets the user get a store by its' id.
    public class Store
    {
        
        public static string Country;
        public JObject Data;
        public string id;
        private async Task<string> GetMenuJSONString()
        {
            
            if (Country == "ca")
            {
                var httpClient = new HttpClient();
                string URL = urls.urls.ca["menu_url"].Replace("{store_id}", id).Replace("{lang}", "en");

                var content = await httpClient.GetStringAsync(URL);
                return content;

            }
            else
            {
                var httpClient = new HttpClient();
                string URL = urls.urls.us["menu_url"].Replace("{store_id}", id).Replace("{lang}", "en");

                
                var content = await httpClient.GetStringAsync(URL);
                return content;

            }
        }


        public Menu.Menu GetMenu()
        {
            JObject MenuJSON = JObject.Parse(GetMenuJSONString().Result);
            return new Menu.Menu(Country, MenuJSON);
        }
      
        public Store(JObject data, string country, string storeid)
        {
            Country = country;
            Data = data;
            id = storeid;
        }
    }
}
