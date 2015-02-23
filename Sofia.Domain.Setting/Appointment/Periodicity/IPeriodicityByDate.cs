using System;

namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// Представляет правило, описывающее периодичность по дате.
    /// </summary>
    public interface IPeriodicityByDate : IFrequencyOccurrence
    {
        /// <summary>
        /// Возвращает текущее значение периода.
        /// </summary>
        RhythmByDate Occurs { get; }

        /// <summary>
        /// Возвращает или задаёт начальную дату периода.
        /// </summary>
        DateTime DurationFrom { get; set; }

        /// <summary>
        /// Возвращает или задаёт конечную дату периода.
        /// </summary>
        DateTime DurationTo { get; set; }

        /// <summary>
        /// Вычисляет следующую точку периода на основании предыдущей.
        /// </summary>
        /// <param name="lastLaunch">Предыдущая дата.</param>
        /// <returns>Следующее значение периодичности по дате.</returns>
        DateTime? GetNextLaunching(DateTime? lastLaunch);
    }
}