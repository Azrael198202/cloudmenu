using System;
using System.Collections.Generic;


namespace cloudmenu_api.LwUtils
{
    public enum PadOption
    {
        PadLeft,
        PadRight,
        PadBoth
    }
    public static class CmnUtils
    {
        public static string formatCurrency(long value, int fixLength = 0)
        {
            string valueWithComma = value.ToString("N0");
            int length = valueWithComma.Length;
            if (fixLength > 0)
            {
                if (length < fixLength)
                {
                    valueWithComma = new String(' ', fixLength - length) + valueWithComma;
                }
            }

            return "¥" + valueWithComma;
        }

        public static string formatCommaNum(long value, int fixLength = 0)
        {
            string valueWithComma = value.ToString("N0");
            int length = valueWithComma.Length;
            if (fixLength > 0)
            {
                if (length < fixLength)
                {
                    valueWithComma = new String(' ', fixLength - length) + valueWithComma;
                }
            }

            return valueWithComma;
        }

        /**
         * 
         */
        public static string padWith(PadOption padTo, int length, string value)
        {
            string rsltValue = value;

            char padValue = ' ';
            int valueLength = value.Length;
            int padLength = length - valueLength;
            if (padLength <= 0)
            {
                return rsltValue;
            }

            if (padTo == PadOption.PadLeft)
            {
                rsltValue = new String(padValue, padLength) + value;
            }
            else if (padTo == PadOption.PadRight)
            {
                rsltValue = value + new String(padValue, padLength);
            }
            else if (padTo == PadOption.PadBoth)
            {
                if (padLength % 2 == 0)
                {
                    int halfLength = padLength / 2;
                    rsltValue = new String(padValue, halfLength) + value + new String(padValue, halfLength);
                }
                else
                {
                    int halfLength = padLength / 2;
                    rsltValue = new String(padValue, halfLength) + value + new String(padValue, halfLength + 1);
                }
            }

            return rsltValue;
        }

        public static long calsIncludeTax(decimal amountIncludTax, int taxPercent)
        {
            return (long)Math.Floor(amountIncludTax - amountIncludTax * 100 / (taxPercent + 100));
        }
    }
}