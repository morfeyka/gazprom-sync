using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using Sofia.Hosting.Configuration;

namespace Sofia.Hosting
{
    /// <summary>
    /// Класс, выполняющий запуск хостов, определённых в конфигурационном файле приложения.
    /// </summary>
    public class HostContainer
    {
        #region Fields

        private readonly List<ServiceHost> _hosts = new List<ServiceHost>();

        #endregion Fields

        #region Ctors

        #endregion

        #region Methods

        /// <summary>
        /// Выполняет запуск зависимых служб на указанном хосте.
        /// </summary>
        public void StartServices(string winHostName)
        {
            // Получаем список сервисов
            var servicesSection = ConfigurationManager.GetSection("serviceSettings") as ServicesConfigurationSection;
            if (servicesSection == null)
                throw new ConfigurationErrorsException("Failed to load 'serviceSettings' configuration section.");
            foreach (ServiceConfigurationElement serviceElement in servicesSection.Services)
            {
                Uri[] addr = new Uri[] { new Uri(serviceElement.Uri) };
                Type serviceType = Type.GetType(serviceElement.ServiceType, true, true);
                if (serviceType == null)
                    throw new TypeLoadException("Failed to get service type '" + serviceElement.ServiceType + "'.");
                // Открывам хоста
                OpenHost(serviceType, addr);
            }
        }

        /// <summary>
        /// Выполняет останов зависимых служб.
        /// </summary>
        public void StopServices()
        {
            foreach (ServiceHost host in _hosts)
                CloseHost(host);
        }

        /// <summary>
        /// Gets the hosted service names.
        /// </summary>
        /// <returns>string[] of service names</returns>
        public string[] GetHostedServiceNames()
        {
            var names = new List<string>();
            foreach (ServiceHost host in _hosts)
            {
                var svcInfo = new StringBuilder();
                svcInfo.AppendLine(host.Description.ConfigurationName);
                foreach (ServiceEndpoint endPoint in host.Description.Endpoints)
                    svcInfo.AppendLine(endPoint.ListenUri.ToString());
                names.Add(svcInfo.ToString());
            }
            return names.ToArray();
        }

        #endregion Methods

        #region Helper Methods

        private void OpenHost(Type hostType, Uri[] points)
        {
            var host = new ServiceHost(hostType, points);
            host.Open();
            _hosts.Add(host);
        }

        private void CloseHost(ServiceHost host)
        {
            try
            {
                if (host != null)
                    host.Close();
            }
            catch
            {
                _hosts.Clear();
            }
        }

        #endregion Helper Methods
    }
}