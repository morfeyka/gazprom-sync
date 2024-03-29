﻿using System;
using System.Text.RegularExpressions;

namespace Sofia.Scheduling.data
{
    public class KCConverter:BaseConverter,IDataConverter
    {
         public KCConverter()
         {
             ConverterName = "КС Портовая";
             Pattern = @"T09\.UT13\.KS01\.KC\.{2}M_KC[1-2]";
         }

        public new double Convert(double val,string key)
        {
            var state = (int)val;
            double result = 0;
            if (0 == state) result = 0;
            if (1 == state) result = 3;
            if (2 == state) result = 1;
            if (4 == state) result = 0;
            if (8 == state) result = 5;
            if (16 == state) result = 3;
            if (32 == state) result = 0;
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