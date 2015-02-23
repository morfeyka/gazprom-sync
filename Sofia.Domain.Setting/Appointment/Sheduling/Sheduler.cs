using System;
using Sofia.Domain.Setting.Appointment.Periodicity;

namespace Sofia.Domain.Setting.Appointment.Sheduling
{
    /// <summary>
    /// ������������ ����� �������� ������������ �������.
    /// </summary>
    public abstract class Sheduler:Entity
    {
        #region Fields

        private readonly DateTime _createdOn;
        private int _iteration;
        private SheduleFrequencyProperty _frequency;
        private bool _isEnabled;
        private bool _isKill;
        private readonly string _type;
        #endregion

        protected Sheduler():this("import", true)
        {
        }

        protected Sheduler(string type, bool isPeriodic)
        {
            _createdOn = DateTime.Now;
            _iteration = 0;
            _isEnabled = true;
            _isKill = false;
            _type = type;
            if (isPeriodic)
                _frequency = new DailyFrequency();
            else
                _frequency = new SingleLanchFrequency(DateTime.Now.AddHours(2));
        }

        public virtual string Type
        {
            get { return _type; }
        }

        /// <summary>
        /// ���������� ��� ����� ��������.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// ���������� ���� ��������.
        /// </summary>
        public virtual DateTime CreatedOn { get{ return _createdOn;} }

        /// <summary>
        /// ���������� ��� ����� ���� ���������� �������.
        /// </summary>
        public virtual DateTime? LastRun { get; set; }

        /// <summary>
        /// ���������� ��� ����� ���� ���������� �������.
        /// </summary>
        public virtual DateTime? NextRun { get; set; }

        /// <summary>
        /// ���������� ��� ����� ������������ ���������� �������.
        /// </summary>
        public virtual decimal? Duration { get; set; }

        /// <summary>
        /// ���������� ��� ����� ����������� ��������� ������������ ���������� �������.
        /// </summary>
        public virtual int? Runtime { get; set; }

        /// <summary>
        /// ���������� ��� ����� ����� ���������� ��������.
        /// </summary>
        public virtual int Iteration
        {
            get{ return _iteration;}
            set{ _iteration = value;}
        }

        /// <summary>
        /// ���������� ��� ����� ������������ ������������� �������.
        /// </summary>
        public virtual SheduleFrequencyProperty Frequency
        {
            get{ return _frequency;}
            set{ _frequency = value;}
        }
        /// <summary>
        /// ���������� ��� ����� ��������, ������������� ����������.
        /// </summary>
        public virtual bool IsEnabled
        {
            get{ return _isEnabled;}
            set{ _isEnabled = value;}
        }

        /// <summary>
        /// ���������� ��� ����� ��������, ������������� ���������� ��� ��� ��������� ������ ��������� ������� �� ���������� ������� <see cref="Runtime"/>.
        /// </summary>
        public virtual bool IsKill
        {
            get { return _isKill; }
            set { _isKill = value; }
        }

        /// <summary>
        /// ���������� ��� ������ �������� ��� �������� � ������
        /// </summary>
        public virtual string Param { get; set; }

        /// <summary>
        /// ���������� ��� ������ ���� ��� ������������ ������ ��������� �������
        /// </summary>
        public virtual DateTime? TimeGetData { get; set; }
        /// <summary>
        /// ��������� �������� ���������� �������.
        /// </summary>
        public virtual void UpdateShedule()
        {
            NextRun = _frequency.GetNextLaunching(LastRun);
            _isEnabled = NextRun.HasValue;
        }
    }
}