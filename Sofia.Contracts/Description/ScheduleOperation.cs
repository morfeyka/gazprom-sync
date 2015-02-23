using System;

namespace Sofia.Contracts.Description
{
    /// <summary> 
    /// ������������ ������������� ����� ����������, ������������ �������� ������ ������ ��������� ����� ������ �������������.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ScheduleOperation : Attribute, IScheduleAction
    {
        #region Ctors

        /// <summary>
        /// �������������� ����� ���������� ����������, ����������� �������� ������ ������ ��������� ������
        /// �� ���������� ������������� ����� ���� ��� � ���������� � ���� ������
        /// </summary>
        public ScheduleOperation()
        {
            Period = (int) TimeSpan.FromHours(1).TotalSeconds;
            DueTime = (int) TimeSpan.FromMinutes(1).TotalSeconds;
        }

        #endregion

        #region Implementation of IScheduleAction

        /// <summary>
        /// ���������� ��� ����� �������� �������.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ���������� ���������� ������� (� ��������), ����� ������� ������� ������ ���� �������� �� ����������.
        /// </summary>
        public int DueTime { get; set; }

        /// <summary>
        /// ���������� �������� ������� (� ��������), ����� ������� ������� ������ ����������� ������� �������������.
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// ���������� ��� ����� ����������� �� ���������� �������� ������� (�� ���������: -1, ��� ��������� �� ���������� ����������� ��
        /// ���������� ��������).
        /// </summary>
        public int LaunchesLimit { get; set; }

        #endregion
    }
}