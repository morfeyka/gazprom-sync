namespace Sofia.Scheduling.Core
{
    /// <summary>
    /// ������������ �������� ������ � ������� ���������� ������ ������������.
    /// </summary>
    public class SchedulingComletionEventArgs
    {
        /// <summary>
        /// ���������� �������� � ����, ����������� ���� ������������.
        /// </summary>
        public ScheduleInstance Instance { get; internal set; }
    }
}