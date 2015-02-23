using System;

namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// ������������ �������� ���������� ������������ ���������� ������������� ������� ������������ �������.
    /// </summary>
    public class DailyFrequency : SheduleFrequencyProperty
    {
        #region Overrides of SheduleFrequencyProperty

        public override RhythmByDate Occurs
        {
            get { return RhythmByDate.Daily; }
        }

        /// <summary>
        /// ��������� ��������� ����� ������� �� ��������� ����������.
        /// </summary>
        /// <param name="lastLaunch">���������� ����.</param>
        /// <returns>��������� �������� ������������� �� ����.</returns>
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