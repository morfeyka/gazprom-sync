using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Sofia.Contracts;
using Sofia.Data;
using Sofia.Data.Logic;
using Sofia.Domain.Setting.Appointment.Periodicity;
using Sofia.Domain.Setting.Appointment.Sheduling;
using Sofia.WebSite.Models;

namespace Sofia.WebSite.Areas.Admin.Controllers
{
    public class ScheduleController : Controller
    {
        //
        // GET: /Admin/Schedule/
        private Sheduler _shaper;

        public ActionResult Index(int? id, string type)
        {
            SchedulerModel model;
            try
            {
                if (id.HasValue)
                {
                    _shaper = Db<WebEnvironmentFiact>.NSession.Get<Sheduler>(id);
                }
                else
                {
                    switch (type)
                    {
                        case "import":
                            _shaper = new ImportSheduling();
                            break;
                        case "export":
                            _shaper = new ExportSheduling();
                            break;
                        case "eitp_hour":
                            _shaper = new EitpHourScheduling();
                            break;
                        case "eitp_daily":
                            _shaper = new EitpDailyScheduling();
                            break;
                        default:
                            _shaper = new MoreSheduling();
                            break;
                    }

                }
                model = LoadSheduling(_shaper.Frequency);
                model.Type = _shaper.Type;                
                model.IsEnabled = _shaper.IsEnabled;
                model.Name = _shaper.Name;
                model.IsKill = _shaper.IsKill;
                model.Runtime = _shaper.Runtime;
                model.Param = _shaper.Param;
                model.TimeGetData = _shaper.TimeGetData;
                if (_shaper.Frequency is SingleLanchFrequency)
                    model.FrequencyType = 0;
                else
                {
                    model.FrequencyType = 1;
                }
                model.Id = _shaper.Id;
                LoadEntity(model);
            }
            catch(Exception e)
            {
                model = new SchedulerModel();
            }
            
            return View(model);
        }
        private void LoadEntity(SchedulerModel model)
        {
            const string def = @"<font color=""red"">{0}</font>";
            //if (!IsFirstLoad) return;
            //_shaper = ParamId == 0 ? new PricelistShaper() : NSession.Get<PricelistShaper>(ParamId);
            //RatePlansSession = new HashSet<int>(_shaper..RatePlans.Select(x => x.ID));
            model.CreatedOn = model.Id == 0 ? string.Format(def,"сегодня" ) : _shaper.CreatedOn.ToString();//GetLabel("today")
            model.LastRun = model.Id == 0 ? string.Format(def, "никогда") : _shaper.LastRun.ToString();//GetLabel("Never")
            model.NextRun = model.Id == 0 ? string.Format(def, "неизвестно" ) : _shaper.NextRun.ToString();//GetLabel("Unknown")
            model.Duration = model.Id == 0 ? string.Format(def,"неизвестно" ) : _shaper.Duration.ToString();//GetLabel("Unknown")
            model.Iteration = _shaper.Iteration.ToString();
            //LoadSheduling(_shaper.Frequency);
            //
            model.Description = new Sheduling().DescriptionFor(_shaper.Frequency);// _sheduling.DescriptionFor();
        }

        [HttpPost]
        public ActionResult Index(SchedulerModel model)
        {
           
            var session = Db<WebEnvironmentFiact>.NSession;
            if (model.Id == 0)
            {
                switch (model.Type)
                {
                    case "import":
                        _shaper = new ImportSheduling();
                        break;
                    case "export":
                        _shaper = new ExportSheduling();
                        break;
                    default:
                        _shaper = new MoreSheduling();
                        break;
                }
            }
            else
            {
                _shaper = session.Get<Sheduler>(model.Id);
                try
                {
                    using (var factory = new ChannelFactory<IImportAxapta>("MyImportAxapta"))
                    {
                        IImportAxapta channel = factory.CreateChannel();
                        var id = _shaper.Id;
                        channel.RemoveShedule(id);
                    }
                    Thread.Sleep(2000);
                }catch(Exception e)
                {
                }
            }

            _shaper.IsEnabled = model.IsEnabled;
            _shaper.IsKill = model.IsKill;
            _shaper.Runtime = model.Runtime;
            _shaper.Param = model.Param;
            _shaper.TimeGetData = model.TimeGetData;
            _shaper.Name = model.Name;
            SheduleFrequencyProperty oldFreq = _shaper.Frequency;
            session.Delete(oldFreq);
            _shaper.Frequency = ApplySheduling(model);
            _shaper.UpdateShedule();
            model = LoadSheduling(_shaper.Frequency);
            model.IsEnabled = _shaper.IsEnabled;

            model.Name = _shaper.Name;
            if (_shaper.Frequency is SingleLanchFrequency)
                model.FrequencyType = 0;
            else
            {
                model.FrequencyType = 1;
            }
            model.Id = _shaper.Id;
            model.Runtime = _shaper.Runtime;
            model.IsKill = _shaper.IsKill;
            model.Param = _shaper.Param;
            model.TimeGetData = _shaper.TimeGetData;
            LoadEntity(model);
            ModelState.Clear();
            session.Save(_shaper);
            session.Flush();
            if (_shaper.IsEnabled)
            {
                try
                {
                    using (var factory = new ChannelFactory<IImportAxapta>("MyImportAxapta"))
                    {
                        IImportAxapta channel = factory.CreateChannel();
                        channel.AddShedule(_shaper.Id);
                    }
                }catch(Exception e)
                {
                }
            }
            return View(model);
        }
        [HttpPost]
        public JsonResult Refresh(SchedulerModel model)
        {
            try
            {
                return Json(new Sheduling().DescriptionFor(ApplySheduling(model)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult CreateTable()
        {
            try
            {
                using (TransactionScope tran = new TransactionScope())
                {
                    new SchemaExport(HG.Base.NHibernate.NhSessionManager.Instance<WebEnvironmentFiact>().CurrentConfiguration.AddAssembly(typeof(ImportSheduling).Assembly)).Execute(true, true, false);    
                    tran.Complete();
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        private SheduleFrequencyProperty ApplySheduling(Sofia.WebSite.Models.SchedulerModel model)
        {
            SheduleFrequencyProperty frequencyProperty = null;
            if (model.FrequencyType == 0)
                return new SingleLanchFrequency(model.cDate);
            var periodType =
                (RhythmByDate)Enum.Parse(typeof(RhythmByDate), (model.ddlDateRhythm).ToString());
            switch (periodType)
            {
                case RhythmByDate.Daily:
                    frequencyProperty = new DailyFrequency { Period = model.DailyPeriod };
                    break;
                case RhythmByDate.Weekly:
                    frequencyProperty = new WeeklyFrequency { Period = model.WeeklyPeriod };
                    var daysOfWeek = new HashSet<DayOfWeek>();
                    if (model.Monday) daysOfWeek.Add(DayOfWeek.Monday);
                    if (model.Tuesday) daysOfWeek.Add(DayOfWeek.Tuesday);
                    if (model.Wednesday) daysOfWeek.Add(DayOfWeek.Wednesday);
                    if (model.Thursday) daysOfWeek.Add(DayOfWeek.Thursday);
                    if (model.Friday) daysOfWeek.Add(DayOfWeek.Friday);
                    if (model.Saturday) daysOfWeek.Add(DayOfWeek.Saturday);
                    if (model.Sunday) daysOfWeek.Add(DayOfWeek.Sunday);
                    ((WeeklyFrequency)frequencyProperty).SetDays(daysOfWeek);
                    break;
                case RhythmByDate.Monthly:
                    frequencyProperty = new MonthlyFrequency(model.RblMonthDetail);
                    if (model.RblMonthDetail==false)
                    {
                        frequencyProperty.Period = model.MonthlyPeriod;
                        ((MonthlyFrequency)frequencyProperty).DayOffset = model.DayOffset;
                    }
                    else
                    {
                        frequencyProperty.Period = model.tbxMonthNumFor;
                        ((MonthlyFrequency)frequencyProperty).DayOfWeek =
                            new KeyValuePair<RhythmByWeek, DayOfWeek>(
                                (RhythmByWeek)
                                Enum.Parse(typeof(RhythmByWeek), model.ddlWeekPart.ToString()),
                                (DayOfWeek)
                                Enum.Parse(typeof(DayOfWeek), model.ddlWeeks.ToString()));
                    }
                    break;
            }
            if (frequencyProperty == null) return null;
            frequencyProperty.DurationFrom = model.DurationFrom.Date;
            if (model.IsInfinite == false)
                frequencyProperty.DurationTo = model.DurationTo.Date;
            frequencyProperty.DailyFrequency = ApplyTimeRange(model);
            return frequencyProperty;
        }

        private TimeSpanFrequency ApplyTimeRange(SchedulerModel model)
        {
            return model.IsSingleLanch == false
                       ? TimeSpanFrequency.StartOnce(model.cTimeConst.TimeOfDay)
                       : new TimeSpanFrequency(model.SingleLanchPeriod,
                                               (RhythmByTime)
                                               Enum.Parse(typeof(RhythmByTime),
                                                          model.OccursEvery.ToString()),
                                               model.StartingAt.TimeOfDay, model.EndingAt.TimeOfDay);
        }

        private SchedulerModel LoadSheduling(SheduleFrequencyProperty sheduler)
        {
            SchedulerModel model = new SchedulerModel();

            switch (sheduler.Occurs)
            {
                case RhythmByDate.OneTime:
                    model.cDate = _shaper.Frequency.DurationFrom.Add(sheduler.DailyFrequency.StartingAt);
                    break;
                case RhythmByDate.Daily:
                    model.DailyPeriod= sheduler.Period;
                    break;
                case RhythmByDate.Weekly:
                    model.WeeklyPeriod = sheduler.Period;
                    List<DayOfWeek> recuring = ((WeeklyFrequency)sheduler).RecuringDays;
                    model.Monday = recuring.Contains(DayOfWeek.Monday);
                    model.Tuesday= recuring.Contains(DayOfWeek.Tuesday);
                    model.Wednesday = recuring.Contains(DayOfWeek.Wednesday);
                    model.Thursday = recuring.Contains(DayOfWeek.Thursday);
                    model.Friday = recuring.Contains(DayOfWeek.Friday);
                    model.Saturday= recuring.Contains(DayOfWeek.Saturday);
                    model.Sunday = recuring.Contains(DayOfWeek.Sunday);
                    break;
                case RhythmByDate.Monthly:
                    model.MonthlyPeriod = model.tbxMonthNumFor = sheduler.Period;
                    if (((MonthlyFrequency)sheduler).IsDefinedByDay)
                    {
                        model.RblMonthDetail = false;
                        model.DayOffset = ((MonthlyFrequency) sheduler).DayOffset;
                    }
                    else
                    {
                        model.RblMonthDetail = true;
                        KeyValuePair<RhythmByWeek, DayOfWeek>? datePart = ((MonthlyFrequency)sheduler).DayOfWeek;
                        if (!datePart.HasValue) return model;
                        foreach (var item in model.WeekPartList)
                        {
                            item.Selected = item.Value == ((int)datePart.Value.Key).ToString();
                            if (item.Selected)
                                model.ddlWeekPart = ((int)datePart.Value.Key);
                        }
                        foreach (var item in model.DayOfWeekList)
                        {
                            item.Selected = item.Value == ((int)datePart.Value.Value).ToString();
                            if (item.Selected)
                            {
                                model.ddlWeeks = (int)datePart.Value.Value;
                            }
                        }
                    }
                    break;
            }
            foreach (var item in model.DateRhythmList)
            {
                item.Selected = item.Value == ((int) sheduler.Occurs).ToString();
                if (item.Selected)
                    model.ddlDateRhythm = (int) sheduler.Occurs;
            }
            TimeSpanFrequency daily = sheduler.DailyFrequency;
            if (daily.IsSingleLanch)
                model.cTimeConst = DateTime.Now.Date.Add(daily.StartingAt);
            else
            {
                model.IsSingleLanch = true;
                //foreach (ListItem item in  IsSingleLanch.Items)
                //{
                //    item.Selected = item.Value == @"1";
                //}
                foreach (var item in model.RhythmByTimeList )
                {
                    item.Selected = item.Value == ((int) daily.OccursEvery).ToString();
                    if (item.Selected)
                        model.OccursEvery = (int) daily.OccursEvery;
                }
                model.StartingAt = new DateTime().Add(daily.StartingAt);
                model.EndingAt = new DateTime().Add(daily.EndingAt);
                model.SingleLanchPeriod = daily.Period;
            }
            model.DurationFrom = sheduler.DurationFrom;
            if (!sheduler.IsInfinite)
            {
                //foreach (ListItem item in rblStopDetail.Items)
                //    item.Selected = item.Value == @"1";
                model.IsInfinite = false;
                model.DurationTo = sheduler.DurationTo;
            }
            else
            {
                model.IsInfinite = true;
            }
            return model;
        }
    }
}
