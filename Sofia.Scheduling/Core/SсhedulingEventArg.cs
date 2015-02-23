using System;

namespace Sofia.Scheduling.Core
{
    /// <summary>
    /// ������������ �������� ������ � ������� ������������ ������������.
    /// </summary>
    public class SchedulingEventArg : EventArgs
    {
        /// <summary>
        /// ���������� ��� ����� ��������� �������� ������� (� �������������) ������� ������������.
        /// </summary>
        public long Interval { get; set; }
    }
}