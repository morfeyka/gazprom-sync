namespace Sofia.Domain.Setting.Appointment.Periodicity
{
    /// <summary>
    /// ������������ ������� ��� �������� �������������.
    /// </summary>
    public interface IFrequencyOccurrence
    {
        /// <summary>
        /// ���������� ��� ����� �������� �������.
        /// </summary>
        int Period { get; set; }
    }
}