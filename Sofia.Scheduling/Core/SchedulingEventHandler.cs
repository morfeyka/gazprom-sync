namespace Sofia.Scheduling.Core
{
    /// <summary>
    /// Представляет делегат для события выполнения задания шедулера.
    /// </summary>
    /// <param name="arg">Данные о запуске задания.</param>
    public delegate void SchedulingEventHandler(SchedulingEventArg arg);
}