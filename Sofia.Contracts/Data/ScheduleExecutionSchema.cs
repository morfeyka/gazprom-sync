using System;
using System.Runtime.Serialization;
using Sofia.Contracts.Description;

namespace Sofia.Contracts.Data
{
    /// <summary>
    /// Представляет описание контракта о данных для настройки планировщика служб.
    /// </summary>
    [DataContract]
    public class ScheduleExecutionSchema : IScheduleAction
    {
        /// <summary>
        /// Инициализирует экзепляр параметров конфигурации планировщика служб.
        /// </summary>
        /// <param name="scheduleInfo">Сведения о расписании выполняемого задания.</param>
        public ScheduleExecutionSchema(IScheduleAction scheduleInfo)
        {
            Name = scheduleInfo.Name;
            DueTime = scheduleInfo.DueTime;
            Period = scheduleInfo.Period;
            LaunchesLimit = scheduleInfo.LaunchesLimit;
            Id = new Guid();
        }

        #region Properties

        #region Implementation of IScheduleAction

        /// <summary>
        /// Возвращает или задаёт название задания.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Возвращает промежуток времени (в секундах), через который задание должно быть запущено для выполнения.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int DueTime { get; set; }

        /// <summary>
        /// Возвращает интервал времени (в секундах), через который задание должно запускаться внешним планировщиком.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Period { get; set; }

        /// <summary>
        /// Возвращает или задаёт ограничение по количеству запусков задания (по умолчанию: -1, что указывает на отстутсвие ограничений по
        /// количеству запусков).
        /// </summary>
        [DataMember(IsRequired = true)]
        public int LaunchesLimit { get; set; }

        #endregion

        /// <summary>
        /// Возвращает или задаёт имя сборки контракта вызываемой службы.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string AssemblyName { get; set; }

        /// <summary>
        /// Возвращает или задаёт полное имя контракта (с указанием пространства имён) вызываемой службы.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Contract { get; set; }

        /// <summary>
        /// Возвращает или задаёт имя метода, вызываемый планировщиком.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string MethodName { get; set; }

        /// <summary>
        /// Возвращает или задаёт схему привязки данных вызываемой оконечной точки службы.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string BindingSchema { get; set; }

        /// <summary>
        /// Возвращает или задаёт адрес в формате URI вызываемой оконечной точки службы.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Uri { get; set; }

        /// <summary>
        /// Представляет параметр для идентификации.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid Id { get; private set; }

        #endregion

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}