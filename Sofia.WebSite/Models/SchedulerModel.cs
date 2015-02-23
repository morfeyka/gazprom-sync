using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Sofia.Domain.Setting.Appointment.Periodicity;

namespace Sofia.WebSite.Models
{
    public class SchedulerModel
    {

        public int Id { get; set; }
        /// <summary>
        /// Тип расписания
        /// </summary>
        public string Type { get; set; }
        public int FrequencyType
        {
            get; set; }

        //public SingleLanchFrequencyProperty SingleLanchFrequency { get; set; }

        public DateTime cDate { get; set; }

        public int ddlDateRhythm { get; set; }

        public SelectList DateRhythmList
        {
            get
            {
                return new SelectList(
                    (new[]
                         {
                             new {Id = 0, Name = "Ежедневно"}, new {Id = 1, Name = "Еженедельно"},
                             new {Id = 2, Name = "Ежемесячно"}
                         }), "Id", "Name", ddlDateRhythm);
            }
        }

        public int DailyPeriod { get; set; }
        public int WeeklyPeriod { get; set; }

        public bool Monday { get; set; }

        public bool Wednesday { get; set; }

        public bool Friday { get; set; }

        public bool Saturday { get; set; }

        public bool Tuesday { get; set; }

        public bool Thursday { get; set; }

        public bool Sunday { get; set; }

        public bool RblMonthDetail { get; set; }

        public int? DayOffset { get; set; }

        public int MonthlyPeriod { get; set; }

        public int ddlWeekPart { get; set; }
        public int ddlWeeks { get; set; }
        public int OccursEvery { get; set; }
        private SelectList _WeekPartList;
        private SelectList _listDayOfWeek;
        private SelectList _RhythmByTime;
        public DateTime StartingAt { get; set; }
        public string Description { get; set; }
    
        public SelectList RhythmByTimeList
        {
            get { return _RhythmByTime; }
        }
        public SelectList WeekPartList
        {
            get { return _WeekPartList; }
        }

        public SelectList DayOfWeekList { get { return _listDayOfWeek; }
        }

        public int tbxMonthNumFor { get; set; }

        public bool IsSingleLanch { get; set; }

        public DateTime cTimeConst { get; set; }

        public int SingleLanchPeriod { get; set; }

        public DateTime EndingAt { get; set; }

        public DateTime DurationFrom { get; set; }

        public bool IsInfinite { get; set; }

        public DateTime DurationTo { get; set; }

        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public string CreatedOn { get; set; }

        public string LastRun { get; set; }

        public string NextRun { get; set; }

        public string Duration { get; set; }

        public string Iteration { get; set; }

        public int? Runtime { get; set; }

        public bool IsKill { get; set; }

        public string Param { get; set; }
        /// <summary>
        /// Возвращает или задает дату-время формирования данных
        /// </summary>
        public DateTime? TimeGetData { get; set; }

        public SchedulerModel()
        {
            Calendar calendar = CultureInfo.CurrentCulture.Calendar;
            CultureInfo german = new CultureInfo("ru-RU");
            DateTime date = DateTime.Now;
            cTimeConst = date;
            cDate = date;
            cTimeConst = cTimeConst.AddHours(2);
            StartingAt = date;
            StartingAt = StartingAt.AddHours(1);
            EndingAt = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
            DurationTo= new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            DurationFrom = new DateTime(date.Year, 1, 1);
            //Description.Text = _sheduling.DescriptionFor(ApplySheduling());

            DailyPeriod = 1;
            SingleLanchPeriod = 1;
            WeeklyPeriod = 1;
            DayOffset = 1;
            MonthlyPeriod = 1;
            tbxMonthNumFor = 1;
            var list = new List<SelectListItem>();
            foreach (RhythmByWeek part in Enum.GetValues(typeof(RhythmByWeek)))
            {
                var x = new SelectListItem {Text =GetNameRhythmByWeek(part), Value = ((int) part).ToString()};
                list.Add(x);
            }
            _WeekPartList = new SelectList(list, "Value", "Text");
            list = new List<SelectListItem>();
            foreach (DayOfWeek week in Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Where(x => x != DayOfWeek.Sunday))
            {
                var x = new SelectListItem { Text = german.DateTimeFormat.DayNames[(int)week], Value = ((int)week).ToString() };
                list.Add(x);
            }
            var y = new SelectListItem { Text = german.DateTimeFormat.DayNames[(int)DayOfWeek.Sunday], Value = ((int)DayOfWeek.Sunday).ToString() };
            list.Add(y);
            _listDayOfWeek = new SelectList(list, "Value", "Text");

            list = new List<SelectListItem>();
            foreach (RhythmByTime part in Enum.GetValues(typeof(RhythmByTime)))
            {
                var x = new SelectListItem { Text = GetName(part), Value = ((int)part).ToString() };
                list.Add(x);
            }
            _RhythmByTime = new SelectList(list, "Value", "Text");
        }

        private string GetNameRhythmByWeek(RhythmByWeek id)
        {
            switch (id)
            {
                    case RhythmByWeek.First:
                    return "первый";
                    case RhythmByWeek.Second:
                    return "второй";
                    case RhythmByWeek.Third:
                    return "третий";
                    case RhythmByWeek.Fourth:
                    return "четвертый";
                    case RhythmByWeek.Last:
                    return "последний";
                default:
                    return "первый";

            }
        }

        private string GetName(RhythmByTime id)
        {
            switch (id)
            {
                case RhythmByTime.Seconds:
                    return "секунд";
                case RhythmByTime.Minutes:
                    return "мин";
                case RhythmByTime.Hours:
                    return "ч";
                default:
                    return "ч";

            }
        }
    }
}
