using System;
using Newtonsoft.Json.Linq;

namespace CloackaV4.Scripts
{
    public class AnalyticsModel
    {
        public string Locale;
        public string Model;
        public int ApiVersion;
        public float ScreenSize;
        public int Uptime;
        public bool IsEmulator;
        public bool IsAccelerometerDead;
        public string DeviceId;
        public string Brand;
        public string AdjustId;
        public string RefCode;
        public string CustomerToken;
        public string AppToken;
        public string AdjustToken;
        public string Url;

        public string ToJson()
        {
            var x = 'a';
            
            try
            {
                var json = new JObject();
                
                /* a */
                json.Add(x.ToString(), IsEmulator);
                x++;
            
                /* b */
                json.Add(x.ToString(), DeviceId);
                x++;
            
                /* c */
                json.Add(x.ToString(), Locale);
                x++;
            
                /* d */
                json.Add(x.ToString(), ScreenSize);
                x++;
            
                /* e */
                json.Add(x.ToString(), ApiVersion);
                x++;
            
                /* f */
                //json.Add(x.ToString(), simRegion);
                x++;
            
                /* g */
                json.Add(x.ToString(), Brand);
                x++;
            
                /* h */
                json.Add(x.ToString(), Uptime);
                x++;
            
                /* i */
                json.Add(x.ToString(), IsAccelerometerDead);
                x++;
            
                /* j */
                x++; // emails: deprecated
            
                /* k */
                x++; // push mainToken (if used)
            
                /* l */
                x++; // l reserved
            
                /* m */
                json.Add(x.ToString(), Model);
                x++;
            
                /* n */
                //json.Add(x.ToString(), simCardOperatorName);
                x++;
            
                /* o */
                json.Add(x.ToString(), RefCode);
                x++;
            
                /* p */
                //json.Add(x.ToString(), deep());
                x++;
            
                /* q */
                x++; // app count (optional)
            
                /* r */
                json.Add(x.ToString(), AdjustId); // Adjust ID
                x++;
            
                /* s */
                json.Add(x.ToString(), AppToken); // App Token

                return json.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
