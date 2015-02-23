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
    /// ������������ �������� ��������� ������, ������� ��������, ���������� ���� �� ���� �����
    /// (���������� ��������� <see cref="ScheduleOperation"/>),
    /// ����������� ������������� � ����������� � ��������� �����������.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ScheduleBehavior : Attribute, IServiceBehavior
    {
        private readonly HashSet<ScheduleExecutionSchema> _schemes;

        /// <summary>
        /// ��������� ������������� ������ ���������� ���������.
        /// </summary>
        public ScheduleBehavior()
        {
            _schemes = new HashSet<ScheduleExecutionSchema>();
        }

        #region Implementation of IServiceBehavior

        /// <summary>
        /// ������������� ����������� �������� ��������� ���������� � �������� ������, ����� ����������� ���������� ������.
        /// </summary>
        /// <param name="serviceDescription">�������� ������.</param><param name="serviceHostBase">�������� ���������� ������, ������� ��������� � ��������� ������.</param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        /// <summary>
        /// ������������� ����������� �������� ������������� ������ � �������� �������� ��� �������������� ��������� ���������� ���������.
        /// </summary>
        /// <param name="serviceDescription">�������� ������.</param><param name="serviceHostBase">�������� ���������� ������.</param><param name="endpoints">�������� ����� ������.</param><param name="bindingParameters">������������� �������, � ������� ����� ������ �������� ��������.</param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
                                         Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// ������������� ����������� ��� ��������� �������� �������� ������� ���������� ��� ��� ������� �������� ������������� ����������, ��������, ������������ ������, ������������� ���������� ��� ���������, � ����� ������ �������� ������������� ����������.
        /// </summary>
        /// <param name="serviceDescription">�������� ������.</param><param name="serviceHostBase">�������� ����������, ���������� �������� ����������� � ��������� �����.</param>
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