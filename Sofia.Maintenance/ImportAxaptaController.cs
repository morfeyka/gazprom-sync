using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using Sofia.Contracts;
using Sofia.Data.Logic;
using Sofia.Data;
using Sofia.Domain.Setting.Appointment.Sheduling;
using Sofia.Dto;
using Sofia.Scheduling.Schedulers;
using NHibernate;
namespace Sofia.Maintenance
{
    /// <summary>
    /// ѕредставл€ет описание службы по импорту данных из Axapta в Factelligence.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, MaxItemsInObjectGraph = 2147483647)]
    public class ImportAxaptaController : IImportAxapta
    {
        private static HashSet<SchedulingEntity<Sheduler>> _schedulers;

        private ImportAxaptaController()
        {
            _schedulers = new HashSet<SchedulingEntity<Sheduler>>();
            LoadSchedules();
        }

        private void LoadSchedules()
        {

            List<Sheduler> schIds =
                Db<WebEnvironmentFiact>.Get<Sheduler>().Where(x => x.NextRun != default(DateTime?) && x.IsEnabled).
                    ToList();
            foreach (Sheduler shedule in
                schIds)
            {
                if (shedule.Type == "import")
                {
                    var import = new Import(shedule.Id);
                    import.Completed += shedule_Completed;
                    _schedulers.Add(import);
                }
                if (shedule.Type == "export")
                {
                    var export = new Export(shedule.Id);
                    export.Completed += shedule_Completed;
                    _schedulers.Add(export);

                }
                if (shedule.Type == "more")
                {
                    var more = new More(shedule.Id);
                    more.Completed += shedule_Completed;
                    _schedulers.Add(more);
                }

                if (shedule.Type == "eitp_hour")
                {
                    var more = new EitpHourlyJob(shedule.Id);
                    more.Completed += shedule_Completed;
                    _schedulers.Add(more);
                }

                if (shedule.Type == "eitp_daily")
                {
                    var more = new EitpDailyJob(shedule.Id);
                    more.Completed += shedule_Completed;
                    _schedulers.Add(more);
                }
            }
        }

        private void shedule_Completed(object sender, EventArgs args)
        {
            var shedule = sender as SchedulingEntity<Sheduler>;
            if (shedule != null)
                lock (_schedulers)
                    _schedulers.Remove(shedule);
        }

        #region Implementation of IImportAxapta

        /// <summary>
        /// ¬ыполн€ет добавление нового задани€ в список задач шедулера.
        /// </summary>
        /// <param name="id"></param>
        public void AddShedule(int id)
        {
            lock (_schedulers)
            {
                if (_schedulers.All(x => x.Id != id))
                {
                    var task = Db<WebEnvironmentFiact>.NSession.Get<Sheduler>(id);
                    if (task.Type == "import")
                    {
                        var import = new Import(task.Id);
                        _schedulers.Add(import);
                        import.Completed += shedule_Completed;
                    }
                    if (task.Type == "export")
                    {
                        var export = new Export(task.Id);
                        _schedulers.Add(export);
                        export.Completed += shedule_Completed;

                    }
                    if (task.Type == "more")
                    {
                        var more = new More(task.Id);
                        _schedulers.Add(more);
                        more.Completed += shedule_Completed;
                    }
                }
            }
        }

        /// <summary>
        /// ¬озвращает набор данных об текущих активных задани€х.
        /// </summary>
        /// <returns>—писок из транспортных объектов сведений о выполнении заданий.</returns>
        public List<SchedulerDto> GetSummaryInfo()
        {
            
            return   _schedulers.ToList().Select(
                    x =>
                    new SchedulerDto
                        {
                            CountLaunches = x.CountLaunches,
                            LastDuration = x.LastDuration,
                            LastRun = x.LastRun,
                            NextRun = x.NextRun,
                            Id = x.Id,
                            State = x.State,
                            Name = x.Name,
                            Type = x.Type
                        }).ToList();
        }

        /// <summary>
        /// ¬ыполн€ет удаление задани€ из списока задач шедулера.
        /// </summary>
        /// <param name="id"></param>
        public void RemoveShedule(int id)
        {
            if (_schedulers.Any(x => x.Id == id))
            {
                var xxx = _schedulers.SingleOrDefault(x => x.Id == id);
                if (xxx != null)
                {
                    xxx.IsRemove = true;
                    if (xxx.State == TaskExecutionType.Running)
                    {
                        xxx.KillShedule();
                    }
                    else if (xxx.State != TaskExecutionType.Running)
                     _schedulers.Remove(xxx);
                }
            }
        }

        /// <summary>
        /// ¬ыполн€ет остановку выполнени€ задани€ содержащегос€ в списке задач.
        /// </summary>
        /// <param name="id"></param>
        public void KillShedule(int id)
        {
                lock (_schedulers)
                {
                    if (_schedulers.Any(x => x.Id == id))
                    {
                        var sched = _schedulers.SingleOrDefault(x => x.Id == id);
                        if (sched != null)
                        {
                            sched.KillShedule();
                        }
                    }
                }
        }

        #endregion
    }
}