using System;
using Sofia.Contracts.Description;
using Sofia.Dto;

namespace Sofia.Contracts.Data
{
    /// <summary>
    /// Представляет набор правил, описывающих процесс состояния выполнения задания,
    /// представляющего собой вызов метода оконечной точки сервиса. 
    /// </summary>
    public interface ISchedulingContractInfo : IExecutionState, IScheduleAction
    {
        /// <summary>
        /// Возвращает идентификтор задания.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Возвращает текущее состояние задания.
        /// </summary>
        TaskExecutionType State { get; }
    }
}