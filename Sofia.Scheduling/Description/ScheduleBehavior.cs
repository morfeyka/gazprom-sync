using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using Sofia.Connect.Proxy;
using Sofia.Contracts;
using Sofia.Contracts.Data;
using Sofia.Contracts.Description;

namespace Sofia.Scheduling.Description
{
    /// <summary>
    /// Представляет описание поведения службы, имеющей контракт, содержащий хотя бы один метод
    /// (отмеченный атрибутом <see cref="ScheduleOperation"/>),
    /// выполняемый планировщиком в соответсвии с некоторым расписанием.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ScheduleBehavior : Attribute, IServiceBehavior
    {
        private readonly HashSet<ScheduleExecutionSchema> _schemes;

        /// <summary>
        /// Выполняет инициализацию нового экземпляра поведения.
        /// </summary>
        public ScheduleBehavior()
        {
            _schemes = new HashSet<ScheduleExecutionSchema>();
        }

        #region Implementation of IServiceBehavior

        /// <summary>
        /// Предоставляет возможности проверки основного приложения и описания службы, чтобы подтвердить готовность службы.
        /// </summary>
        /// <param name="serviceDescription">Описание службы.</param><param name="serviceHostBase">Основное приложение службы, которое создается в настоящий момент.</param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        /// <summary>
        /// Предоставляет возможности передачи настраиваемых данных в элементы привязки для предоставления поддержки реализации контракта.
        /// </summary>
        /// <param name="serviceDescription">Описание службы.</param><param name="serviceHostBase">Основное приложение службы.</param><param name="endpoints">Конечные точки службы.</param><param name="bindingParameters">Настраиваемые объекты, к которым имеют доступ элементы привязки.</param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
                                         Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Предоставляет возможности для изменения значений свойства времени выполнения или для вставки объектов настраиваемых расширений, например, обработчиков ошибок, перехватчиков параметров или сообщений, а также других объектов настраиваемых расширений.
        /// </summary>
        /// <param name="serviceDescription">Описание службы.</param><param name="serviceHostBase">Основное приложение, построение которого выполняется в настоящее время.</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            serviceHostBase.Opened += ServiceHostBaseOpened;
            //serviceHostBase.Closing += ServiceHostBaseClosing;
            foreach (ServiceEndpoint endpoint in serviceHostBase.Description.Endpoints)
            {
                List<MethodInfo> methods =
                    endpoint.Contract.ContractType.GetMethods().Where(
                        x => x.GetCustomAttributes(typeof (ScheduleOperation), false).Length > 0).ToList();
                foreach (MethodInfo meth in methods)
                {
                    var sInfo = (ScheduleOperation) meth.GetCustomAttributes(typeof (ScheduleOperation), false).Single();
                    if (string.IsNullOrEmpty(sInfo.Name))
                        sInfo.Name = string.Format("{0}:{1}", endpoint.Contract.ContractType.FullName, meth.Name);
                    if (sInfo.LaunchesLimit == default(int))
                        sInfo.LaunchesLimit = -1;
                    _schemes.Add(new ScheduleExecutionSchema(sInfo)
                                     {
                                         BindingSchema = endpoint.Binding.Scheme,
                                         MethodName = meth.Name,
                                         Contract = endpoint.Contract.ContractType.FullName,
                                         AssemblyName = endpoint.Contract.ContractType.Assembly.FullName,
                                         Uri = endpoint.Address.Uri.ToString()
                                     });
                }
            }
        }

        #endregion

        private void ServiceHostBaseOpened(object sender, EventArgs e)
        {
            //TODO:ClientsProxyFactory
            //IImportAxapta scheduleClient = ClientsProxyFactory.ImportAxaptaClient;
            //foreach (ScheduleExecutionSchema schema in _schemes)
            //    scheduleClient.AddShedule(sh);.ApplySchedule(schema);
        }

        private void ServiceHostBaseClosing(object sender, EventArgs e)
        {
            //IImportAxapta scheduleClient = ClientsProxyFactory.ImportAxaptaClient;
            //foreach (ScheduleExecutionSchema schema in _schemes)
            //    scheduleClient.DetachSchedule(schema);
        }
    }
}