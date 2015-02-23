using Sofia.Contracts.Data;
using Sofia.Dto;

namespace Sofia.Scheduling.Schedulers
{
    /// <summary>
    /// ѕредставл€ет набор правил, описывающих процесс состо€ни€ выполнени€ задани€,
    /// сведени€ о котором хран€тс€ в источнике данных
    /// </summary>
    public interface ISchedulingEntity : IExecutionState
    {
        /// <summary>
        /// ¬озвращает идентификатор задани€.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// ¬озвращает текущее состо€ние задани€.
        /// </summary>
        TaskExecutionType State { get; }
    }
}