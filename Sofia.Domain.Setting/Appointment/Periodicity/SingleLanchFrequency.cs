using System;

namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// Представляет описание параметров конфигурации разового запуска планировщика заданий.
    /// </summary>
    public class SingleLanchFrequency : SheduleFrequencyProperty, IOneTimeOccurrence
    {
        #region Ctors

        /// <summary>
        /// Инициализирует экземпляр конфигурации отложенного разового запуска в 2 часа.
        /// </summary>
        public SingleLanchFrequency()
            : this(DateTime.Now.AddHours(2))
        {
        }

        /// <summary>
        /// Инициализирует экземпляр конфигурации разового запуска в указанное время.
        /// </summary>
        /// <param name="runAt">Время запуска.</param>
        public SingleLanchFrequency(DateTime runAt)
        {
            SetOneTimeOccurrence(runAt);
        }

        #endregion

        #region Overrides of SheduleFrequencyProperty

        /// <summary>
        /// Возвращает перечеслитель периодичности по дате.
        /// </summary>
        public override RhythmByDate Occurs
        {
            get { return RhythmByDate.OneTime; }
        }

        /// <summary>
        /// Вычисляет следующую точку периода на основании предыдущей.
        /// </summary>
        /// <param name="lastLaunch">Предыдущая дата.</param>
        /// <returns>Следующее значение периодичности по дате.</returns>
        public override DateTime? GetNextLaunching(DateTime? lastLaunch)
        {
            if (lastLaunch.HasValue) return default(DateTime?);
            var next = DurationFrom.Add(DailyFrequency.StartingAt);
            return next <= DateTime.Now ? default(DateTime?) : next;
        }

        #endregion

        #region Implementation of IOneTimeOccurrence

        /// <summary>
        /// Возвращает признак разового выполнения задания.
        /// </summary>
        public virtual bool IsSingleLanch
        {
            get { return true; }
        }

        #endregion
    }
}