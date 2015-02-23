namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// ������������ ������� ��� ���������� ������ �������.
    /// </summary>
    public interface IOneTimeOccurrence
    {
        /// <summary>
        /// ���������� ������� �������� ���������� �������.
        /// </summary>
        bool IsSingleLanch { get; }
    }
}