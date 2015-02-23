using System;
using System.Linq;

namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// Представляет структуру, описывающую конфигурацию периодичности по времени.
    /// </summary>
    [Serializable]
    public class TimeSpanFrequency : IPeriodicityByTime, IOneTimeOccurrence
    {
        private int _period;

        #region Ctors
        public TimeSpanFrequency()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр с указанием периодичности запуска в указанное значение перечислителя времени 
        /// с соответствующим временным диапазоном запуска.
        /// </summary>
        /// <param name="period">Период.</param>
        /// <param name="occursType">Значение перечислителя времени.</param>
        /// <param name="startAt">Начальное значение диапазона запуска.</param>
        /// <param name="stopAt">Конечное значение диапазона запуска.</param>
        public TimeSpanFrequency(int period, RhythmByTime occursType, TimeSpan startAt, TimeSpan stopAt) : this()
        {
            _period = period;
            OccursEvery = occursType;
            StartingAt = startAt;
            EndingAt = stopAt;
        }

        /// <summary>
        /// Инициализирует новый экземпляр с указанием периодичности запуска в указанное значение перечислителя времени и открытым диапазоном запуска.
        /// </summary>
        /// <param name="period">Период.</param>
        /// <param name="occursType">Значение перечислителя времени.</param>
        /// <param name="startAt">Начальное время для запуска.</param>
        public TimeSpanFrequency(int period, RhythmByTime occursType, TimeSpan startAt)
            : this(period, occursType, startAt, TimeSpan.MaxValue)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр с заданной периодичностью выполнения и типом времени, неограниченную по времени запуска.
        /// </summary>
        /// <param name="period">Период.</param>
        /// <param name="occursType">Значение перечислителя времени.</param>
        public TimeSpanFrequency(int period, RhythmByTime occursType) : this(period, occursType, TimeSpan.MinValue)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр с заданной периодичностью выполнения.
        /// </summary>
        /// <param name="period">Периодичность выполнения, указанная в часах.</param>
        public TimeSpanFrequency(int period) : this(period, RhythmByTime.Hours)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр единичного запуска с указанание времени запуска, неограниченную по времени запуска.
        /// </summary>
        /// <param name="startAt">Время запуска.</param>
        public TimeSpanFrequency(TimeSpan startAt) : this(0, RhythmByTime.Hours, startAt, startAt)
        {
        }

        #endregion

        #region Properties

        #region Implementation of IFrequencyOccurrence

        /// <summary>
        /// Возвращает или задаёт значение периода.
        /// </summary>
        public int Period
        {
            get { return _period; }
            set
            {
                if (_period == 0)
                    SetOneTimeOccurrence();
                else
                {
                    switch (OccursEvery)
                    {
                        case RhythmByTime.Hours:
                            if (!Enumerable.Range(1, 23).Contains(value))
                                throw new ArgumentOutOfRangeException(
                                    "Period property value for current occurs  must between 1 " + "and 24");
                            break;
                        case RhythmByTime.Minutes:
                            if (!Enumerable.Range(1, 59).Contains(value))
                                throw new ArgumentException(
                                    "Period property value for current occurs must between 1 and 60");
                            break;
                        case RhythmByTime.Seconds:
                            if (!Enumerable.Range(10, 49).Contains(value))
                                throw new ArgumentException(
                                    "Period property value for current occurs must between 10 and 60");
                            break;
                    }
                }
                _period = value;
            }
        }

        #endregion

        #region Implementation of IPeriodicityByTime

        /// <summary>
        /// Возвращает или задаёт текущее значение перечислителя промежутков времени.
        /// </summary>
        public RhythmByTime OccursEvery { get; set; }

        /// <summary>
        /// Возвращает или задаёт начальное значение временного диапазона.
        /// </summary>
        public TimeSpan StartingAt { get; set; }

        /// <summary>
        /// Возвращает или задаёт конечное значение временного диапазона.
        /// </summary>
        public TimeSpan EndingAt { get; set; }

        #endregion

        #region Implementation of IOneTimeOccurrence

        /// <summary>
        /// Возвращает признак разового выполнения задания.
        /// </summary>
        public bool IsSingleLanch
        {
            get { return StartingAt.Equals(EndingAt); }
        }

        #endregion

        #endregion

        private void SetOneTimeOccurrence()
        {
            EndingAt = StartingAt;
            OccursEvery = RhythmByTime.Hours;
        }

        /// <summary>
        /// Инициализирует экземпляр единичного запуска со значением по умолчанию для времени запуска, равным 2 часа.
        /// </summary>
        /// <returns>Экземпляр единичного запуска.</returns>
        public static TimeSpanFrequency StartOnce()
        {
            return StartOnce(new TimeSpan(2, 0, 0));
        }

        /// <summary>
        /// Инициализирует экземпляр единичного запуска с указанием времени запуска.
        /// </summary>
        /// <param name="occursAt">Значение времени.</param>
        /// <returns>Экземпляр единичного запуска.</returns>
        public static TimeSpanFrequency StartOnce(TimeSpan occursAt)
        {
            return new TimeSpanFrequency(occursAt);
        }

        /// <summary>
        /// Возвращает хэш-код данного экземпляра.
        /// </summary>
        /// <returns>
        /// 32-разрядное целое число со знаком, являющееся хэш-кодом для данного экземпляра.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return StartingAt.GetHashCode()
                   ^ EndingAt.GetHashCode()
                   ^ Period.GetHashCode()
                   ^ OccursEvery.GetHashCode()
                   ^ IsSingleLanch.GetHashCode();
        }

        /// <summary>
        /// Возвращает следующую точку периода временного интервала для указанного значения времени.
        /// </summary>
        /// <param name="current">Значение времени.</param>
        /// <returns>Значение времени или null, если достигнут предел временного диапазона.</returns>
        public TimeSpan? GetNextTimeLaunching(TimeSpan current)
        {
            if (IsSingleLanch) 
                return default(TimeSpan?);
            var time = DateTime.Now.TimeOfDay;
            var next = (current > time && AppendPeriod(time) > current) ? current : StartingAt;
            while (next < time)
                next = AppendPeriod(next);
            if (next > EndingAt) return default(TimeSpan?);

            return (next <= StartingAt) ? StartingAt : next;
        }

        private TimeSpan AppendPeriod(TimeSpan current)
        {
            TimeSpan next = current;
            switch (OccursEvery)
            {
                case RhythmByTime.Hours:
                    next = next.Add(new TimeSpan(Period, 0, 0));
                    break;
                case RhythmByTime.Minutes:
                    next = next.Add(new TimeSpan(0, Period, 0));
                    break;
                case RhythmByTime.Seconds:
                    next = next.Add(new TimeSpan(0, 0, Period));
                    break;
            }
            return next;
        }
    }
}