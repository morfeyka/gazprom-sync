using Sofia.Contracts.Data;
using Sofia.Dto;

namespace Sofia.Scheduling.Schedulers
{
    /// <summary>
    /// ������������ ����� ������, ����������� ������� ��������� ���������� �������,
    /// �������� � ������� �������� � ��������� ������
    /// </summary>
    public interface ISchedulingEntity : IExecutionState
    {
        /// <summary>
        /// ���������� ������������� �������.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// ���������� ������� ��������� �������.
        /// </summary>
        TaskExecutionType State { get; }
    }
}