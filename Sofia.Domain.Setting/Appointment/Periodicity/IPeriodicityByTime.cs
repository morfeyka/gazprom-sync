using System;

namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// ������������ �������, ����������� ������������� �� �������.
    /// </summary>
    public interface IPeriodicityByTime : IFrequencyOccurrence
    {
        /// <summary>
        /// ���������� ��� ����� ������� �������� ������������� ����������� �������.
        /// </summary>
        RhythmByTime OccursEvery { get; set; }

        /// <summary>
        /// ���������� ��� ����� ��������� �������� ���������� ���������.
        /// </summary>
        TimeSpan StartingAt { get; set; }

        /// <summary>
        /// ���������� ��� ����� �������� �������� ���������� ���������.
        /// </summary>
        TimeSpan EndingAt { get; set; }
    }
}