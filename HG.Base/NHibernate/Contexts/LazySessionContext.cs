using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Web;
using NHibernate;
using NHibernate.Context;
using NHibernate.Engine;

namespace HG.Base.NHibernate.Contexts
{
    public class LazySessionContext : CurrentSessionContext, ICurrentSessionContext
    {
        private const string CurrentSessionContextKey = "NHibernateCurrentWebSession";
        private readonly ISessionFactoryImplementor _factory;

        public LazySessionContext(ISessionFactoryImplementor factory)
        {
            _factory = factory;
        }

        #region Implementation of ICurrentSessionContext

        /// <summary>
        /// Retrieve the current session according to the scoping defined
        ///             by this implementation.
        /// </summary>
        /// <returns>
        /// The current session.
        /// </returns>
        /// <exception cref="T:NHibernate.HibernateException">Typically indicates an issue
        ///             locating or creating the current session.</exception>
        public ISession CurrentSession()
        {
            Lazy<ISession> initializer;
            IDictionary<ISessionFactory, Lazy<ISession>> currentSessionFactoryMap = GetCurrentFactoryMap();
            if (currentSessionFactoryMap == null || !currentSessionFactoryMap.TryGetValue(_factory, out initializer))
                return null;
            return initializer.Value;
        }

        /// <summary>
        /// Gets or sets the currently bound session. 
        /// </summary>
        protected override ISession Session
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        public static void Bind(Lazy<ISession> sessionInitializer, ISessionFactory sessionFactory)
        {
            IDictionary<ISessionFactory, Lazy<ISession>> map = GetCurrentFactoryMap();
            map[sessionFactory] = sessionInitializer;
        }

        public static ISession UnBind(ISessionFactory sessionFactory)
        {
            IDictionary<ISessionFactory, Lazy<ISession>> map = GetCurrentFactoryMap();
            Lazy<ISession> sessionInitializer = map[sessionFactory];
            map[sessionFactory] = null;
            if (sessionInitializer == null || !sessionInitializer.IsValueCreated) return null;
            return sessionInitializer.Value;
        }

        private static IDictionary<ISessionFactory, Lazy<ISession>> GetCurrentFactoryMap()
        {
            var currFactoryMap =
                (IDictionary<ISessionFactory, Lazy<ISession>>) (HttpContext.Current != null
                    ? HttpContext.Current.Items[CurrentSessionContextKey]
                    : CallContext.GetData(CurrentSessionContextKey));
            if (currFactoryMap == null)
            {
                currFactoryMap = new Dictionary<ISessionFactory, Lazy<ISession>>();
                if (HttpContext.Current != null)
                    HttpContext.Current.Items[CurrentSessionContextKey] = currFactoryMap;
                else
                    CallContext.SetData(CurrentSessionContextKey, currFactoryMap);
            }
            return currFactoryMap;
        }
    }
}