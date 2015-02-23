using System;

namespace Sofia.Scheduling.Core
{
    /// <summary>
    /// Представляет делегат для события завершения работы шедулера.
    /// </summary>
    /// <param name="sender">Объект, инициировавший событие.</param>
    /// <param name="args">Данные о событии</param>
    public delegate void SchedulingComleteEventHandler(object sender, EventArgs args);
}