using System.Configuration;

namespace Sofia.Hosting.Configuration
{
    /// <summary>
    /// Представляет описание раздела конфигурационного файла "services".
    /// </summary>
    public class ServicesConfigurationSection : ConfigurationSection
    {
        #region Properties

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>The services.</value>
        [ConfigurationProperty("services", IsDefaultCollection = false)]
        [ConfigurationCollection(
            typeof (ServiceElementCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ServiceElementCollection Services
        {
            get
            {
                var servicesCollection = (ServiceElementCollection) base["services"];
                return servicesCollection;
            }
        }

        #endregion Properties
    }
}