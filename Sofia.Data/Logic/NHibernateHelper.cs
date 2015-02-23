using HG.Base.NHibernate;
using NHibernate.Cfg;

namespace Sofia.Data.Logic
{
    public class NHibernateHelper
    {
        private static readonly NHibernateHelper Helper = new NHibernateHelper();
        private static NhSessionManager _sessionManager;
        private static NHibernateReflector _reflector;

        private NHibernateHelper()
        {
        }


        public static NhSessionManager SessionManager
        {
            get
            {
                if (_sessionManager == null)
                {
                    lock (Helper)
                    {
                        _sessionManager = NhSessionManager.Instance<WebEnvironmentFiact>();
                    }
                }
                return _sessionManager;
            }
        }

        public static NHibernateReflector Reflector
        {
            get
            {
                if (_reflector == null)
                {
                    Configuration cfg = SessionManager.CurrentConfiguration;
                    _reflector = new NHibernateReflector(cfg);
                }
                return _reflector;
            }
        }
    }
}