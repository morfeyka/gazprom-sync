using System;
using System.Collections.Generic;
using HG.Base.NHibernate.Domain;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Mapping;

namespace HG.Base.NHibernate
{
    /// <summary>
    /// Представляет хэлпер, содержащий распространнённые операции рефлексии с сущностями NHibernate.
    /// </summary>
    public class NHibernateReflector
    {
        private readonly Configuration _configuration;

        public NHibernateReflector(Configuration configuration)
        {
            _configuration = configuration;
        }

        ///<summary>
        /// Выполняет генерацию и присвоение идентификатора указанному объекту по алгоритму HiLo.
        /// (Не использовать, если в маппинге сущности указан другой генератор Id!!!!)
        ///</summary>
        /// <remarks>
        /// Не использовать, если в маппинге сущности указан другой генератор Id.
        /// </remarks>
        ///<param name="target">Экземпляр, которому требуется сгенерировать Id.</param>
        ///<param name="session"></param>
        public void GenerateIdentifier(object target, ISession session)
        {
            Type tType = target.GetType();
            PersistentClass mapping = _configuration.GetClassMapping(tType);
            ISessionImplementor impl = session.GetSessionImplementation();
            object newId =
                mapping.Identifier.CreateIdentifierGenerator(impl.Factory.Dialect, mapping.Table.Catalog,
                                                             mapping.Table.Schema, mapping.RootClazz).Generate(impl,
                                                                                                               target);
            mapping.IdentifierProperty.GetSetter(tType).Set(target, newId);
            return;
        }

        /// <summary>
        /// Выполняет присвоение значения идентификатора указанному объекту.
        /// </summary>
        /// <param name="target">Объект.</param>
        /// <param name="id">Значение идентификатора.</param>
        public void SetIdentifier(object target, object id)
        {
            Type tType = target.GetType();
            PersistentClass mapping = _configuration.GetClassMapping(tType);
            mapping.IdentifierProperty.GetSetter(tType).Set(target, id);
        }

        /// <summary>
        /// Извлекает значение свойства указанного объекта через рефлексию.
        /// </summary>
        /// <param name="target">Объект, содержащий указанное свойство.</param>
        /// <param name="propName">Свойство, значение которого требуется извлечь.</param>
        /// <param name="targetType">Тип <see cref="target"/> (Данный параметр рекомендуется использовать,
        /// если <see cref="target"/> является прокси-объектом).</param>
        /// <returns>Извлечённое значение свойства.</returns>
        public object RetrievePropValue(object target, string propName, Type targetType = null)
        {
            Type tType = targetType ?? target.GetType();
            PersistentClass mapping = _configuration.GetClassMapping(tType);
            return mapping.IdentifierProperty.Name == propName
                       ? mapping.IdentifierProperty.GetGetter(tType).Get(target)
                       : mapping.GetProperty(propName).GetGetter(tType).Get(target);
        }

        /// <summary>
        /// Возвращает набор свойств сущности указанного типа (К данным свойствам идентификатор не относится).
        /// </summary>
        /// <param name="entityType">Тип <see cref="IEntityWithTypedId{TId}"/> объекта.</param>
        /// <returns>Набор свойств объекта.</returns>
        public IEnumerable<Property> GetPropertyIterator(Type entityType)
        {
            PersistentClass mapping = _configuration.GetClassMapping(entityType);
            return mapping.PropertyIterator;
        }
    }
}