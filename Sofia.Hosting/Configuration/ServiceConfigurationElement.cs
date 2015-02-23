using System.Configuration;

namespace Sofia.Hosting.Configuration
{
    /// <summary>
    /// ������������ �������� ���������������� �������� ������ � �������������� ����� *.config.
    /// </summary>
    public class ServiceConfigurationElement : ConfigurationElement
    {
        #region Ctors

        #endregion

        #region Properties

        /// <summary>
        /// ���������� ��� ����� �������� �������� ������������.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string) this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// ���������� ��� ����� ��� �������� ������������.
        /// </summary>
        [ConfigurationProperty("serviceType", IsRequired = true)]
        public string ServiceType
        {
            get { return (string) this["serviceType"]; }
            set { this["serviceType"] = value; }
        }

        /// <summary>
        /// ���������� ��� ����� ���, ����������� ��������� �������� ������������ <see cref="ServiceType"/>.
        /// </summary>
        [ConfigurationProperty("Contract", IsRequired = true)]
        public string ServiceContract
        {
            get { return (string) this["Contract"]; }
            set { this["Contract"] = value; }
        }
        [ConfigurationProperty("uri", IsRequired = true)]
        public string Uri
        {
            get { return (string)this["uri"]; }
            set { this["uri"] = value; }
        }
        #endregion
    }
}