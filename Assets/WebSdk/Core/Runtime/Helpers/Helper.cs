using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WebSdk.Core.Runtime.Helpers
{
    public static class Helper
    {
        private static AndroidJavaClass toastJavaClass;
        public const string ClassName = "com.until.unit.ToastMsg";

        public static void ShowToast(string msg)
        {
            if (toastJavaClass == null)
            {
                toastJavaClass = new AndroidJavaClass(ClassName);
            }
            
            toastJavaClass.CallStatic("show", msg);
        }
        
        public static string EncodeDecrypt(string str)
        {
            ushort secretKey = 0x0088;
            
            string newStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                var ch = str.ToArray();
            
                foreach (var c in ch)
                {
                    newStr += TopSecret(c, secretKey);
                }
            }
            //Root.logger.Send($"EncodeDecrypt from {str} -> {newStr}");
            return newStr;
        }
        
        private static char TopSecret(char character, ushort secretKey)
        {
            character = (char)(character ^ secretKey);
            return character;
        }
        
        /// <summary>
        /// Get string value after [last] a.
        /// </summary>
        public static string After(this string value, string a)
        {
            int posA = value.LastIndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= value.Length)
            {
                return "";
            }
            return value.Substring(adjustedPosA);
        }
        
        public static void Shuffle<T> (this System.Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1) 
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
        
        public static void Shuffle<T> (this System.Random rng, List<T> list)
        {
            int n = list.Count;
            while (n > 1) 
            {
                int k = rng.Next(n--);
                T temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }
    }
}
