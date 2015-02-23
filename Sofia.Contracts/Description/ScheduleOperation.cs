using System;

namespace Sofia.Contracts.Description
{
    /// <summary> 
    /// ѕредставл€ет настраиваемый набор параметров, раскрывающих характер вызова метода оконечной точки службы планировщиком.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ScheduleOperation : Attribute, IScheduleAction
    {
        #region Ctors

        /// <summary>
        /// »нициализирует новую конфигуцию параметров, описывающих характер вызова метода контракта службы
        /// на выполнение планировщиком через один час с интервалом в одну минуту
        /// </summary>
        public ScheduleOperation()
        {
            Period = (int) TimeSpan.FromHours(1).TotalSeconds;
            DueTime = (int) TimeSpan.FromMinutes(1).TotalSeconds;
        }

        #endregion

        #region Implementation of IScheduleAction

        /// <summary>
        /// ¬озвращает или задаЄт название задани€.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ¬озвращает промежуток времени (в секундах), через который задание должно быть запущено на выполнение.
        /// </summary>
        public int DueTime { get; set; }

        /// <summary>
        /// ¬озвращает интервал времени (в секундах), через который задание должно запускатьс€ внешним планировщиком.
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// ¬озвращает или задаЄт ограничение по количеству запусков задани€ (по умолчанию: -1, что указывает на отстутсвие ограничений по
        /// количеству запусков).
        /// </summary>
        public int LaunchesLimit { get; set; }

        #endregion
    }
}