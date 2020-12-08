using System;

namespace DominosNET.Customer
{
    ///<summary>
    /// The class for the customer, used to create an order.
    /// </summary>
    public class Customer
    {
        public string phone_number;
        public string first_name;
        public string last_name; //state or province
        public string email; //This can be your postal code if you live in canada

        public Customer(string phonenumber, string firstname, string lastname, string e_mail)
        {
            phone_number = phonenumber;
            first_name = firstname;
            last_name = lastname;
            email = e_mail;
            
        }
    }
}