namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// Представляет правило для описания периодичности.
    /// </summary>
    public interface IFrequencyOccurrence
    {
        /// <summary>
        /// Возвращает или задаёт значение периода.
        /// </summary>
        int Period { get; set; }
    }
}