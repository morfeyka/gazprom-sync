using System;

namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// Представляет базовое описание конструкции периодичности.
    /// </summary>
    public abstract class SheduleFrequencyProperty : Entity, IPeriodicityByDate
    {
        private TimeSpanFrequency _dailyFrequency;
        private DateTime _durationFrom;
        private DateTime _durationTo;
        private int _period;

        protected SheduleFrequencyProperty()
        {
            _durationFrom = DateTime.Now.Date;
            _durationTo = DateTime.MaxValue.Date;
            _period = 1;
            _dailyFrequency = TimeSpanFrequency.StartOnce();
        }

        /// <summary>
        /// Возвращает или задаёт конфигурацию периодичности по времени.
        /// </summary>
        public virtual TimeSpanFrequency DailyFrequency
        {
            get { return _dailyFrequency; }
            set { _dailyFrequency = value; }
        }

        /// <summary>
        /// Возвращает признак бесконечной деятельности планировщика.
        /// </summary>
        public virtual bool IsInfinite
        {
            get { return _durationTo == DateTime.MaxValue.Date; }
        }

        #region IPeriodicityByDate Members

        /// <summary>
        /// Возвращает или задаёт начальную дату планировщика.
        /// </summary>
        public virtual DateTime DurationFrom
        {
            get { return _durationFrom; }
            set { _durationFrom = value; }
        }

        /// <summary>
        /// Возвращает или задаёт конечную дату планировщика (по умолчанию null - открытая дата).
        /// </summary>
        public virtual DateTime DurationTo
        {
            get{ return _durationTo;}
            set{ _durationTo = value;}
        }

        /// <summary>
        /// Вычисляет следующую точку периода на основании предыдущей.
        /// </summary>
        /// <param name="lastLaunch">Предыдущая дата.</param>
        /// <returns>Следующее значение периодичности по дате.</returns>
        public abstract DateTime? GetNextLaunching(DateTime? lastLaunch);

        /// <summary>
        /// Возвращает или задаёт значение периода для текущей конфигурации периодичности.
        /// </summary>
        public virtual int Period
        {
            get { return _period; }
            set { _period = value; }
        }

        /// <summary>
        /// Возвращает перечеслитель периодичности по дате.
        /// </summary>
        public abstract RhythmByDate Occurs { get; }

        #endregion


        protected void SetOneTimeOccurrence(DateTime occursAt)
        {
            _period = 0;
            _durationTo = _durationFrom = occursAt.Date;
            _dailyFrequency = new TimeSpanFrequency(occursAt.TimeOfDay);
        }
    }
}