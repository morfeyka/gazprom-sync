using System;

namespace Sofia.Domain.Setting.Appointment
{
    /// <summary>
    /// ������������ ������� ����� ��������� �������, �� ��������� ������ �������� �������������� ���������� �������-������.
    /// </summary>
    public abstract class Maintenance:Entity
    {
        #region Fields

        private MaintenanceState _jobState;
        private DateTime? _completedOn;
        private DateTime _createdOn;
        private string _errorDescription;
        private int _attempts;

        #endregion

        protected Maintenance()
        {
            _jobState = MaintenanceState.Pending;
            _createdOn = DateTime.Now;
            _attempts = 0;
        }

        #region Properties

        /// <summary>
        /// ���������� ��� ����� ���� �������� �������.
        /// </summary>
        public virtual DateTime CreatedOn
        {
            get{ return _createdOn;}
            protected set { _createdOn = value; }
        }

        /// <summary>
        /// ���������� ��� ����� ���� ������� �������.
        /// </summary>
        public virtual DateTime? LaunchedOn { get; set; }

        /// <summary>
        /// ���������� ��� ����� ���� ���������� �������.
        /// </summary>
        public virtual DateTime? CompletedOn
        {
            get{ return _completedOn;}
            set
            {
                _completedOn = value;
                if (value != null) _jobState = MaintenanceState.Finished;
            }
        }
        /// <summary>
        /// ���������� ��� ����� ������� ��������� �������.
        /// </summary>
        public virtual MaintenanceState JobState
        {
            get { return _jobState; }
            set
            {
                if (value == MaintenanceState.Finished)
                    _jobState = _completedOn == null ? _jobState : value;
                else
                {
                    _jobState = value;
                }
            }
        }

        /// <summary>
        /// ���������� ��� ����� �������� ������, � ������� ����������� ���������� �������� �������.
        /// </summary>
        public virtual string ErrorDescription
        {
            get { return _errorDescription; }
            set
            {
                _errorDescription = value;
                if (value != null)
                    _jobState = MaintenanceState.Error;
            }
        }

        /// <summary>
        /// ���������� ���������� ������� ��������� �������.
        /// </summary>
        public virtual int Attempts
        {
            get{ return _attempts;}
            set{ _attempts = value;}
        }

        #endregion
    }
}