using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DominosNET.Payment
{
   /// <summary>
   /// Class for paying via credit card, with some nice logic to see if your card is valid using regular expressions (regex).
   /// </summary>
    public class PaymentObject
    {
        public enum CardType
        {
            MasterCard, Visa, AmericanExpress, Discover, JCB
        };
        [Serializable]
        private class InvalidCardException : Exception
        {

            public InvalidCardException() { }
            public InvalidCardException(string message) : base(message) { }
            public InvalidCardException(string message, Exception inner) : base(message, inner) { }
            protected InvalidCardException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
        private static CardType FindType(string cardNumber)
        {
            //https://www.regular-expressions.info/creditcard.html
            if (Regex.Match(cardNumber, @"^4[0-9]{12}(?:[0-9]{3})?$").Success)
            {
                return CardType.Visa;
            }

            if (Regex.Match(cardNumber, @"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$").Success)
            {
                return CardType.MasterCard;
            }

            if (Regex.Match(cardNumber, @"^3[47][0-9]{13}$").Success)
            {
                return CardType.AmericanExpress;
            }

            if (Regex.Match(cardNumber, @"^6(?:011|5[0-9]{2})[0-9]{12}$").Success)
            {
                return CardType.Discover;
            }

            if (Regex.Match(cardNumber, @"^(?:2131|1800|35\d{3})\d{11}$").Success)
            {
                return CardType.JCB;
            }

            throw new InvalidCardException("Unknown card.");
        }
        public string name;
        public string expiration;
        public string number;
        public string cvv;
        public string zip;
        public CardType type;
        public PaymentObject(string Name, string Expiration, string Number, string Cvv, string Zip)
        {
            name = Name;
            expiration = Expiration;
            number = Number;
            cvv = Cvv;
            zip = Zip;
            type = FindType(number);
        }
    }
}
