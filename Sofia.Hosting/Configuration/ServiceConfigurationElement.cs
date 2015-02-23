using System.Configuration;

namespace Sofia.Hosting.Configuration
{
    /// <summary>
    /// Представляет описание конфигуационного элемента службы в соответсвующем файле *.config.
    /// </summary>
    public class ServiceConfigurationElement : ConfigurationElement
    {
        #region Ctors

        #endregion

        #region Properties

        /// <summary>
        /// Возвращает или задаёт название элемента конфигурации.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string) this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Возвращает или задаёт тип элемента конфигурации.
        /// </summary>
        [ConfigurationProperty("serviceType", IsRequired = true)]
        public string ServiceType
        {
            get { return (string) this["serviceType"]; }
            set { this["serviceType"] = value; }
        }

        /// <summary>
        /// Возвращает или задаёт тип, реализуемый значением элемента конфигурации <see cref="ServiceType"/>.
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