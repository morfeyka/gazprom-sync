using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Sofia.Connect.State;
using Sofia.Contracts;
using Sofia.Dto;

namespace Sofia.Connect.Proxy.Clients
{
    public delegate void ServiceFailedEventHandler(object sender, ServiceDetailArgs args);

    public class ImportAxaptaClient : ClientBase<IImportAxapta>, IImportAxapta
    {
        protected EndpointAddress RemoteAddress;

        public ImportAxaptaClient(Binding binding, EndpointAddress endpointAddress)
            : base(binding, endpointAddress)
        {
            RemoteAddress = endpointAddress;
            InnerChannel.Opening += InnerChannelOpening;
            InnerChannel.Faulted += InnerChannelFaulted;
        }

        /// <summary>
        /// Событие, возникающее после перехода коммуникационного стэка в состояние <see cref="CommunicationState.Faulted"/>.
        /// </summary>
        public event ServiceFailedEventHandler Failed;

        protected void OnFailed(object sender, ServiceDetailArgs e)
        {
            if (Failed != null)
                Failed(sender, e);
        }

        private void InnerChannelFaulted(object sender, EventArgs e)
        {
            OnFailed(this, new ServiceDetailArgs(typeof(IImportAxapta).Name));
        }

        private void InnerChannelOpening(object sender, EventArgs e)
        {
            if (!TcpSocket.IsOpenPort(RemoteAddress.Uri))
                throw new EndpointNotFoundException("Service connection timeout. Server not responds.");
        }

        public void AddShedule(int id)
        {
            Channel.AddShedule(id);
        }

        public List<SchedulerDto> GetSummaryInfo()
        {
            return Channel.GetSummaryInfo();
        }

        /// <summary>
        /// Выполняет удаление задания из списока задач шедулера.
        /// </summary>
        /// <param name="id"></param>
        public void RemoveShedule(int id)
        {
            Channel.RemoveShedule(id);
        }

        /// <summary>
        /// Выполняет остановку выполнения задания содержащегося в списке задач.
        /// </summary>
        /// <param name="id"></param>
        public void KillShedule(int id)
        {
            Channel.KillShedule(id);
        }
    }
}
