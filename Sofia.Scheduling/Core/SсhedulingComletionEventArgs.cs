namespace Sofia.Scheduling.Core
{
    /// <summary>
    /// ѕредставл€ет описание данных о событии завершени€ работы планировщика.
    /// </summary>
    public class SchedulingComletionEventArgs
    {
        /// <summary>
        /// ¬озвращает сведени€ о €дре, завершившим свою де€тельность.
        /// </summary>
        public ScheduleInstance Instance { get; internal set; }
    }
}