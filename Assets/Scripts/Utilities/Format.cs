using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Utilities
{
    class Format
    {
        public static string Currency(int amount)
        {
            string currency = "";
            if (amount > 0)
            {
                currency = "$" + amount.ToString("n0");
            }
            else if(amount < 0)
            {
                currency = "$" + amount.ToString("n0");
            }
            else
            {
                currency = "($" + amount.ToString("n0") +")";
            }
            return currency;
        }
        public static string Number(int amount)
        {
            string number = "";
            if (amount > 0)
            {
                number = amount.ToString("n0");
            }
            else if (amount < 0)
            {
                number = amount.ToString("n0");
            }
            else
            {
                number = "(" + amount.ToString("n0") + ")";
            }
            return number;
        }
    }
}
