using System;

namespace Sofia.Scheduling.Core
{
    /// <summary>
    /// Представляет описание данных о событии срабатывания планировщика.
    /// </summary>
    public class SchedulingEventArg : EventArgs
    {
        /// <summary>
        /// Возвращает или задаёт следующее значение периода (в миллисекундах) запуска планировщика.
        /// </summary>
        public long Interval { get; set; }
    }
}