using System.Collections.Generic;
using System.ServiceModel;
using Sofia.Contracts.Data;

namespace Sofia.Contracts
{
    /// <summary>
    /// Представляет описание контракта службы, выполняющей автоматический, в соответствии
    /// с заданными параметрами, вызов оконечных точек зависимых служб.
    /// </summary>
    [ServiceContract]
    public interface ISensetiveCommunity
    {
        /// <summary>
        /// Выполняет добавление указанной конфигурации вызова в список автоматически вызываемых заданий. 
        /// </summary>
        /// <param name="schema">Конфигурация параметров для планировщика служб.</param>
        [OperationContract]
        void ApplySchedule(ScheduleExecutionSchema schema);

        /// <summary>
        /// Выполняет удаление указанной конфигурации вызова в список автоматически вызываемых заданий.
        /// </summary>
        /// <param name="schema">Конфигурация параметров для планировщика служб.</param>
        [OperationContract]
        void DetachSchedule(ScheduleExecutionSchema schema);

        /// <summary>
        /// Возвращает сведения о зарегестрированных заданиях в планировщике служб.
        /// </summary>
        /// <returns>Список из объектов, хранящих сведения о выполнении заданий.</returns>
        [OperationContract]
        List<ISchedulingContractInfo> GetMonitoringList();
    }
}