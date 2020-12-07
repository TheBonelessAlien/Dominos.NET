using System;
using System.Collections.Generic;
using System.Text;

namespace DominosNET.urls
{
    /// <summary>
    /// Class for accessing the URLs from other namespaces.
    /// </summary>
    public class urls
    {
        public static Dictionary<string, string> ca
        {
            get
            {
                return new Dictionary<string, string>
            {
           {"find_url", "https://order.dominos.ca/power/store-locator?s={line1}&c={line2}&type={type}"},
           {"info_url", "https://order.dominos.ca/power/store/{store_id}/profile"},
           {"menu_url", "https://order.dominos.ca/power/store/{store_id}/menu?lang={lang}&structured=true"},
           {"place_url", "https://order.dominos.ca/power/place-order"},
           {"price_url", "https://order.dominos.ca/power/price-order"},
           {"track_by_order", "https://trkweb.dominos.ca/orderstorage/GetTrackerData?StoreID={store_id}&OrderKey={order_key}"},
           {"track_by_phone", "https://trkweb.dominos.ca/orderstorage/GetTrackerData?Phone={phone}"},
           {"validate_url", "https://order.dominos.ca/power/validate-order"},
           {"coupon_url", "https://order.dominos.ca/power/store/{store_id}/coupon/{couponid}?lang={lang}"},
            };
            }
        }

        public static Dictionary<string, string> us
        {
            get
            {
                return new Dictionary<string, string>
        {
            {"find_url", "https://order.dominos.com/power/store-locator?s={line1}&c={line2}&type={type}"},
            {"info_url", "https://order.dominos.com/power/store/{store_id}/profile"},
            {"menu_url", "https://order.dominos.com/power/store/{store_id}/menu?lang={lang}&structured=true"},
            {"place_url", "https://order.dominos.com/power/place-order"},
            {"price_url", "https://order.dominos.com/power/price-order"},
            {"track_by_order", "https://trkweb.dominos.com/orderstorage/GetTrackerData?StoreID={store_id}&OrderKey={order_key}"},
            {"track_by_phone", "https://trkweb.dominos.com/orderstorage/GetTrackerData?Phone={phone}"},
            {"validate_url", "https://order.dominos.com/power/validate-order"},
            {"coupon_url", "https://order.dominos.com/power/store/{store_id}/coupon/{couponid}?lang={lang}"},
        };
            }

        }

    }
}
