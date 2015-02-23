using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using Sofia.Data.Logic;
using Sofia.Domain.Setting.Appointment.Sheduling;
using Sofia.Domain.Setting.Log;
using Sofia.Dto;
using Sofia.Scheduling.Core;

namespace Sofia.Scheduling.Schedulers
{
    /// <summary>
    /// Представляет общее описание планировщика, хранящего сведения о себе в источнике данных.
    /// </summary>
    /// <typeparam name="TSheduler">Производный тип от <see cref="Sheduler"/>.</typeparam>
    public abstract class SchedulingEntity<TSheduler> : ISchedulingEntity where TSheduler : Sheduler
    {
        private readonly ScheduleInstance _heart;
        private readonly int _id;

        #region Ctors

        protected SchedulingEntity(int id)
        {
            _id = id;
            using (ISession session = NHibernateHelper.SessionManager.GetSessionFor<WebEnvironmentFiact>().SessionFactory.OpenSession())
            {
                Sheduler sheduler = session.Get<TSheduler>(id);
                var isSynchronized = CorrectTime(sheduler);
                DateTime? fixedTime = sheduler.NextRun;
                if (isSynchronized || !fixedTime.HasValue)
                {
                    session.Save(sheduler);
                    session.Flush();
                    if (!fixedTime.HasValue)
                        return;
                }
                TimeSpan launch = fixedTime.Value.Subtract(DateTime.Now);
                State = TaskExecutionType.Idle;
                _heart = new ScheduleInstance(launch);
            }
            _heart.Tripped += InstanceTripped;
            _heart.Completed += (obj, args) => OnCompleted(this, args);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Возвращает признак активности планировщика.
        /// </summary>
        public bool IsAlive
        {
            get { return _heart != null; }
        }

        /// <summary>
        /// Возвращает экземпляр объекта планировщика из источника данных.
        /// </summary>
        protected TSheduler Entity
        {
            get
            {
                TSheduler entity;
                using (ISession session = NHibernateHelper.SessionManager.GetSessionFor<WebEnvironmentFiact>().SessionFactory.OpenSession())
                    entity = session.Get<TSheduler>(_id);
                return entity;
            }
        }

        /// <summary>
        /// Возвращает системный идентификатор планировщика.
        /// </summary>
        public int Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Возвращает текущее состояние задания.
        /// </summary>
        public TaskExecutionType State { get; private set; }

        #region Implementation of IExecutionState

        /// <summary>
        /// Возвращает дату последнего запуска на выполнение.
        /// </summary>
        public DateTime? LastRun
        {
            get { return _heart.LastRun; }
        }

        public string Name { get { return Entity.Name; } }
        public string Type { get { return Entity.Type; } }

        /// <summary>
        /// Возвращает дату последующего запуска на выполнение.
        /// </summary>
        public DateTime NextRun
        {
            get { return _heart.NextRun; }
        }

        /// <summary>
        /// Возвращает длительность (сек.) последнего выполнения.
        /// </summary>
        public decimal LastDuration
        {
            get { return _heart.LastDuration; }
        }

        /// <summary>
        /// Возвращает общее количество запусков.
        /// </summary>
        public int CountLaunches
        {
            get { return _heart.CountLaunches; }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Представляет событие, возникающее при окончании работы планировщика.
        /// </summary>
        public event SchedulingComleteEventHandler Completed;

        #endregion

        /// <summary>
        /// Метод, описывающий задачу, выполняемую планировщиком.
        /// </summary>
        public abstract void DoWork(object idStatus);
        private CancellationTokenSource _cancelTokSource;

        public  bool IsRemove;

        public void KillShedule()
        {
            if (_cancelTokSource != null)
            {
                lock (_cancelTokSource)
                {
                    _cancelTokSource.Cancel();
                }
            }
        }
        StatusTask _status;
        int _recId;
        private void InstanceTripped(SchedulingEventArg arg)
        {
            lock (_heart)
            {


                Sheduler sheduler = null;
                using (
                    ISession session =
                        NHibernateHelper.SessionManager.GetSessionFor<WebEnvironmentFiact>().SessionFactory.OpenSession()
                    )
                {
                    try
                    {
                        sheduler = session.Get<TSheduler>(_id);
                        if (sheduler == null)
                        {
                            arg.Interval = -1;
                            return;
                        }
                        if (_cancelTokSource != null)
                        {
                            arg.Interval = -1;
                            return;
                        }
                        State = TaskExecutionType.Running;
                        sheduler.LastRun = _heart.LastRun;
                        sheduler.Iteration = _heart.CountLaunches;
                        _cancelTokSource = new CancellationTokenSource();

                        _status = new StatusTask
                        {
                            StartRun = DateTime.Now,
                            EndRun = null,
                            Error = null,
                            Sheduler = session.Get<Sheduler>(Id),
                            TaskExecType = TaskExecType.Running
                        };

                        session.Save(_status);
                        _recId = _status.Id;
                        var keyPair = new KeyValuePair<int, CancellationToken>(_status.Id, _cancelTokSource.Token);
                        Task task = null;
                        try
                        {
                            task = Task.Factory.StartNew(x =>
                            {
                                var key =
                                    (KeyValuePair<int, CancellationToken>)x;
                                if (!key.Value.IsCancellationRequested)
                                {
                                    var t = new Thread(DoWork);
                                    t.Start(key.Key);
                                    while (t.IsAlive &&
                                           !key.Value.IsCancellationRequested)
                                    {
                                    }
                                    if (t.IsAlive)
                                    {
                                        t.Abort();
                                    }
                                }
                            }, keyPair, _cancelTokSource.Token);
                            if (sheduler.IsKill && sheduler.Runtime.HasValue)
                            {
                                task.Wait(new TimeSpan(0, sheduler.Runtime.Value, 0));
                                if (task.IsCompleted && _cancelTokSource != null &&
                                    !_cancelTokSource.IsCancellationRequested)
                                    State = TaskExecutionType.Succeed;
                                else if (task.Status != TaskStatus.Canceled && task.Status != TaskStatus.RanToCompletion &&
                                         task.Status != TaskStatus.Faulted)
                                {
                                    _cancelTokSource.Cancel();
                                    State = TaskExecutionType.Failure;
                                }
                                else
                                {
                                    State = TaskExecutionType.Failure;
                                }
                            }
                            else
                            {
                                task.Wait();
                                if (task.IsCompleted && _cancelTokSource != null &&
                                    !_cancelTokSource.IsCancellationRequested)
                                    State = TaskExecutionType.Succeed;
                                else
                                {
                                    State = TaskExecutionType.Failure;
                                }
                            }
                            Task.WaitAll(new[] { task });
                            if (task.Status != TaskStatus.Canceled && task.Status != TaskStatus.RanToCompletion &&
                                task.Status != TaskStatus.Faulted)
                                task.Dispose();
                        }
                        catch (InvalidOperationException)
                        {
                            State = TaskExecutionType.Failure;
                            if (task != null)
                                Task.WaitAll(new[] { task });

                        }
                        catch (AggregateException)
                        {
                            State = TaskExecutionType.Failure;
                            if (task != null)
                                Task.WaitAll(new[] { task });

                        }
                        catch (OperationCanceledException)
                        {
                            State = TaskExecutionType.Failure;
                            if (task != null)
                                Task.WaitAll(new[] { task });
                        }
                        catch (ThreadAbortException)
                        {
                            State = TaskExecutionType.Failure;
                            Thread.ResetAbort();
                            if (task != null)
                                Task.WaitAll(new[] { task });
                        }
                        finally
                        {
                            if (task != null && task.Status != TaskStatus.RanToCompletion)
                            {
                                Task.WaitAll(new[] { task });
                                if (task.Status != TaskStatus.Canceled &&
                                    task.Status != TaskStatus.Faulted)
                                    task.Dispose();
                            }
                        }
                    }
                    catch
                    {
                        State = TaskExecutionType.Failure;
                    }

                    finally
                    {

                        if (sheduler != null)
                        {
                            if (_status != null)
                            {
                                session.Refresh(_status);
                                _status = session.Get<StatusTask>(_recId);
                                if (_status.TaskExecType != TaskExecType.Failure)
                                {
                                    _status.TaskExecType =
                                        (TaskExecType) Enum.Parse(typeof (TaskExecType), ((int) State).ToString());
                                    _status.EndRun = DateTime.Now;
                                    session.Save(_status);
                                }
                            }
                            sheduler.Duration = _heart.LastDuration;
                            sheduler.UpdateShedule();

                            DateTime? next = sheduler.NextRun;
                            arg.Interval = !next.HasValue
                                               ? -1
                                               : (long)next.Value.Subtract(DateTime.Now).TotalMilliseconds;
                            if (IsRemove)
                            {
                                arg.Interval = -1;
                            }
                            sheduler.IsEnabled = next.HasValue;
                            session.Save(sheduler);
                            session.Flush();
                            _cancelTokSource = null;
                            _status = null;
                        }
                    }
                }
            }
        }

        private static bool CorrectTime(Sheduler sheduler)
        {
            bool cng = false;
            if (sheduler == null) return false;
            if (!sheduler.NextRun.HasValue) return false;
            if (sheduler.NextRun.Value <= DateTime.Now)
            {
                sheduler.UpdateShedule();
                cng = true;
            }
            return cng;
        }

        protected void OnCompleted(object sender, EventArgs args)
        {
            if (Completed != null)
                Completed(sender, args);
        }
    }
}