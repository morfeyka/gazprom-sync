using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Scheduling.data
{
    public class BaseConverter:IDataConverter
    {

        protected string Pattern = "";
        

        public string ConverterName { get; protected set; }


        public bool CanBeUsedFor(String tag)
        {
            var match = Regex.Matches(tag, Pattern);
            return match.Count > 0;
        }

        public double Convert(double val,String key)
        {
            return val;
        }

        public static string WildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern).
                               Replace(@"\*", ".*").
                               Replace(@"\?", ".") + "$";
        }

    }
}
