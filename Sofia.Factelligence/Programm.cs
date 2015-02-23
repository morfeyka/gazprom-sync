using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using Sofia.Contracts;
using Sofia.Contracts.Data;
using Sofia.Hosting.Configuration;
using Sofia.Maintenance;

//using FactMES.Client.Common;

namespace Sofia.Factelligence
{
    public class Programm
    {
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
                host.Faulted += new EventHandler(host_Faulted);
                host.UnknownMessageReceived += new EventHandler<UnknownMessageReceivedEventArgs>(host_UnknownMessageReceived);
                host.ManualFlowControlLimit = int.MaxValue;
                host.Closing += new EventHandler(host_Closing);
                _hosts.Add(host);
            }

            void host_Closing(object sender, EventArgs e)
            {
                throw new NotImplementedException();
            }

            void host_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
            {
                throw new NotImplementedException();
            }

            void host_Faulted(object sender, EventArgs e)
            {
                throw new NotImplementedException();
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
        private static void Main()
        {
            var shUri = new Uri("net.tcp://127.0.0.1:812/IImportAxapta");
            var host = new HostContainer();
            host.StartServices("Жопа");
            //shHost.Open();
            Console.WriteLine("Шудулер запущен. Ждём 3 секунды");
           // Thread.CurrentThread.Join(TimeSpan.FromSeconds(3));
            //Console.WriteLine("Запуск зависимой службы");
            //ServiceHost host = new ServiceHost(typeof(PricelistLoader), new[] { uri });
            //host.Open();
            var binding = new NetTcpBinding();
            //binding.CloseTimeout = new TimeSpan(0,10,0);
            //binding.OpenTimeout = new TimeSpan(0, 10, 0);
            //binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            //binding.SendTimeout = new TimeSpan(0, 10, 0);
            //binding.ReliableSession.Enabled = false;
            //binding.ReliableSession.InactivityTimeout = new TimeSpan(0, 10, 0);
            //binding.ReliableSession.Ordered = true;
            //binding.MaxConnections = 10;
            NetTcpBinding setting = new NetTcpBinding();
            setting.CloseTimeout = new TimeSpan(0, 1, 0);
            setting.OpenTimeout = new TimeSpan(0, 1, 0);
            setting.ReceiveTimeout = new TimeSpan(0, 10, 0);
            setting.SendTimeout = new TimeSpan(0, 1, 0);
            setting.TransactionFlow = false;
            setting.TransferMode = TransferMode.Buffered;
            setting.TransactionProtocol = TransactionProtocol.OleTransactions;
            setting.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            setting.ListenBacklog = 10;
            setting.MaxBufferPoolSize = 2147483647;
            setting.MaxBufferSize = 2147483647;
            setting.MaxConnections = 10;
            setting.MaxReceivedMessageSize = 2147483647;
            setting.ReaderQuotas.MaxDepth = 32;
            setting.ReaderQuotas.MaxStringContentLength = 800000000;
            setting.ReaderQuotas.MaxArrayLength = 800000000;
            setting.ReaderQuotas.MaxBytesPerRead = 4096;
            setting.ReaderQuotas.MaxNameTableCharCount = 16384;
            setting.ReliableSession.Enabled = false;
            setting.ReliableSession.Ordered = true;
            setting.ReliableSession.InactivityTimeout = new TimeSpan(0, 10, 0);
            //setting.Security.Mode = SecurityMode.None;
            setting.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            setting.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;
            setting.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
        StartOther:
            try
            {
                
            using (var factory = new ChannelFactory<IImportAxapta>(setting, new EndpointAddress(shUri)))
            {
                IImportAxapta channel = factory.CreateChannel();
                factory.Faulted += new EventHandler(factory_Faulted);
            while (true)
            {
                


             
                //Thread.CurrentThread.Join(TimeSpan.FromSeconds(3));
                var list = channel.GetSummaryInfo();
                foreach (var item in list)
                    Console.WriteLine("{0} | {1} | {2} | {3}", item.State, item.LastRun, item.NextRun, item.State);

                Thread.CurrentThread.Join(TimeSpan.FromSeconds(5));
                list = channel.GetSummaryInfo();
                foreach (var item in list)
                    Console.WriteLine("{0} | {1} | {2} | {3}", item.State, item.LastRun, item.NextRun, item.State);

                Thread.CurrentThread.Join(TimeSpan.FromSeconds(9));
                list = channel.GetSummaryInfo();
                foreach (var item in list)
                    Console.WriteLine("{0} | {1} | {2} | {3}", item.State, item.LastRun, item.NextRun, item.State);
            }
           
           }
            }
            catch (Exception e)
            {

                goto StartOther;
            }

            //using (var session = NHibernateSessionManager.Instance.SessionFactory.OpenSession())
            //{
            //    var upl = new OriginalsUploader<RtPlanBuy, PlanItemBuy>(session);
            //    //var task = session.Get<PriceListTask>(39);
            //    var task = upl.SetUploaderTaskOnDelete(13254);
            //    upl.LoadByTask(ref task);
            //}

            //host.Close();
            host.StopServices();
        }

        static void factory_Faulted(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}