using System;

namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// Представляет описание параметров конфигурации ежедневной периодичности запуска планировщика заданий.
    /// </summary>
    public class DailyFrequency : SheduleFrequencyProperty
    {
        #region Overrides of SheduleFrequencyProperty

        public override RhythmByDate Occurs
        {
            get { return RhythmByDate.Daily; }
        }

        /// <summary>
        /// Вычисляет следующую точку периода на основании предыдущей.
        /// </summary>
        /// <param name="lastLaunch">Предыдущая дата.</param>
        /// <returns>Следующее значение периодичности по дате.</returns>
        public override DateTime? GetNextLaunching(DateTime? lastLaunch)
        {
            DateTime current = DateTime.Now;
            DateTime point = lastLaunch.HasValue ? lastLaunch.Value.Date : DurationFrom;
            while (point < current.Date)
                point = point.AddDays(Period);
            if (DailyFrequency.IsSingleLanch)
            {
                point = point.Add(DailyFrequency.StartingAt);
                if (lastLaunch.HasValue)
                    if (lastLaunch.Value.Date == DateTime.Now.Date)
                        point = point.AddDays(Period);
            }
            else
            {
                TimeSpan time = lastLaunch.HasValue ? lastLaunch.Value.TimeOfDay : current.TimeOfDay;
                TimeSpan? nextTime = DailyFrequency.GetNextTimeLaunching(time);
                point = nextTime.HasValue
                            ? point.Add(nextTime.Value)
                            : point.AddDays(Period).Add(DailyFrequency.StartingAt);
            }
            return point > DurationTo ? default(DateTime?) : point;
        }

        #endregion
    }
}