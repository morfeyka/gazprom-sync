using System;
using System.Collections;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using Environment = NHibernate.Cfg.Environment;

namespace HG.Base.NHibernate
{
    public class NhSessionManager
    {
        private static ISessionFactory _factory;
        private static NhSessionManager _manager;
        private static Hashtable _factories;
        private static Configuration _configuration;
        private static volatile object _sinchro = new object();
        
        private NhSessionManager()
        {
            _factories = new Hashtable();
        }

        public ISessionFactory CurrentSessionFactory
        {
            get { return _factory; }
        }

        public Configuration CurrentConfiguration
        {
            get { return _configuration; }
        }

        public ISession CurrentSession
        {
            get
            {
                var session = _factory.GetCurrentSession();
                return session ?? _factory.OpenSession();

                //if (CurrentSessionContext.HasBind(_factory))
                //    return _factory.GetCurrentSession();
                //ISession session = _factory.OpenSession();
                //CurrentSessionContext.Bind(session);
                //return session;
            }
        }

        public static NhSessionManager Instance<TEnvironment>() where TEnvironment : class, INhEnvironment, new()
        {
            if (_manager == null)
            {
                lock (_sinchro)
                {
                    _manager = new NhSessionManager();
                    _factory = GetFactory<TEnvironment>();
                }
            }
            return _manager;
        }

        public ISession GetSessionFor<TEnvironment>() where TEnvironment : class, INhEnvironment, new()
        {
            _factory = GetFactory<TEnvironment>();
            return CurrentSession;
        }

        private static ISessionFactory GetFactory<TEnvironment>() where TEnvironment : class, INhEnvironment, new()
        {
            string fType = typeof (TEnvironment).FullName;
            if (fType == null) return null;
            if (!_factories.ContainsKey(fType))
            {
                var environment = new TEnvironment();
                _configuration = environment.ApplySettings().Invoke()
                    .SetProperty(Environment.SessionFactoryName, environment.SessionFactoryName).
#if DEBUG
                    SetProperty(Environment.ShowSql, "true");
#else
                SetProperty(Environment.ShowSql, "false");
#endif

                _configuration.AddDeserializedMapping(environment.GetHbmMapping(), "Mappings");
                _factory = _configuration.BuildSessionFactory();
                _factories.Add(fType, _factory);
                return _factory;
            }
            return (ISessionFactory) _factories[fType];
        }

        public void CloseSession()
        {
            if (_factory == null)
                return;
            if (CurrentSessionContext.HasBind(_factory))
            {
                ISession session = CurrentSessionContext.Unbind(_factory);
                session.Close();
            }
        }

        public void CommitSession(ISession session)
        {
            if (session.Transaction != null && session.Transaction.IsActive)
                try
                {
                    session.Transaction.Commit();
                }
                catch (Exception)
                {
                    session.Transaction.Rollback();
                    throw;
                }
        }
    }
}