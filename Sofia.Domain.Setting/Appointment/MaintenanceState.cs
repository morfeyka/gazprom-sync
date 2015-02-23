namespace Sofia.Domain.Setting.Appointment
{
    /// <summary>
    /// ������������ ������������� ��������� ��� �������.
    /// </summary>
    public enum MaintenanceState
    {
        /// <summary>
        /// ��������.
        /// </summary>
        Pending = 0,
        /// <summary>
        /// ���������.
        /// </summary>
        Finished = 1,
        /// <summary>
        /// ������.
        /// </summary>
        Error = 2,
        /// <summary>
        /// ��������.
        /// </summary>
        Aborted = 3,
        /// <summary>
        /// �����������.
        /// </summary>
        Run = 4
    }
}