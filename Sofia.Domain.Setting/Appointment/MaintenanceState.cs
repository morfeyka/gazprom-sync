namespace Sofia.Domain.Setting.Appointment
{
    /// <summary>
    /// Представляет перечислитель состояний для задания.
    /// </summary>
    public enum MaintenanceState
    {
        /// <summary>
        /// Ожидание.
        /// </summary>
        Pending = 0,
        /// <summary>
        /// Завершено.
        /// </summary>
        Finished = 1,
        /// <summary>
        /// Ошибка.
        /// </summary>
        Error = 2,
        /// <summary>
        /// Отменено.
        /// </summary>
        Aborted = 3,
        /// <summary>
        /// Выполняется.
        /// </summary>
        Run = 4
    }
}