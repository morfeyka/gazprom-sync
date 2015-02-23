using System;
using System.Configuration;

namespace Sofia.Hosting.Configuration
{
    /// <summary>
    /// Представляет список элементов конфигурации типа <see cref="ServiceElementCollection"/>.
    /// </summary>
    public class ServiceElementCollection : ConfigurationElementCollection
    {
        #region Constructors

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Возвращает или задаёт элемент <see cref="ServiceConfigurationElement"/> по указанному индексу.
        /// </summary>
        public ServiceConfigurationElement this[int index]
        {
            get { return (ServiceConfigurationElement) BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Возвращает элемент по указанному <see cref="ServiceConfigurationElement"/> названию.
        /// </summary>
        public new ServiceConfigurationElement this[string name]
        {
            get { return (ServiceConfigurationElement) BaseGet(name); }
        }

        /// <summary>
        /// Возвращает тип этой коллекции.
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Выполняет создание нового элемента типа <see cref="ServiceConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// Новый экземпляр типа <see cref="ServiceConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceConfigurationElement();
        }

        /// <summary>
        /// Возвращает ключевое поле указанного элемента конфигурации.
        /// </summary>
        /// <param name="element">The <see cref="ServiceConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="ServiceConfigurationElement"/>.
        /// </returns>
        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceConfigurationElement) element).Name;
        }

        /// <summary>
        /// Возвращает индекс заданного элемента <see cref="ServiceConfigurationElement"/>.
        /// </summary>
        /// <param name="element">Экземпляр типа <see cref="ServiceConfigurationElement"/>.</param>
        /// <returns>Индекс элемента типа <see cref="ServiceConfigurationElement"/>.</returns>
        public int IndexOf(ServiceConfigurationElement element)
        {
            return BaseIndexOf(element);
        }

        /// <summary>
        /// Добавляет указанный элемент в коллекцию.
        /// </summary>
        /// <param name="element">Элемент типа <see cref="ServiceConfigurationElement"/>.</param>
        public void Add(ServiceConfigurationElement element)
        {
            BaseAdd(element);
        }

        /// <summary>
        /// Выполняет добавление конфигурационного элемента типа <see cref="T:System.Configuration.ConfigurationElementCollection"/> в коллекцию.
        /// </summary>
        /// <param name="element">Элемент типа <see cref="T:System.Configuration.ConfigurationElement"/> для добавления.</param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        /// <summary>
        /// Удаляет указанный элемент из коллекции.
        /// </summary>
        /// <param name="element">Элемент типа <see cref="ServiceConfigurationElement"/>.</param>
        public void Remove(ServiceConfigurationElement element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.Name);
        }

        /// <summary>
        /// Удаляет элемент типа <see cref="ServiceConfigurationElement"/> по указанному индексу.
        /// </summary>
        /// <param name="index">Индекс.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Удаляет элемент типа <see cref="ServiceConfigurationElement"/> по указанному названию.
        /// </summary>
        /// <param name="name">Название элемента.</param>
        public void Remove(string name)
        {
            BaseRemove(name);
        }

        /// <summary>
        /// Очищает данную коллекцию.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        #endregion Methods
    }
}