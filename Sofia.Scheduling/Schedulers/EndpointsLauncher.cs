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
    /// Представляет описание задания для автоматического вызова удалённой оконечной точки сервиса.
    /// </summary>
    /// <typeparam name="TChanel">Контракт, реализуемый оконечной точкой.</typeparam>
    public class EndpointsLauncher<TChanel> : ISchedulingContractInfo
    {
        private readonly ScheduleExecutionSchema _executionSchema;
        private readonly ScheduleInstance _heart;

        #region Properties

        #region Implementation of IExecutionState

        /// <summary>
        /// Возвращает дату последнего запуска на выполнение.
        /// </summary>
        public DateTime? LastRun { get; set; }

        /// <summary>
        /// Возвращает дату последующего запуска на выполнение.
        /// </summary>
        public DateTime NextRun { get; set; }

        /// <summary>
        /// Возвращает длительность (сек.) последнего выполнения.
        /// </summary>
        public decimal LastDuration { get; set; }

        /// <summary>
        /// Возвращает общее количество запусков.
        /// </summary>
        public int CountLaunches { get; set; }

        #endregion

        #region Implementation of ISchedulingContractInfo

        /// <summary>
        /// Возвращает идентификтор задания.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Возвращает текущее состояние задания.
        /// </summary>
        public TaskExecutionType State { get; private set; }

        #endregion

        #region Implementation of IScheduleAction

        /// <summary>
        /// Возвращает или задаёт название задания.
        /// </summary>
        public string Name
        {
            get { return _executionSchema.Name; }
        }

        /// <summary>
        /// Возвращает промежуток времени (в секундах), через который задание должно быть запущено для выполнения.
        /// </summary>
        public int DueTime
        {
            get { return _executionSchema.DueTime; }
        }

        /// <summary>
        /// Возвращает интервал времени (в секундах), через который задание должно запускаться внешним планировщиком.
        /// </summary>
        public int Period
        {
            get { return _executionSchema.Period; }
        }

        /// <summary>
        /// Возвращает или задаёт ограничение по количеству запусков задания (по умолчанию: -1, что указывает на отстутсвие ограничений по
        /// количеству запусков).
        /// </summary>
        public int LaunchesLimit
        {
            get { return _executionSchema.LaunchesLimit; }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Представляет событие, возникающее при окончании работы планировщика.
        /// </summary>
        public event SchedulingComleteEventHandler Completed;

        #endregion

        #region Ctors

        /// <summary>
        /// Инициализирует экземпляр задания в соответствии с заданным правилом работы планировщика. 
        /// </summary>
        /// <param name="schema">Конфигурация параметров планировщика служб.</param>
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