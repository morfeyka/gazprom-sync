using System;

namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// ������������ �������� ���������� ������������ �������� ������� ������������ �������.
    /// </summary>
    public class SingleLanchFrequency : SheduleFrequencyProperty, IOneTimeOccurrence
    {
        #region Ctors

        /// <summary>
        /// �������������� ��������� ������������ ����������� �������� ������� � 2 ����.
        /// </summary>
        public SingleLanchFrequency()
            : this(DateTime.Now.AddHours(2))
        {
        }

        /// <summary>
        /// �������������� ��������� ������������ �������� ������� � ��������� �����.
        /// </summary>
        /// <param name="runAt">����� �������.</param>
        public SingleLanchFrequency(DateTime runAt)
        {
            SetOneTimeOccurrence(runAt);
        }

        #endregion

        #region Overrides of SheduleFrequencyProperty

        /// <summary>
        /// ���������� ������������� ������������� �� ����.
        /// </summary>
        public override RhythmByDate Occurs
        {
            get { return RhythmByDate.OneTime; }
        }

        /// <summary>
        /// ��������� ��������� ����� ������� �� ��������� ����������.
        /// </summary>
        /// <param name="lastLaunch">���������� ����.</param>
        /// <returns>��������� �������� ������������� �� ����.</returns>
        public override DateTime? GetNextLaunching(DateTime? lastLaunch)
        {
            if (lastLaunch.HasValue) return default(DateTime?);
            var next = DurationFrom.Add(DailyFrequency.StartingAt);
            return next <= DateTime.Now ? default(DateTime?) : next;
        }

        #endregion

        #region Implementation of IOneTimeOccurrence

        /// <summary>
        /// ���������� ������� �������� ���������� �������.
        /// </summary>
        public virtual bool IsSingleLanch
        {
            get { return true; }
        }

        #endregion
    }
}