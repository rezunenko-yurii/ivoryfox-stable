using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CloudContentDelivery.Editor.Scripts
{
    public class CcdHelper
    {
        public static string Base64Encode(string plainText) 
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    
        public static string Base64Decode(string base64EncodedData) {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        
        public static IEnumerable<string> FindAll(string path, string[] except)
        {
            IEnumerable<string> files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(s => !except.Any(s.EndsWith));
            
            return files;
        }
        
        public static string GetCheckSum(MemoryStream memoryStream)
        {
            string checkSum;
            using (var md5 = MD5.Create())
            {
                checkSum = ToHex(md5.ComputeHash(memoryStream), false);
            }

            return checkSum;
        }
        
        public static string ToHex(byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length*2);

            foreach (var t in bytes) result.Append(t.ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }
    }
}