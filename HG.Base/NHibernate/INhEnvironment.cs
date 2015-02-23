using System;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;

namespace HG.Base.NHibernate
{
    public interface INhEnvironment
    {
        string SessionFactoryName { get; }
        HbmMapping GetHbmMapping();
        Func<Configuration> ApplySettings();
    }
}