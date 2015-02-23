using System;

namespace Sofia.Contracts.Data
{
    /// <summary>
    /// Описывает общее представление схемы состояния некоего выполняющего процесса
    /// </summary>
    public interface IExecutionState
    {
        /// <summary>
        /// Возвращает дату последнего запуска на выполнение.
        /// </summary>
        DateTime? LastRun { get; }
        /// <summary>
        /// Возвращает дату последующего запуска на выполнение.
        /// </summary>
        DateTime NextRun { get; }
        /// <summary>
        /// Возвращает длительность (сек.) последнего выполнения.
        /// </summary>
        decimal LastDuration { get; }
        /// <summary>
        /// Возвращает общее количество запусков.
        /// </summary>
        int CountLaunches { get; }
    }
}