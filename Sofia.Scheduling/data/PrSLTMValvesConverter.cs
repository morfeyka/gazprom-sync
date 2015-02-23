using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using PSI_API;

namespace Sofia.Scheduling.data
{
    public class PrSLTMValvesConverter:BaseConverter,IDataConverter
    {

        public PrSLTMValvesConverter()
        {
            ConverterName = "PrvalvesConverter";
            Pattern = WildcardToRegex(@"T09\.UT[0-9][0-9]\.SLTM\.KP*\.V*");
        }

        public new double Convert(double val,string key)
        {

            if (IsValueUnknwon(val))
            {
                val = GetFromHistory(key);
            }

            var state = (int)val;


            double result = -1;
            if (0 == state) result = 0;
            if (1 == state) result = 1;
            if (2 == state) result = 2;
            if (3 == state) result = 2;
            if (8 == state) result = 0;
            if (9 == state) result = 0;
            if (10 == state) result = 0;
            if (11 == state) result = 0;
            if (64 == state) result = 2;
            if (65 == state) result = 2;
            if (66 == state) result = 2;
            if (67 == state) result = 2;
            if (342 == state) result = 2;

            return (double)result;
        }





        protected double GetFromHistory(string key)
        {

            int result = 0;
            cls_IPSIAPI api = new PSI_API.cls_IPSIAPIClass();
            var loginInfo = new typLoginInfo
                                {
                                    strUser = @"Suser",
                                    strPassword = @"Superuser",
                                    strUserClass = @"Suser",
                                    strView = @"psidisplay"
                                };
            try
            {
                api.vbLogin(loginInfo);
                DateTime now = DateTime.Now;

                var values = api.vbGetStdValues(key, now, "MI5", 13);
                foreach (PSI_API.typeValue value in values)
                {
                    if (!IsValueUnknwon(value.Wert))
                    {
                        api.vbLogout();
                        return value.Wert;
                    }
                }

                values = api.vbGetStdValues(key, now, "H1", 24);
                foreach (PSI_API.typeValue value in values)
                {
                    if (!IsValueUnknwon(value.Wert))
                    {
                        api.vbLogout();
                        return value.Wert;
                    }
                }
                api.vbLogout();
            }
            catch (COMException e)
            {
            }

            return -1;
        }


        private Boolean IsValueUnknwon(double val)
        {
            var value = (int)val;
            if (value == 0 || value == 8 || value == 9 || value == 10 || value==11) return true;
            return false;
        }

        public new bool CanBeUsedFor(String tag)
        {
            var match = Regex.Matches(tag, Pattern);
            return match.Count > 0;
        }
         
    }
}