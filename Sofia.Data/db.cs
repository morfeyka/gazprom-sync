using System;
using System.Linq;
using System.Reflection;
using HG.Base.NHibernate;
using NHibernate;
using NHibernate.Linq;
using Sofia.Data.Logic;

namespace Sofia.Data
{
    /// <summary>
    /// Предоставляет функциональные возможности расчета запросов к конкретному источнику
    /// данных с известным типом данных.
    /// </summary>
    public static class Db<TEnvironment> where TEnvironment : class, INhEnvironment, new()
    {
        /// <summary>
        /// Возвращает NHibernate Session 
        /// </summary>
        public static ISession NSession
        {
            get { return NHibernateHelper.SessionManager.GetSessionFor<TEnvironment>(); }
        }

        /// <summary>
        /// Предоставляет функциональные возможности расчета запросов к конкретному источнику
        /// данных с известным типом данных.
        /// </summary>
        /// <typeparam name="TSource">Тип данных в источнике данных.</typeparam>
        /// <returns></returns>
        public static IQueryable<TSource> Get<TSource>()
        {
            return NHibernateHelper.SessionManager.GetSessionFor<TEnvironment>().Query<TSource>();
        }


        /// <summary>
        /// Предоставляет функциональные возможности расчета запросов к конкретному источнику
        /// данных с известным типом данных.
        /// </summary>
        /// <param name="type">Тип данных в источнике данных.</param>
        /// <returns></returns>
        public static IQueryable Get(Type type)
        {
            MethodInfo method = typeof (LinqExtensionMethods).GetMethod("Query").MakeGenericMethod(type);
            dynamic result = method.Invoke(null, new object[] { NHibernateHelper.SessionManager.GetSessionFor<TEnvironment>() });
            return result;
        }

        public static void Save(object entity)
        {
            if (entity == null) return;
            ISession session = NSession;
            session.Save(entity);
            session.Flush();
        }

        public static void Delete(object entity)
        {
            if (entity == null) return;
            ISession session = NSession;
            session.Delete(entity);
            session.Flush();
        }
    }
}