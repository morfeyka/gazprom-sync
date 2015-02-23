using System;

namespace Sofia.Dto
{
    /// <summary>
    /// Описывает общее представление схемы состояния некоего выполняющего процесса
    /// </summary>
    public class SchedulerDto
    {
        /// <summary>
        /// Возвращает общее количество запусков.
        /// </summary>
        public int CountLaunches { get; set; }

        /// <summary>
        /// Возвращает длительность (сек.) последнего выполнения.
        /// </summary>
        public decimal LastDuration { get; set; }

        /// <summary>
        /// Возвращает дату последнего запуска на выполнение.
        /// </summary>
        public DateTime? LastRun { get; set; }

        /// <summary>
        /// Возвращает дату последующего запуска на выполнение.
        /// </summary>
        public DateTime NextRun { get; set; }

        /// <summary>
        /// Возвращает системный идентификатор планировщика.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Возвращает текущее состояние задания.
        /// </summary>
        public TaskExecutionType State { get; set; }

        /// <summary>
        /// Наименование расписания
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Тип выполняемой задачи
        /// </summary>
        public string Type { get; set; }
    }
}