using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Sofia.Domain.Setting.Appointment.Periodicity;

//using hahaGroup.Core.Domain.Localization;
//using hahaGroup.Data.Logic.Localization;

namespace Sofia.Data
{
    /// <summary>
    /// /
    /// </summary>
    public class Sheduling
    {
        private const char DescriptionValuesDelimeter = '|';
        private const string DescriptionFormattingString = @"{0}|{1}";
        //private readonly ResourceDao _resourceStore;
        private readonly string[] _delimeters;

        /// <summary>
        /// 
        /// </summary>
        public Sheduling()
        {
          //  _resourceStore = new ResourceDao();
            _delimeters = new[] { ",", "." };
        }

        /// <summary>
        /// ¬озвращает локализованное представление описани€ дл€ указанного расписани€ <see cref="SheduleFrequencyProperty"/>.
        /// </summary>
        /// <param name="property">–асписание.</param>
        /// <returns>Ћокализованна€ строка расписани€ <see cref="SheduleFrequencyProperty"/>.</returns>
        public string DescriptionFor(SheduleFrequencyProperty property)
        {
            var descr = new StringBuilder();
            IList<string> dict = GetSuitableDictionaryFor(property);
            for (int i = 0; i < dict.Count; i++)
            {
                string key;
                string value = string.Empty;
                if (dict[i].Contains(DescriptionValuesDelimeter))
                {
                    string[] words = dict[i].Split(DescriptionValuesDelimeter);
                    key = words.First();
                    value = words.Last();
                }
                else
                    key = dict[i];
                if (!string.IsNullOrEmpty(key))
                {
                   // key = _resourceStore.GetValueForKey(ResourceType.PageContent, key);
                    descr.AppendFormat(i == 0 ? "{0}" : " {0}", key);
                }
                if (string.IsNullOrEmpty(value)) continue;
                descr.AppendFormat(value.Length > 1 ? " {0}" : (_delimeters.Contains(value) ? "{0}" : " {0}"), value);
            }
            return descr.ToString();
        }

        #region Descriptions property building methods

        private IList<string> GetSuitableDictionaryFor(SheduleFrequencyProperty property)
        {
            if (property.Occurs == RhythmByDate.OneTime)
                return AddUnitaryAddIns((SingleLanchFrequency) property);
            var dict = new List<string> { "¬ыполн€етс€" };
            switch (property.Occurs)
            {
                case RhythmByDate.Daily:
                    dict.AddRange(AddDailyAddIns((DailyFrequency) property));
                    break;
                case RhythmByDate.Weekly:
                    dict.AddRange(AddWeeklyAddIns((WeeklyFrequency) property));
                    break;
                case RhythmByDate.Monthly:
                    dict.AddRange(AddMonthlyAddIns((MonthlyFrequency) property));
                    break;
            }
            dict.AddRange(AddTimeAddIns(property.DailyFrequency));
            dict.Add("–асписание будет использовано");//ScheduleWillBeUsed
            dict.Add(string.Format(DescriptionFormattingString, (property.IsInfinite ? @"с" : @"между"),//starting on, between
                                   property.DurationFrom.ToShortDateString()));
            if (!property.IsInfinite)
                dict.Add(string.Format(DescriptionFormattingString, @"и", property.DurationTo.ToShortDateString()));//and2
            dict.Add(string.Format(DescriptionFormattingString, string.Empty, "."));
            return dict;
        }

        private static IEnumerable<string> AddTimeAddIns(TimeSpanFrequency frequency)
        {
            var addIns = new List<string>();
            if (frequency.IsSingleLanch)
                addIns.Add(string.Format(DescriptionFormattingString, "в", frequency.StartingAt.ToString("T")));
            else
            {
                addIns.Add(frequency.Period == 1
                               ? (frequency.OccursEvery == RhythmByTime.Hours ? @"каждый" : @"каждую") //overEvery overEvery_w
                               : string.Format(DescriptionFormattingString, @"каждые", frequency.Period)); //overEvery(s)
                addIns.Add(DeclinableForTimes(frequency.OccursEvery, frequency.Period));
                addIns.Add(string.Format(DescriptionFormattingString, @"между", frequency.StartingAt.ToString("T")));//betweenIn
                addIns.Add(string.Format(DescriptionFormattingString, @"и", frequency.EndingAt.ToString("T")));
            }
            addIns.Add(string.Format(DescriptionFormattingString, string.Empty, "."));
            return addIns;
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
        private IEnumerable<string> AddMonthlyAddIns(MonthlyFrequency monthlyFrequency)
        {
            var addIns = new List<string>();
            if (monthlyFrequency.IsDefinedByDay)
            {
                addIns.Add(monthlyFrequency.Period == 1
                               ? @"каждый"
                               : string.Format(DescriptionFormattingString, @"каждые", monthlyFrequency.Period));
                addIns.Add(DeclinableForDates(monthlyFrequency.Occurs, monthlyFrequency.Period));
                if (monthlyFrequency.DayOffset.HasValue)
                    addIns.Add(string.Format(DescriptionFormattingString, "на",
                                             StringNumericEndOfWord(monthlyFrequency.DayOffset.Value, 'm')));
                addIns.Add(DeclinableForDates(RhythmByDate.Daily, 1));
            }
            else
            {
                const string format = @"{0}";
                KeyValuePair<RhythmByWeek, DayOfWeek>? weekDetalization = monthlyFrequency.DayOfWeek;
                if (weekDetalization.HasValue)
                {
                    CultureInfo russi = new CultureInfo("ru-RU");
                    DayOfWeek currWeek = weekDetalization.Value.Value;
                    RhythmByWeek currWeekPart = weekDetalization.Value.Key;
                    //string masculine = GetMasculineDayOfWeek(currWeek);
                    addIns.Add(string.Format(format, "каждый"));//, masculine));//evry
                    addIns.Add(string.Format(format, GetNameRhythmByWeek(currWeekPart)));//, masculine));//currWeekPart.ToString().ToLower()
                    addIns.Add(string.Format("{0}", russi.DateTimeFormat.DayNames[(int)currWeek]));
                }
                addIns.Add(monthlyFrequency.Period == 1
                               ? @"каждый"
                               : string.Format(DescriptionFormattingString, @"каждые", monthlyFrequency.Period));
                addIns.Add(DeclinableForDates(monthlyFrequency.Occurs,
                                              monthlyFrequency.Period == 1 ? 2 : monthlyFrequency.Period));
            }
            return addIns;
        }

        private IEnumerable<string> AddWeeklyAddIns(WeeklyFrequency weeklyFrequency)
        {
            var addIns = new List<string> 
                             {
                                 weeklyFrequency.Period == 1
                                     ? @"каждую" //every_w
                                     : string.Format(DescriptionFormattingString, @"каждые" , weeklyFrequency.Period),//@"every(s)"
                                 DeclinableForDates(weeklyFrequency.Occurs, weeklyFrequency.Period)
                             };
            List<DayOfWeek> recuringDays = weeklyFrequency.RecuringDays;
            CultureInfo russi = new CultureInfo("ru-RU");
            
            if (recuringDays.Count > 0)
            {
                const string declinable = @"{0}";
                addIns.Add("в");
                addIns.AddRange(
                    recuringDays.Select(
                        (t, i) =>
                        i == (recuringDays.Count - 1)
                            ? string.Format(declinable, russi.DateTimeFormat.DayNames[(int)t])
                            : string.Format(DescriptionFormattingString, string.Format(declinable, russi.DateTimeFormat.DayNames[(int)t]),
                                            i == (recuringDays.Count - 2)
                                                ? @"и"// "and" //_resourceStore.GetValueForKey(ResourceType.PageContent, "and")
                                                : ",")));
            }
            return addIns;
        }

        private static IEnumerable<string> AddDailyAddIns(DailyFrequency dailyFrequency)
        {
            return new List<string>
                       {
                           dailyFrequency.Period == 1
                               ?@"каждый"// @"every"
                               : string.Format(DescriptionFormattingString, @"каждые" , dailyFrequency.Period),//@"every(s)"
                           DeclinableForDates(dailyFrequency.Occurs, dailyFrequency.Period)
                       };
        }

        private static IList<string> AddUnitaryAddIns(SingleLanchFrequency singleLanchFrequency)
        {
            var addIns = new List<string>
                             {
                                 string.Format(DescriptionFormattingString, @"ѕроисходит",
                                               singleLanchFrequency.DurationFrom.ToShortDateString())
                             };
            addIns.AddRange(AddTimeAddIns(singleLanchFrequency.DailyFrequency));
            return addIns;
        }

        #endregion

        #region Date/Time Localization Helpers

        /// <summary>
        /// ¬озвращает ключ локализуемого ресурса, пременимый к текущему условию склонени€ типов дат.
        /// </summary>
        /// <param name="rhythm">—клон€емый тип даты.</param>
        /// <param name="value">«начение склон€емого типа.</param>
        /// <returns> люч объекта локализации.</returns>
        public static string DeclinableForDates(RhythmByDate rhythm, int value)
        {
            if (value > 100 || value < 0)
                throw new ArgumentOutOfRangeException("value", @"Parameter value must between 1 to 100.");
            int part = value > 20 ? value%10 : value;
            switch (rhythm)
            {
                case RhythmByDate.Daily:
                    return part == 1 ? "день" : (part > 4 ? "дней" : "дн€");//day day(s) ofDay
                case RhythmByDate.Weekly:
                    return part == 1 ? "неделю" : (part > 4 ? "недель" : "недели"); //everyWeek week(s) ofWeek
                case RhythmByDate.Monthly:
                    return part == 1 ? "мес€ц" : (part > 4 ? "мес€цев" : "мес€ца");
            }
            return string.Empty;
        }

        /// <summary>
        /// ¬озвращает ключ локализуемого ресурса, пременимый к текущему условию склонени€ типов времени.
        /// </summary>
        /// <param name="rhythm">—клон€емый тип времени.</param>
        /// <param name="value">«начение склон€емого типа.</param>
        /// <returns> люч объекта локализации.</returns>
        public static string DeclinableForTimes(RhythmByTime rhythm, int value)
        {
            if (value > 100 || value < 0)
                throw new ArgumentOutOfRangeException("value", @"Parameter value must between 1 to 100.");
            int part = value > 20 ? value%10 : value;
            switch (rhythm)
            {
                case RhythmByTime.Hours:
                    return part == 1 ? "час" : (part > 4 ? "часов" : "часа");
                case RhythmByTime.Minutes:
                    return part == 1 ? "минуту" : (part > 4 ? "минут" : "минуты");
                case RhythmByTime.Seconds:
                    return part == 1 ? "секунду" : (part > 4 ? "секунд" : "секунды");
            }
            return string.Empty;
        }

        /// <summary>
        /// ¬озвращает локализованное строковое представление склонени€ чисел.
        /// </summary>
        /// <param name="num">«начение числа.</param>
        /// <param name="masculine">–од слова, с которым используетс€ число</param>
        /// <returns>Ћокализованное значение строкового представлени€ числа.</returns>
        public string StringNumericEndOfWord(int num, char masculine)
        {
            //masculine = ' ';
            //const string format = @"{0}_{1}";
            string end;
            switch (num)
            {
                case 1:
                    return @"первый";
            //        end = string.Format(format, masculine, @"первый");
                    //break;
                case 2:
                    return @"второй";
                    //end = string.Format(format, masculine, @"второй");
                    //break;
                case 3:
                    return @"третий";
                    //end = string.Format(format, masculine, @"третий");
                    //break;
                default:
                    return num.ToString();
                    //end = string.Format(format, masculine, @"th");
                    break;
            }
            return string.Format(end, num); //string.Format(_resourceStore.GetValueForKey(ResourceType.PageContent, end), num);
        }

        private static string GetMasculineDayOfWeek(DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Sunday)
                return "s";
            if (dayOfWeek == DayOfWeek.Wednesday || dayOfWeek == DayOfWeek.Friday || dayOfWeek == DayOfWeek.Saturday)
                return "w";
            return "m";
        }

        #endregion
    }
}