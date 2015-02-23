using System;
using System.Diagnostics;
using System.Threading;
using Sofia.Contracts.Data;
namespace Sofia.Scheduling.Core
{
    /// <summary>
    /// ѕредставл€ет описание €дра планировщика заданий.
    /// </summary>
    public class ScheduleInstance : IExecutionState
    {
        #region Fields

        private readonly TimeSpan _defaultPeriod;
        private readonly Stopwatch _monitor;
        private bool _isComplete;
        private int _numberOfLaunches;

        #endregion

        #region Members

        protected TimeSpan Period;
        protected Timer StateTimer;

        #endregion

        #region Properties

        /// <summary>
        /// ¬озвращает дату последующего запуска на выполнение.
        /// </summary>
        public DateTime NextRun { get; set; }

        /// <summary>
        /// ¬озвращает длительность (в секундах) работы планировщика в текущей итерации.
        /// </summary>
        public decimal LastDuration
        {
            get { return (decimal) _monitor.Elapsed.TotalSeconds; }
        }

        /// <summary>
        /// ¬озвращает или задаЄт врем€ последнего запуска задани€ планировщика.
        /// </summary>
        public DateTime? LastRun { get; set; }

        /// <summary>
        /// ¬озвращает общее количество запусков задани€ планировщика
        /// </summary>
        public int CountLaunches
        {
            get { return _numberOfLaunches; }
        }

        #endregion

        #region Ctors

        /// <summary>
        /// »нициализирует новый план выполнени€ в соответствии с указанным смещением времени запуска и периодом повторного запуска,
        /// равным одному часу.
        /// </summary>
        /// <param name="dueTime">—мещение времени, через которое произойдЄт первое выполнение задани€.</param>
        public ScheduleInstance(TimeSpan dueTime)
            : this(dueTime, TimeSpan.FromMinutes(60))
        {
        }

        /// <summary>
        /// »нициализирует план выполнени€ задани€ в соответствии с указанными смещением по времени запуска задани€ и периодом его повтора.
        /// </summary>
        /// <param name="dueTime">—мещение времени, через которое произойдЄт первое выполнение задани€.</param>
        /// <param name="period">ѕериод повтора выполнени€ задани€.</param>
        public ScheduleInstance(TimeSpan dueTime, TimeSpan period)
        {
            _defaultPeriod = period;
            StateTimer = new Timer(DoWork, new AutoResetEvent(false), dueTime, _defaultPeriod);
            NextRun = DateTime.Now.Add(dueTime);
            _monitor = new Stopwatch();
        }

        #endregion

        #region Events

        /// <summary>
        /// ѕредставл€ет событие, возникающее при наступлении момента выполнени€ задани€ планировщика.
        /// </summary>
        public event SchedulingEventHandler Tripped;

        /// <summary>
        /// ѕредставл€ет событие, сигнализирующее об окончании выполнени€ расписани€ планировщика.
        /// </summary>
        public event SchedulingComleteEventHandler Completed;

        #endregion

        protected void DoWork(object state)
        {
            _numberOfLaunches++;
            _monitor.Start();
            LastRun = DateTime.Now;
            OnTripped(new SchedulingEventArg {Interval = (long) _defaultPeriod.TotalMilliseconds});
            _monitor.Reset();
            var autoEvent = state as AutoResetEvent;
            if (autoEvent == null) return;
            if (_isComplete)
            {
                StateTimer.Dispose();
                autoEvent.Dispose();
                OnCompleted(this, new EventArgs());
            }
            else
                autoEvent.Set();
        }

        protected void OnTripped(SchedulingEventArg arg)
        {
            if (Tripped == null) return;
            Tripped(arg);
            if (arg.Interval > 0)
            {
                TimeSpan period = TimeSpan.FromMilliseconds(arg.Interval);
                NextRun = DateTime.Now.Add(period);
                if (!period.Equals(_defaultPeriod))
                    StateTimer.Change(arg.Interval, arg.Interval);
            }
            else
                _isComplete = true;
        }

        protected void OnCompleted(object sender, EventArgs args)
        {
            if (Completed != null)
                Completed(sender, args);
        }
    }
}