using System;
using System.Diagnostics;
using System.Threading;
using Sofia.Contracts.Data;
namespace Sofia.Scheduling.Core
{
    /// <summary>
    /// ������������ �������� ���� ������������ �������.
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
        /// ���������� ���� ������������ ������� �� ����������.
        /// </summary>
        public DateTime NextRun { get; set; }

        /// <summary>
        /// ���������� ������������ (� ��������) ������ ������������ � ������� ��������.
        /// </summary>
        public decimal LastDuration
        {
            get { return (decimal) _monitor.Elapsed.TotalSeconds; }
        }

        /// <summary>
        /// ���������� ��� ����� ����� ���������� ������� ������� ������������.
        /// </summary>
        public DateTime? LastRun { get; set; }

        /// <summary>
        /// ���������� ����� ���������� �������� ������� ������������
        /// </summary>
        public int CountLaunches
        {
            get { return _numberOfLaunches; }
        }

        #endregion

        #region Ctors

        /// <summary>
        /// �������������� ����� ���� ���������� � ������������ � ��������� ��������� ������� ������� � �������� ���������� �������,
        /// ������ ������ ����.
        /// </summary>
        /// <param name="dueTime">�������� �������, ����� ������� ��������� ������ ���������� �������.</param>
        public ScheduleInstance(TimeSpan dueTime)
            : this(dueTime, TimeSpan.FromMinutes(60))
        {
        }

        /// <summary>
        /// �������������� ���� ���������� ������� � ������������ � ���������� ��������� �� ������� ������� ������� � �������� ��� �������.
        /// </summary>
        /// <param name="dueTime">�������� �������, ����� ������� ��������� ������ ���������� �������.</param>
        /// <param name="period">������ ������� ���������� �������.</param>
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
        /// ������������ �������, ����������� ��� ����������� ������� ���������� ������� ������������.
        /// </summary>
        public event SchedulingEventHandler Tripped;

        /// <summary>
        /// ������������ �������, ��������������� �� ��������� ���������� ���������� ������������.
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