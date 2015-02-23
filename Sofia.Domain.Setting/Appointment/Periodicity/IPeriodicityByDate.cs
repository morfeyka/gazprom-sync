using System;

namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// ������������ �������, ����������� ������������� �� ����.
    /// </summary>
    public interface IPeriodicityByDate : IFrequencyOccurrence
    {
        /// <summary>
        /// ���������� ������� �������� �������.
        /// </summary>
        RhythmByDate Occurs { get; }

        /// <summary>
        /// ���������� ��� ����� ��������� ���� �������.
        /// </summary>
        DateTime DurationFrom { get; set; }

        /// <summary>
        /// ���������� ��� ����� �������� ���� �������.
        /// </summary>
        DateTime DurationTo { get; set; }

        /// <summary>
        /// ��������� ��������� ����� ������� �� ��������� ����������.
        /// </summary>
        /// <param name="lastLaunch">���������� ����.</param>
        /// <returns>��������� �������� ������������� �� ����.</returns>
        DateTime? GetNextLaunching(DateTime? lastLaunch);
    }
}