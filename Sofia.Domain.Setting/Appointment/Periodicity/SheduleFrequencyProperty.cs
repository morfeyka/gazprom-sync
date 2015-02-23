using System;

namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// ������������ ������� �������� ����������� �������������.
    /// </summary>
    public abstract class SheduleFrequencyProperty : Entity, IPeriodicityByDate
    {
        private TimeSpanFrequency _dailyFrequency;
        private DateTime _durationFrom;
        private DateTime _durationTo;
        private int _period;

        protected SheduleFrequencyProperty()
        {
            _durationFrom = DateTime.Now.Date;
            _durationTo = DateTime.MaxValue.Date;
            _period = 1;
            _dailyFrequency = TimeSpanFrequency.StartOnce();
        }

        /// <summary>
        /// ���������� ��� ����� ������������ ������������� �� �������.
        /// </summary>
        public virtual TimeSpanFrequency DailyFrequency
        {
            get { return _dailyFrequency; }
            set { _dailyFrequency = value; }
        }

        /// <summary>
        /// ���������� ������� ����������� ������������ ������������.
        /// </summary>
        public virtual bool IsInfinite
        {
            get { return _durationTo == DateTime.MaxValue.Date; }
        }

        #region IPeriodicityByDate Members

        /// <summary>
        /// ���������� ��� ����� ��������� ���� ������������.
        /// </summary>
        public virtual DateTime DurationFrom
        {
            get { return _durationFrom; }
            set { _durationFrom = value; }
        }

        /// <summary>
        /// ���������� ��� ����� �������� ���� ������������ (�� ��������� null - �������� ����).
        /// </summary>
        public virtual DateTime DurationTo
        {
            get{ return _durationTo;}
            set{ _durationTo = value;}
        }

        /// <summary>
        /// ��������� ��������� ����� ������� �� ��������� ����������.
        /// </summary>
        /// <param name="lastLaunch">���������� ����.</param>
        /// <returns>��������� �������� ������������� �� ����.</returns>
        public abstract DateTime? GetNextLaunching(DateTime? lastLaunch);

        /// <summary>
        /// ���������� ��� ����� �������� ������� ��� ������� ������������ �������������.
        /// </summary>
        public virtual int Period
        {
            get { return _period; }
            set { _period = value; }
        }

        /// <summary>
        /// ���������� ������������� ������������� �� ����.
        /// </summary>
        public abstract RhythmByDate Occurs { get; }

        #endregion


        protected void SetOneTimeOccurrence(DateTime occursAt)
        {
            _period = 0;
            _durationTo = _durationFrom = occursAt.Date;
            _dailyFrequency = new TimeSpanFrequency(occursAt.TimeOfDay);
        }
    }
}