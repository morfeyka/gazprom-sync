using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using PSI_API;

namespace Sofia.Scheduling.data
{
    public class ValvesConverter:BaseConverter,IDataConverter
    {
         public ValvesConverter()
         {
             ConverterName = "Обычные краны";
             Pattern = @"T09\.UT.*\.*\.V.*";
         }
        
        public new double ConvertRegular(double val,string key)
        {
 

            

            var state = (int)val;
            
            double result = -1;
            if (0 == state) result=0;
            if (1 == state) result=1;
            if (2 == state) result=2;
            if (3 == state) result=2;
            if (4 == state) result = 0;
            if (8 == state) result=2;
            if (9 == state) result=1;
            if (10 == state) result=2;
            if (12== state) result=2;
            if (16 == state) result=0;
            if (24 == state) result=2;
            if (32 == state) result=0;
            if (40 == state) result=2;
            if (64 == state) result=2;
            if (65 == state) result=2;
            if (66 == state) result=2;
            if (72 == state) result=2;
            if (128 == state) result=2;
            if (342 == state) result=2;

            if (IsValueUnknwon(result))
            {
                result = GetFromHistory(key,"general");
            }

            return result;
        }


        public new double ConvertSLTM(double val, string key)
        {

            

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

            if (IsValueUnknwon(result))
            {
                result = GetFromHistory(key,"sltm");
            }

            return (double)result;
        }

        public new double ConvertPIK(double val, string key)
        {

           

            var state = (int)val;


            double result = -1;
            if (0 == state) result = 0;
            if (1 == state) result = 1;
            if (2 == state) result = 2;
            if (3 == state) result = 2;
            if (4 == state) result = 0;
            if (8 == state) result = 0;
            if (9 == state) result = 0;
            if (10 == state) result = 0;
            if (12 == state) result = 0;
            if (16 == state) result = 0;
            if (17 == state) result = 0;
            if (18 == state) result = 0;
            if (20 == state) result = 0;
            if (state >=32) result = 2;


            if (IsValueUnknwon(result))
            {
                result = GetFromHistory(key,"pik");
            }

            return (double)result;
        }

        public new double Convert(double val,string key)
        {
            const string regSltm = @"T09\.UT[0-9][0-9]\.SLTM\.KP.*\.V.*";
            const string regPik = @"T09\.UT05\.KS\..*\.V.*"; 
            
            if (Regex.Matches(key,regSltm).Count>0)
            {
                return ConvertSLTM(val,key);
            }
            if (Regex.Matches(key, regPik).Count > 0)
            {
                return ConvertPIK(val, key);
            }

            return ConvertRegular(val, key);
        }



        protected double GetFromHistory(string key,String mode)
        {

            int result = 0;
            cls_IPSIAPI api = new PSI_API.cls_IPSIAPIClass();
            try
            {
                var loginInfo = new typLoginInfo
                                    {
                                        strUser = @"Suser",
                                        strPassword = @"Superuser",
                                        strUserClass = @"Suser",
                                        strView = @"psidisplay"
                                    };
                api.vbLogin(loginInfo);
                DateTime now = DateTime.Now;

                var values = api.vbGetStdValues(key, now, "MI5", 13);
                foreach (PSI_API.typeValue value in values)
                {
                    bool isOk = false;
                    if (mode == "general")
                    {
                        isOk = !IsValueUnknwonGeneral(value.Wert);
                    }
                    if (mode == "pik")
                    {
                        isOk = !IsValueUnknwonPIK(value.Wert);
                    }
                    if (mode == "sltm")
                    {
                        isOk = !IsValueUnknwonSLTM(value.Wert);
                    }
                
                    if (isOk) {
                        api.vbLogout();
                        return value.Wert;
                    }
                }

                values = api.vbGetStdValues(key, now, "H1", 24);
                foreach (PSI_API.typeValue value in values)
                {

                   bool  isOk = false;
                    if (mode == "general")
                    {
                        isOk = !IsValueUnknwonGeneral(value.Wert);
                    }
                    if (mode == "pik")
                    {
                        isOk = !IsValueUnknwonPIK(value.Wert);
                    }
                    if (mode == "sltm")
                    {
                        isOk = !IsValueUnknwonSLTM(value.Wert);
                    }

                    if (isOk)
                    {
                        api.vbLogout();
                        return value.Wert;
                    }
                }
                api.vbLogout();
            } catch (COMException e)
            {
            }
            return -1;
        }


        private Boolean IsValueUnknwon(double val)
        {
            var value =(int) val;
            if (value == 0) return true;
            return false;
        }

        private Boolean IsValueUnknwonGeneral(double val)
        {
            var state = (int)val;

            int result = -1;
            if (0 == state) result = 0;
            if (1 == state) result = 1;
            if (2 == state) result = 2;
            if (3 == state) result = 2;
            if (4 == state) result = 0;
            if (8 == state) result = 2;
            if (9 == state) result = 2;
            if (10 == state) result = 2;
            if (12 == state) result = 2;
            if (16 == state) result = 0;
            if (24 == state) result = 2;
            if (32 == state) result = 0;
            if (40 == state) result = 2;
            if (64 == state) result = 2;
            if (65 == state) result = 2;
            if (66 == state) result = 2;
            if (72 == state) result = 2;
            if (128 == state) result = 2;
            if (342 == state) result = 2;
            if (result == 0) return true;
            return false;
        }


        private Boolean IsValueUnknwonSLTM(double val)
        {
            var state = (int)val;

            int result = -1;
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
            if (result == 0) return true;
            return false;
        }

        private Boolean IsValueUnknwonPIK(double val)
        {
            var state = (int)val;

            int result = -1;
            if (0 == state) result = 0;
            if (1 == state) result = 1;
            if (2 == state) result = 2;
            if (3 == state) result = 2;
            if (4 == state) result = 0;
            if (8 == state) result = 0;
            if (9 == state) result = 0;
            if (10 == state) result = 0;
            if (12 == state) result = 0;
            if (16 == state) result = 0;
            if (17 == state) result = 0;
            if (18 == state) result = 0;
            if (20 == state) result = 0;
            if (state >= 32) result = 2;
            if (result == 0) return true;
            return false;
        }

        public new bool CanBeUsedFor(String tag)
        {
            var match = Regex.Matches(tag, Pattern);
            return match.Count > 0;
        }

    }
}