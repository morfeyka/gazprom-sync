using System;
using System.Collections.Generic;
using System.Linq;

namespace Sofia.Scheduling.data
{
    public sealed class ConverterManager
    {
        readonly List<IDataConverter> _converters;

         private static volatile ConverterManager _instance;
         private static readonly object SyncRoot = new Object();

        private ConverterManager()
        {
            _converters=new List<IDataConverter>
                            {
                                new BptgKcConverter(),
                                new GpaBigConverter(),
                                new GpaConverter(),
                                new KCConverter(),
                                new ValvesConverter()
                            };
        }

       public static ConverterManager Instance
       {
          get 
          {
             if (_instance == null) 
             {
                lock (SyncRoot) 
                {
                   if (_instance == null) 
                      _instance = new ConverterManager();
                }
             }

             return _instance;
          }
       }


        public double GetVestaValue(string tag,double value)
        {
            foreach (var dataConverter in _converters)
            {
                if (dataConverter.CanBeUsedFor(tag))
                    return dataConverter.Convert(value,tag);
            }

            return value;
        }
    }
}