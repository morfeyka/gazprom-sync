using System;
using System.Text.RegularExpressions;

namespace Sofia.Scheduling.data
{
    public class GpaBigConverter:BaseConverter,IDataConverter
    {
         public GpaBigConverter()
         {
             ConverterName = "ГПА Вохов, Елизаветинская,Пикалево";
             Pattern = @"T09\.UT0[0-9]\.KS\.GPA.*\.\.CA01";
         }

        public new double Convert(double val,string key)
        {
            var state = (int)val;
            double result = 0;
            if (6 == state) result = 0;
            if (7 == state) result = 0;
            if (8 == state) result = 0;
            if (9 == state) result = 0;
            if (10 == state) result = 0;
            if (11 == state) result = 0;
            if (12 == state) result = 0;

            if (13 == state) result = 1;
            if (14 == state) result = 1;
            if (15 == state) result = 1;
            if (16 == state) result = 1;

            if (17 == state) result = 0;

            if (18 == state) result = 4;
            if (19 == state) result = 4;

            if (20 == state) result = 2;
            if (21 == state) result = 2;
            if (22 == state) result = 2;
            if (23 == state) result = 2;

            if (24 == state) result = 0;
            if (25 == state) result = 0;
            if (26 == state) result = 0;
            if (27 == state) result = 0;

            if (28 == state) result = 2;

            if (29 == state) result = 0;
            if (30 == state) result = 4;
            if (30 == state) result = 4;
            if (342 == state) result = 0;
            if (30 == state) result = 4;
            if (31 == state) result = 4;

            return (double)result;
        }

        public new bool CanBeUsedFor(String tag)
        {
            var match = Regex.Matches(tag, Pattern);
            return match.Count > 0;
        }
    }
}