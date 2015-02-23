using System;
using System.Configuration;

namespace Sofia.Hosting.Configuration
{
    /// <summary>
    /// ������������ ������ ��������� ������������ ���� <see cref="ServiceElementCollection"/>.
    /// </summary>
    public class ServiceElementCollection : ConfigurationElementCollection
    {
        #region Constructors

        #endregion Constructors

        #region Properties

        /// <summary>
        /// ���������� ��� ����� ������� <see cref="ServiceConfigurationElement"/> �� ���������� �������.
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
        /// ���������� ������� �� ���������� <see cref="ServiceConfigurationElement"/> ��������.
        /// </summary>
        public new ServiceConfigurationElement this[string name]
        {
            get { return (ServiceConfigurationElement) BaseGet(name); }
        }

        /// <summary>
        /// ���������� ��� ���� ���������.
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// ��������� �������� ������ �������� ���� <see cref="ServiceConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// ����� ��������� ���� <see cref="ServiceConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceConfigurationElement();
        }

        /// <summary>
        /// ���������� �������� ���� ���������� �������� ������������.
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
        /// ���������� ������ ��������� �������� <see cref="ServiceConfigurationElement"/>.
        /// </summary>
        /// <param name="element">��������� ���� <see cref="ServiceConfigurationElement"/>.</param>
        /// <returns>������ �������� ���� <see cref="ServiceConfigurationElement"/>.</returns>
        public int IndexOf(ServiceConfigurationElement element)
        {
            return BaseIndexOf(element);
        }

        /// <summary>
        /// ��������� ��������� ������� � ���������.
        /// </summary>
        /// <param name="element">������� ���� <see cref="ServiceConfigurationElement"/>.</param>
        public void Add(ServiceConfigurationElement element)
        {
            BaseAdd(element);
        }

        /// <summary>
        /// ��������� ���������� ����������������� �������� ���� <see cref="T:System.Configuration.ConfigurationElementCollection"/> � ���������.
        /// </summary>
        /// <param name="element">������� ���� <see cref="T:System.Configuration.ConfigurationElement"/> ��� ����������.</param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        /// <summary>
        /// ������� ��������� ������� �� ���������.
        /// </summary>
        /// <param name="element">������� ���� <see cref="ServiceConfigurationElement"/>.</param>
        public void Remove(ServiceConfigurationElement element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.Name);
        }

        /// <summary>
        /// ������� ������� ���� <see cref="ServiceConfigurationElement"/> �� ���������� �������.
        /// </summary>
        /// <param name="index">������.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// ������� ������� ���� <see cref="ServiceConfigurationElement"/> �� ���������� ��������.
        /// </summary>
        /// <param name="name">�������� ��������.</param>
        public void Remove(string name)
        {
            BaseRemove(name);
        }

        /// <summary>
        /// ������� ������ ���������.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        #endregion Methods
    }
}