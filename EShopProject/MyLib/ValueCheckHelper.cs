using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.MyLib
{
    public static class ValueCheckHelper
    {
        public static bool EmptyCheckString(params string[] value)
        {
            foreach (string item in value)
            {
                if (FormatControlString(item).Length <= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ValueSizeCheckString(string value, int minSize, int maxSize)
        {
            if ((value.Length >= minSize) && (value.Length <= maxSize))
                return false;
            return true;
        }
        /// <summary>
        /// DeğerUzunluğunu eşit olduği size
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static bool ValueSizeCheckString(string value, int size)
        {
            if (value.Length == size)
                return false;
            return true;
        }

        public static string FormatControlString(string value)
        {
            string buffer;

            buffer = value.Replace("'", "").Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Trim();

            return buffer;
        }

        public static bool NullAndNotZeroCheckNumber(params object[] value)
        {
            bool result = true;

            foreach (object item in value)
            {
                if (item == null)
                    return result;

                if (item is int)
                    if ((int)item == 0)
                        return result;

                if (item is decimal)
                    if ((decimal)item == 0.0m)
                        return result;

                if (item is float)
                    if ((float)item == 0.0f)
                        return result;

            }
            return !result;
        }
    }
}