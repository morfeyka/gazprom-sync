using System;
using System.Text.RegularExpressions;

namespace Sofia.Scheduling.data
{
    public class GpaConverter:BaseConverter,IDataConverter
    {
        public GpaConverter ()
        {
            ConverterName = "ГПА Портовая";
            Pattern = @"T09\.UT13\.KS01\.GPA[0-9]_[0-9]\.\.M_GPA";
        }

        public new double Convert(double val,string key)
        {
            var state = (int)val;
            double result = 0;
            if (0 == state) result = 0;
            if (1 == state) result = 0;
            if (2 == state) result = 0;
            if (4 == state) result = 0;
            if (8 == state) result = 0;
            
            if (10 == state) result = 4;
            
            if (16 == state) result = 1;
            if (21 == state) result = 0;
            if (32 == state) result = 0;
            if (34 == state) result = 0;
            if (64 == state) result = 0;
            if (66 == state) result = 0;
            
            
            
            
            if (128 == state) result = 1;
            if (129 == state) result = 1;
            if (342 == state) result = 0;

            return (double)result;
        }

        public new bool CanBeUsedFor(String tag)
        {
            var match = Regex.Matches(tag, Pattern);
            return match.Count > 0;
        }
         
    }
}