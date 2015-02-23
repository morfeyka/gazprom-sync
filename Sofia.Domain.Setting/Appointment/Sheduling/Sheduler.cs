using System;
using Sofia.Domain.Setting.Appointment.Periodicity;

namespace Sofia.Domain.Setting.Appointment.Sheduling
{
    /// <summary>
    /// Представляет общее описание планировщика заданий.
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
        /// Возвращает или задаёт название.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Возвращает дату создания.
        /// </summary>
        public virtual DateTime CreatedOn { get{ return _createdOn;} }

        /// <summary>
        /// Возвращает или задаёт дату последнего запуска.
        /// </summary>
        public virtual DateTime? LastRun { get; set; }

        /// <summary>
        /// Возвращает или задаёт дату следующего запуска.
        /// </summary>
        public virtual DateTime? NextRun { get; set; }

        /// <summary>
        /// Возвращает или задаёт длительность выполнения задания.
        /// </summary>
        public virtual decimal? Duration { get; set; }

        /// <summary>
        /// Возвращает или задаёт максимально возможную длительность выполнения задания.
        /// </summary>
        public virtual int? Runtime { get; set; }

        /// <summary>
        /// Возвращает или задаёт общее количество запусков.
        /// </summary>
        public virtual int Iteration
        {
            get{ return _iteration;}
            set{ _iteration = value;}
        }

        /// <summary>
        /// Возвращает или задаёт конфигурацию периодичности запуска.
        /// </summary>
        public virtual SheduleFrequencyProperty Frequency
        {
            get{ return _frequency;}
            set{ _frequency = value;}
        }
        /// <summary>
        /// Возвращает или задаёт параметр, опеределяющий активность.
        /// </summary>
        public virtual bool IsEnabled
        {
            get{ return _isEnabled;}
            set{ _isEnabled = value;}
        }

        /// <summary>
        /// Возвращает или задаёт параметр, опеределяющий необходимо или нет завершать работу выполения задания по превышению времени <see cref="Runtime"/>.
        /// </summary>
        public virtual bool IsKill
        {
            get { return _isKill; }
            set { _isKill = value; }
        }

        /// <summary>
        /// Возвращает или задает параметр для передачи в задачу
        /// </summary>
        public virtual string Param { get; set; }

        /// <summary>
        /// Возвращает или задает дату для формирования данных заданного времени
        /// </summary>
        public virtual DateTime? TimeGetData { get; set; }
        /// <summary>
        /// Обновляет параметр следующего запуска.
        /// </summary>
        public virtual void UpdateShedule()
        {
            NextRun = _frequency.GetNextLaunching(LastRun);
            _isEnabled = NextRun.HasValue;
        }
    }
}