using System;
using System.Collections.Generic;

namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// Представляет описание параметров конфигурации ежемесячной периодичности запуска планировщика.
    /// </summary>
    public class MonthlyFrequency : SheduleFrequencyProperty
    {
        private int? _dayOffset;

        #region Ctors

        /// <summary>
        /// Инициализирует экземпляр конфигурации запуска с уровнем детализации в днях. 
        /// </summary>
        public MonthlyFrequency()
            : this(true)
        {
        }

        /// <summary>
        /// Инициализирует экземпляр конфигурации с заданным уровнем детализации.
        /// </summary>
        /// <param name="isOccursByDay">True - детализация по количеству дней, False - недельная детализация.</param>
        public MonthlyFrequency(bool isOccursByDay)
        {
            if (isOccursByDay)
                _dayOffset = 1;
            else
                DayOfWeek = new KeyValuePair<RhythmByWeek, DayOfWeek>(RhythmByWeek.First, System.DayOfWeek.Monday);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Возвращает признак указания детализации по количеству дней.
        /// </summary>
        public virtual bool IsDefinedByDay
        {
            get { return _dayOffset.HasValue; }
        }

        /// <summary>
        /// Возвращает или задаёт параметр: порядковый номер дня в месяце.
        /// </summary>
        public virtual int? DayOffset
        {
            get { return _dayOffset; }
            set
            {
                if (value.HasValue && DayOfWeek.HasValue)
                    DayOfWeek = null;
                _dayOffset = value;
            }
        }

        /// <summary>
        /// Возвращает или задаёт пару ключ-значение, где ключом выступает номер недели в месяце, значение - день недели. 
        /// </summary>
        public virtual KeyValuePair<RhythmByWeek, DayOfWeek>? DayOfWeek
        {
            get
            {
                return (WeekNumber.HasValue && Week.HasValue)
                           ? new KeyValuePair<RhythmByWeek, DayOfWeek>(WeekNumber.Value, Week.Value)
                           : (KeyValuePair<RhythmByWeek, DayOfWeek>?) null;
            }
            set
            {
                _dayOffset = value.HasValue ? null : _dayOffset;
                WeekNumber = value.HasValue ? value.Value.Key : (RhythmByWeek?) null;
                Week = value.HasValue ? value.Value.Value : (DayOfWeek?) null;
            }
        }

        public virtual RhythmByWeek? WeekNumber { get; set; }

        public virtual DayOfWeek? Week { get; set; }

        #region Overrides of SheduleFrequencyProperty

        public override RhythmByDate Occurs
        {
            get { return RhythmByDate.Monthly; }
        }

        /// <summary>
        /// Вычисляет следующую точку периода на основании предыдущей.
        /// </summary>
        /// <param name="lastLaunch">Предыдущая дата.</param>
        /// <returns>Следующее значение периодичности по дате.</returns>
        public override DateTime? GetNextLaunching(DateTime? lastLaunch)
        {
            DateTime current = DateTime.Now;
            DateTime point = lastLaunch.HasValue ? lastLaunch.Value.Date : GetNextByCondition(DurationFrom);
            while (point < current.Date)
                point = GetNextByCondition(point.AddMonths(Period));
            if (DailyFrequency.IsSingleLanch)
            {
                point = point.Add(DailyFrequency.StartingAt);
                if (lastLaunch.HasValue)
                    if (lastLaunch.Value.Date == current.Date)
                        point = GetNextByCondition(point.AddMonths(Period));
            }
            else
            {
                TimeSpan time = lastLaunch.HasValue ? lastLaunch.Value.TimeOfDay : current.TimeOfDay;
                TimeSpan? nextTime = DailyFrequency.GetNextTimeLaunching(time);
                point = nextTime.HasValue
                            ? point.Add(nextTime.Value)
                            : GetNextByCondition(point.AddMonths(Period)).Add(DailyFrequency.StartingAt);
            }
            return point;
        }

        private DateTime GetNextByCondition(DateTime current)
        {
            if (IsDefinedByDay && DayOffset.HasValue)
                return new DateTime(current.Year, current.Month, DayOffset.Value);
            var next = new DateTime(current.Year, current.Month, 1);
            if (WeekNumber.HasValue && Week.HasValue)
            {
                switch (WeekNumber.Value)
                {
                    case RhythmByWeek.Last:
                        next = new DateTime(next.Year, next.Month, DateTime.DaysInMonth(next.Year, next.Month));
                        while (next.DayOfWeek != Week.Value)
                            next = next.AddDays(-1);
                        break;
                    default:
                        while (next.DayOfWeek != Week.Value)
                            next = next.AddDays(1);
                        next = next.AddDays(7*(int) WeekNumber.Value).Month > next.Month
                                   ? GetNextByCondition(new DateTime(next.AddMonths(Period).Year,
                                                                     next.AddMonths(Period).Month, 1))
                                   : next.AddDays(7*(int) WeekNumber.Value);
                        break;
                }
            }
            return next;
        }

        #endregion

        #endregion

        public override int GetHashCode()
        {
            int baseHashCode = base.GetHashCode();
            return DayOffset.HasValue
                       ? baseHashCode ^ DayOffset.Value.GetHashCode()
                       : (DayOfWeek.HasValue ? baseHashCode ^ DayOfWeek.Value.GetHashCode() : baseHashCode);
        }
    }
}