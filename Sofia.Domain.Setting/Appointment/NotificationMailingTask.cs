//using hahaGroup.Core.Domain.Appointment;

//using hahaGroup.Core.Domain.Mailing;
//using Iesi.Collections.Generic;

namespace Sofia.Domain.Setting.Appointment
{
    /// <summary>
    /// Представляет описание задания по рассылке уведомлений после выполнения некоторой задачи.
    /// </summary>
    public class NotificationMailingTask : Maintenance
    {
        //private readonly Iesi.Collections.Generic.ISet<PricelistNotification> _mailingList;

        ///// <summary>
        ///// Инициализирует экземпляр задания по рассылке уведомлений.
        ///// </summary>
        //public NotificationMailingTask()
        //{
        //    _mailingList = new HashedSet<PricelistNotification>();
        //}

        /// <summary>
        /// Возвращает или задаёт количество получателей писем.
        /// </summary>
        public virtual int Senders { get; set; }

        /// <summary>
        /// Возвращает или задаёт количество подтверждений о получении письма.
        /// </summary>
        public virtual int Confirmations { get; set; }

        /// <summary>
        /// Возвращает или задаёт количество повторно разосланных уведомлений.
        /// </summary>
        public virtual int Resendings { get; set; }

        /// <summary>
        /// Возвращает или задаёт задачу для которой выполняется рассылка.
        /// </summary>
        public virtual Maintenance Task { get; set; }

        ///// <summary>
        ///// Возвращает список уведомлений для рассылки.
        ///// </summary>
        //public virtual ReadOnlyCollection<PricelistNotification> MailingList
        //{
        //    get{return new ReadOnlyCollection<PricelistNotification>(new List<PricelistNotification>(_mailingList));}
        //}

        //#region Overrides of DomainObject<int>

        ///// <summary>
        ///// Must be provided to properly compare two objects
        ///// </summary>
        //public override int GetHashCode()
        //{
        //    return GetType().FullName.GetHashCode()
        //           ^ Senders.GetHashCode()
        //           ^ Confirmations.GetHashCode()
        //           ^ Resendings.GetHashCode();
        //}

        //#endregion
    }
}