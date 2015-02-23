using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Sofia.Dto;
using Sofia.Scheduling.Core;
using Sofia.Contracts.Data;

namespace Sofia.Scheduling.Schedulers
{
    /// <summary>
    /// ������������ �������� ������� ��� ��������������� ������ �������� ��������� ����� �������.
    /// </summary>
    /// <typeparam name="TChanel">��������, ����������� ��������� ������.</typeparam>
    public class EndpointsLauncher<TChanel> : ISchedulingContractInfo
    {
        private readonly ScheduleExecutionSchema _executionSchema;
        private readonly ScheduleInstance _heart;

        #region Properties

        #region Implementation of IExecutionState

        /// <summary>
        /// ���������� ���� ���������� ������� �� ����������.
        /// </summary>
        public DateTime? LastRun { get; set; }

        /// <summary>
        /// ���������� ���� ������������ ������� �� ����������.
        /// </summary>
        public DateTime NextRun { get; set; }

        /// <summary>
        /// ���������� ������������ (���.) ���������� ����������.
        /// </summary>
        public decimal LastDuration { get; set; }

        /// <summary>
        /// ���������� ����� ���������� ��������.
        /// </summary>
        public int CountLaunches { get; set; }

        #endregion

        #region Implementation of ISchedulingContractInfo

        /// <summary>
        /// ���������� ������������ �������.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// ���������� ������� ��������� �������.
        /// </summary>
        public TaskExecutionType State { get; private set; }

        #endregion

        #region Implementation of IScheduleAction

        /// <summary>
        /// ���������� ��� ����� �������� �������.
        /// </summary>
        public string Name
        {
            get { return _executionSchema.Name; }
        }

        /// <summary>
        /// ���������� ���������� ������� (� ��������), ����� ������� ������� ������ ���� �������� ��� ����������.
        /// </summary>
        public int DueTime
        {
            get { return _executionSchema.DueTime; }
        }

        /// <summary>
        /// ���������� �������� ������� (� ��������), ����� ������� ������� ������ ����������� ������� �������������.
        /// </summary>
        public int Period
        {
            get { return _executionSchema.Period; }
        }

        /// <summary>
        /// ���������� ��� ����� ����������� �� ���������� �������� ������� (�� ���������: -1, ��� ��������� �� ���������� ����������� ��
        /// ���������� ��������).
        /// </summary>
        public int LaunchesLimit
        {
            get { return _executionSchema.LaunchesLimit; }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// ������������ �������, ����������� ��� ��������� ������ ������������.
        /// </summary>
        public event SchedulingComleteEventHandler Completed;

        #endregion

        #region Ctors

        /// <summary>
        /// �������������� ��������� ������� � ������������ � �������� �������� ������ ������������. 
        /// </summary>
        /// <param name="schema">������������ ���������� ������������ �����.</param>
        public EndpointsLauncher(ScheduleExecutionSchema schema)
        {
            _executionSchema = schema;
            Id = schema.Id;
            _heart = new ScheduleInstance(TimeSpan.FromSeconds(schema.DueTime), TimeSpan.FromSeconds(schema.Period));
            _heart.Tripped += HeartTripped;
            _heart.Completed += (obj, args) => OnCompleted(this, args);
            State = TaskExecutionType.Idle;
        }

        #endregion

        private void HeartTripped(SchedulingEventArg arg)
        {
            try
            {
                State = TaskExecutionType.Running;
                LastRun = DateTime.Now;
                using (var factory =
                    new ChannelFactory<TChanel>(CurrentBinding(_executionSchema.BindingSchema),
                                                new EndpointAddress(_executionSchema.Uri)))
                {
                    TChanel channel = factory.CreateChannel();
                    typeof (TChanel)
                        .InvokeMember(_executionSchema.MethodName, BindingFlags.Default | BindingFlags.InvokeMethod,
                                      null, channel, null);
                }
                State = TaskExecutionType.Succeed;
            }
            catch
            {
                State = TaskExecutionType.Failure;
            }
            finally
            {
                LastDuration = _heart.LastDuration;
                CountLaunches++;
                if (_executionSchema.LaunchesLimit == CountLaunches)
                    arg.Interval = -1;
                else
                    NextRun = DateTime.Now.Add(TimeSpan.FromSeconds(_executionSchema.Period));
            }
        }

        private static Binding CurrentBinding(string schemaBinding)
        {
            Binding binding = null;
            switch (schemaBinding)
            {
                case "net.tcp":
                    binding = new NetTcpBinding                    {
                        Security = new NetTcpSecurity
                        {
                            Mode = SecurityMode.None
                        }
                        ,
                        MaxBufferPoolSize = 2147483647,
                        MaxBufferSize = 2147483647,
                        MaxReceivedMessageSize = 2147483647,
                        ReaderQuotas =
                        {
                            MaxArrayLength = 800000000,
                            MaxStringContentLength = 10000000,
                            MaxBytesPerRead = 4096000,
                            MaxDepth = 40000000
                        },
                        OpenTimeout = TimeSpan.FromSeconds(10)
                    };
                    break;
            }
            return binding;
        }

        protected void OnCompleted(object sender, EventArgs args)
        {
            if (Completed != null)
                Completed(sender, args);
        }
    }
}