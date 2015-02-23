using System;
using System.Collections.Generic;
using System.Linq;

namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// ������������ �������� ���������� ������������ ������������ ������������� ������� ������������ �������.
    /// </summary>
    public class WeeklyFrequency : SheduleFrequencyProperty
    {
        /// <summary>
        /// ���������� ������ ���� ������, � ������������ ������ ������������. 
        /// </summary>
        public virtual List<DayOfWeek> RecuringDays
        {
            get { return OccurringInfo().Where(day => day.Value).Select(x => x.Key).ToList(); }
        }

        protected bool IsOccursOnMon { get; set; }
        protected bool IsOccursOnTue { get; set; }
        protected bool IsOccursOnWed { get; set; }
        protected bool IsOccursOnThu { get; set; }
        protected bool IsOccursOnFri { get; set; }
        protected bool IsOccursOnSat { get; set; }
        protected bool IsOccursOnSun { get; set; }

        /// <summary>
        /// ������������� ��� ������, � ������� ����� ����������� �����������.
        /// </summary>
        /// <param name="daysOfWeek"></param>
        public virtual void SetDays(ISet<DayOfWeek> daysOfWeek)
        {
            foreach (DayOfWeek dayOfWeek in daysOfWeek)
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        IsOccursOnMon = true;
                        break;
                    case DayOfWeek.Tuesday:
                        IsOccursOnTue = true;
                        break;
                    case DayOfWeek.Wednesday:
                        IsOccursOnWed = true;
                        break;
                    case DayOfWeek.Thursday:
                        IsOccursOnThu = true;
                        break;
                    case DayOfWeek.Friday:
                        IsOccursOnFri = true;
                        break;
                    case DayOfWeek.Saturday:
                        IsOccursOnSat = true;
                        break;
                    case DayOfWeek.Sunday:
                        IsOccursOnSun = true;
                        break;
                }
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode()
                   ^ IsOccursOnMon.GetHashCode()
                   ^ IsOccursOnTue.GetHashCode()
                   ^ IsOccursOnWed.GetHashCode()
                   ^ IsOccursOnThu.GetHashCode()
                   ^ IsOccursOnFri.GetHashCode()
                   ^ IsOccursOnSat.GetHashCode()
                   ^ IsOccursOnSun.GetHashCode();
        }

        private Dictionary<DayOfWeek, bool> OccurringInfo()
        {
            return new Dictionary<DayOfWeek, bool>
                       {
                           {DayOfWeek.Monday, IsOccursOnMon},
                           {DayOfWeek.Tuesday, IsOccursOnTue},
                           {DayOfWeek.Wednesday, IsOccursOnWed},
                           {DayOfWeek.Thursday, IsOccursOnThu},
                           {DayOfWeek.Friday, IsOccursOnFri},
                           {DayOfWeek.Saturday, IsOccursOnSat},
                           {DayOfWeek.Sunday, IsOccursOnSun}
                       };
        }

        #region Overrides of SheduleFrequencyProperty

        public override RhythmByDate Occurs
        {
            get { return RhythmByDate.Weekly; }
        }

        private int NearestIncreementFor(DateTime current)
        {
            const int i = 7;
            if (!RecuringDays.Any())
                return i*Period;
            DateTime day;
            int increement = 1;
            while (true)
            {
                day = current.AddDays(increement);
                if (!RecuringDays.Contains(day.DayOfWeek))
                    increement++;
                else break;
            }
            return increement;
        }

        /// <summary>
        /// ��������� ��������� ����� ������� �� ��������� ����������.
        /// </summary>
        /// <param name="lastLaunch">���������� ����.</param>
        /// <returns>��������� �������� ������������� �� ����.</returns>
        public override DateTime? GetNextLaunching(DateTime? lastLaunch)
        {
            DateTime current = DateTime.Now;
            DateTime point = lastLaunch.HasValue
                                 ? lastLaunch.Value.Date
                                 : DurationFrom.AddDays(NearestIncreementFor(DurationFrom.AddDays(-1)));
            while (point < current.Date)
                point = point.AddDays(NearestIncreementFor(point));
            if (DailyFrequency.IsSingleLanch)
            {
                point = point.Add(DailyFrequency.StartingAt);
                if (lastLaunch.HasValue)
                    if (lastLaunch.Value.Date == DateTime.Now.Date)
                        point = point.AddDays(NearestIncreementFor(point));
            }
            else
            {
                TimeSpan time = lastLaunch.HasValue ? lastLaunch.Value.TimeOfDay : current.TimeOfDay;
                TimeSpan? nextTime = DailyFrequency.GetNextTimeLaunching(time);
                point = nextTime.HasValue
                            ? point.Add(nextTime.Value)
                            : point.AddDays(NearestIncreementFor(point)).Add(DailyFrequency.StartingAt);
            }
            return point > DurationTo ? default(DateTime?) : point;
        }

        #endregion
    }
}