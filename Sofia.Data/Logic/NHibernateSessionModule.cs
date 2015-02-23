using System;
using System.Web;
using HG.Base.NHibernate.Contexts;
using NHibernate;
using Sofia.Data.Logic;

namespace Sofia.Factelligence.Logic
{
    public class NHibernateSessionModule : IHttpModule
    {
        private HttpApplication _app;
        private ISessionFactory _factory;

        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            _app = context;
            context.BeginRequest += ContextBeginRequest;
            context.EndRequest += ContextEndRequest;
            context.Error += ContextError;
        }

        public void Dispose()
        {
            _app.BeginRequest -= ContextBeginRequest;
            _app.EndRequest -= ContextEndRequest;
            _app.Error -= ContextError;
        }

        #endregion

        private void ContextBeginRequest(object sender, EventArgs e)
        {
            _factory = NHibernateHelper.SessionManager.CurrentSessionFactory;
            LazySessionContext.Bind(new Lazy<ISession>(() => BeginSession(_factory)), _factory);
        }

        private static ISession BeginSession(ISessionFactory sf)
        {
            ISession session = sf.OpenSession();
            session.BeginTransaction();
            return session;
        }

        private void ContextEndRequest(object sender, EventArgs e)
        {
            ISession s = LazySessionContext.UnBind(_factory);
            if (s == null) return;
            NHibernateHelper.SessionManager.CommitSession(s);
            s.Dispose();
        }

        private void ContextError(object sender, EventArgs e)
        {
            ISession s = LazySessionContext.UnBind(_factory);
            if (s == null) return;
            NHibernateHelper.SessionManager.CommitSession(s);
            s.Dispose();
        }
    }
}