using System;

namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// ѕредставл€ет правило, описывающее периодичность по времени.
    /// </summary>
    public interface IPeriodicityByTime : IFrequencyOccurrence
    {
        /// <summary>
        /// ¬озвращает или задаЄт текущее значение перечислител€ промежутков времени.
        /// </summary>
        RhythmByTime OccursEvery { get; set; }

        /// <summary>
        /// ¬озвращает или задаЄт начальное значение временного диапазона.
        /// </summary>
        TimeSpan StartingAt { get; set; }

        /// <summary>
        /// ¬озвращает или задаЄт конечное значение временного диапазона.
        /// </summary>
        TimeSpan EndingAt { get; set; }
    }
}