using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using HG.Base.NHibernate;
using HG.Base.NHibernate.Collections;
using HG.Base.NHibernate.Contexts;
using NHibernate.Bytecode;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using Sofia.Domain.Setting;
using Sofia.Mapping.Setting;
using Environment = NHibernate.Cfg.Environment;

namespace Sofia.Data.Logic
{
    public class WebEnvironmentFiact : INhEnvironment
    {
        #region Implementation of INhEnvironment

        public string SessionFactoryName
        {
            get { return "webAppFactelligence"; }
        }

        public HbmMapping GetHbmMapping()
        {
            var mapper = new ModelMapper();
            List<Type> mappings =
                typeof (EntityMapper<>).Assembly.GetExportedTypes().Where(x => !x.IsAbstract || !x.IsInterface).Where(
                    x => x.BaseType != null).ToList();
            mapper.AddMappings(mappings);
            IEnumerable<Type> entities =
                typeof (EntityWithTypedId<>).Assembly.GetExportedTypes().Where(x => !x.IsInterface && x.BaseType != null);
            return mapper.CompileMappingFor(entities);
        }

        public Func<Configuration> ApplySettings()
        {
            return (() => new Configuration()
                              .CollectionTypeFactory<Net4CollectionTypeFactory>()
                              .DataBaseIntegration(db =>
                                                       {
                                                           db.Dialect<MsSql2008Dialect>();
                                                           db.Driver<SqlClientDriver>();
                                                           db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                                                           db.IsolationLevel = IsolationLevel.ReadCommitted;
                                                           db.BatchSize = 100;
                                                           db.Timeout = 180;
                                                           db.ConnectionStringName =
                                                               AdoHelper.WebConnectionNameFactelligence;
                                                       })
                              .Proxy(p => p.ProxyFactoryFactory<DefaultProxyFactoryFactory>())
                              .SetProperty(Environment.UseSecondLevelCache, "true")
                              //.SetInterceptor(new EventListener())
                              .CurrentSessionContext<LazySessionContext>());
        }

        #endregion
    }
}