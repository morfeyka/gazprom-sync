namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// Представляет правила для единичного вызова задания.
    /// </summary>
    public interface IOneTimeOccurrence
    {
        /// <summary>
        /// Возвращает признак разового выполнения задания.
        /// </summary>
        bool IsSingleLanch { get; }
    }
}