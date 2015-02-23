using System;
using System.Linq;

namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// ������������ ���������, ����������� ������������ ������������� �� �������.
    /// </summary>
    [Serializable]
    public class TimeSpanFrequency : IPeriodicityByTime, IOneTimeOccurrence
    {
        private int _period;

        #region Ctors
        public TimeSpanFrequency()
        {
        }

        /// <summary>
        /// �������������� ����� ��������� � ��������� ������������� ������� � ��������� �������� ������������� ������� 
        /// � ��������������� ��������� ���������� �������.
        /// </summary>
        /// <param name="period">������.</param>
        /// <param name="occursType">�������� ������������� �������.</param>
        /// <param name="startAt">��������� �������� ��������� �������.</param>
        /// <param name="stopAt">�������� �������� ��������� �������.</param>
        public TimeSpanFrequency(int period, RhythmByTime occursType, TimeSpan startAt, TimeSpan stopAt) : this()
        {
            _period = period;
            OccursEvery = occursType;
            StartingAt = startAt;
            EndingAt = stopAt;
        }

        /// <summary>
        /// �������������� ����� ��������� � ��������� ������������� ������� � ��������� �������� ������������� ������� � �������� ���������� �������.
        /// </summary>
        /// <param name="period">������.</param>
        /// <param name="occursType">�������� ������������� �������.</param>
        /// <param name="startAt">��������� ����� ��� �������.</param>
        public TimeSpanFrequency(int period, RhythmByTime occursType, TimeSpan startAt)
            : this(period, occursType, startAt, TimeSpan.MaxValue)
        {
        }

        /// <summary>
        /// �������������� ����� ��������� � �������� �������������� ���������� � ����� �������, �������������� �� ������� �������.
        /// </summary>
        /// <param name="period">������.</param>
        /// <param name="occursType">�������� ������������� �������.</param>
        public TimeSpanFrequency(int period, RhythmByTime occursType) : this(period, occursType, TimeSpan.MinValue)
        {
        }

        /// <summary>
        /// �������������� ����� ��������� � �������� �������������� ����������.
        /// </summary>
        /// <param name="period">������������� ����������, ��������� � �����.</param>
        public TimeSpanFrequency(int period) : this(period, RhythmByTime.Hours)
        {
        }

        /// <summary>
        /// �������������� ����� ��������� ���������� ������� � ���������� ������� �������, �������������� �� ������� �������.
        /// </summary>
        /// <param name="startAt">����� �������.</param>
        public TimeSpanFrequency(TimeSpan startAt) : this(0, RhythmByTime.Hours, startAt, startAt)
        {
        }

        #endregion

        #region Properties

        #region Implementation of IFrequencyOccurrence

        /// <summary>
        /// ���������� ��� ����� �������� �������.
        /// </summary>
        public int Period
        {
            get { return _period; }
            set
            {
                if (_period == 0)
                    SetOneTimeOccurrence();
                else
                {
                    switch (OccursEvery)
                    {
                        case RhythmByTime.Hours:
                            if (!Enumerable.Range(1, 23).Contains(value))
                                throw new ArgumentOutOfRangeException(
                                    "Period property value for current occurs  must between 1 " + "and 24");
                            break;
                        case RhythmByTime.Minutes:
                            if (!Enumerable.Range(1, 59).Contains(value))
                                throw new ArgumentException(
                                    "Period property value for current occurs must between 1 and 60");
                            break;
                        case RhythmByTime.Seconds:
                            if (!Enumerable.Range(10, 49).Contains(value))
                                throw new ArgumentException(
                                    "Period property value for current occurs must between 10 and 60");
                            break;
                    }
                }
                _period = value;
            }
        }

        #endregion

        #region Implementation of IPeriodicityByTime

        /// <summary>
        /// ���������� ��� ����� ������� �������� ������������� ����������� �������.
        /// </summary>
        public RhythmByTime OccursEvery { get; set; }

        /// <summary>
        /// ���������� ��� ����� ��������� �������� ���������� ���������.
        /// </summary>
        public TimeSpan StartingAt { get; set; }

        /// <summary>
        /// ���������� ��� ����� �������� �������� ���������� ���������.
        /// </summary>
        public TimeSpan EndingAt { get; set; }

        #endregion

        #region Implementation of IOneTimeOccurrence

        /// <summary>
        /// ���������� ������� �������� ���������� �������.
        /// </summary>
        public bool IsSingleLanch
        {
            get { return StartingAt.Equals(EndingAt); }
        }

        #endregion

        #endregion

        private void SetOneTimeOccurrence()
        {
            EndingAt = StartingAt;
            OccursEvery = RhythmByTime.Hours;
        }

        /// <summary>
        /// �������������� ��������� ���������� ������� �� ��������� �� ��������� ��� ������� �������, ������ 2 ����.
        /// </summary>
        /// <returns>��������� ���������� �������.</returns>
        public static TimeSpanFrequency StartOnce()
        {
            return StartOnce(new TimeSpan(2, 0, 0));
        }

        /// <summary>
        /// �������������� ��������� ���������� ������� � ��������� ������� �������.
        /// </summary>
        /// <param name="occursAt">�������� �������.</param>
        /// <returns>��������� ���������� �������.</returns>
        public static TimeSpanFrequency StartOnce(TimeSpan occursAt)
        {
            return new TimeSpanFrequency(occursAt);
        }

        /// <summary>
        /// ���������� ���-��� ������� ����������.
        /// </summary>
        /// <returns>
        /// 32-��������� ����� ����� �� ������, ���������� ���-����� ��� ������� ����������.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return StartingAt.GetHashCode()
                   ^ EndingAt.GetHashCode()
                   ^ Period.GetHashCode()
                   ^ OccursEvery.GetHashCode()
                   ^ IsSingleLanch.GetHashCode();
        }

        /// <summary>
        /// ���������� ��������� ����� ������� ���������� ��������� ��� ���������� �������� �������.
        /// </summary>
        /// <param name="current">�������� �������.</param>
        /// <returns>�������� ������� ��� null, ���� ��������� ������ ���������� ���������.</returns>
        public TimeSpan? GetNextTimeLaunching(TimeSpan current)
        {
            if (IsSingleLanch) 
                return default(TimeSpan?);
            var time = DateTime.Now.TimeOfDay;
            var next = (current > time && AppendPeriod(time) > current) ? current : StartingAt;
            while (next < time)
                next = AppendPeriod(next);
            if (next > EndingAt) return default(TimeSpan?);

            return (next <= StartingAt) ? StartingAt : next;
        }

        private TimeSpan AppendPeriod(TimeSpan current)
        {
            TimeSpan next = current;
            switch (OccursEvery)
            {
                case RhythmByTime.Hours:
                    next = next.Add(new TimeSpan(Period, 0, 0));
                    break;
                case RhythmByTime.Minutes:
                    next = next.Add(new TimeSpan(0, Period, 0));
                    break;
                case RhythmByTime.Seconds:
                    next = next.Add(new TimeSpan(0, 0, Period));
                    break;
            }
            return next;
        }
    }
}