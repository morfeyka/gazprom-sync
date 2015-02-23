//using hahaGroup.Core.Domain.Appointment;

//using hahaGroup.Core.Domain.Mailing;
//using Iesi.Collections.Generic;

namespace Sofia.Domain.Setting.Appointment
{
    /// <summary>
    /// ������������ �������� ������� �� �������� ����������� ����� ���������� ��������� ������.
    /// </summary>
    public class NotificationMailingTask : Maintenance
    {
        //private readonly Iesi.Collections.Generic.ISet<PricelistNotification> _mailingList;

        ///// <summary>
        ///// �������������� ��������� ������� �� �������� �����������.
        ///// </summary>
        //public NotificationMailingTask()
        //{
        //    _mailingList = new HashedSet<PricelistNotification>();
        //}

        /// <summary>
        /// ���������� ��� ����� ���������� ����������� �����.
        /// </summary>
        public virtual int Senders { get; set; }

        /// <summary>
        /// ���������� ��� ����� ���������� ������������� � ��������� ������.
        /// </summary>
        public virtual int Confirmations { get; set; }

        /// <summary>
        /// ���������� ��� ����� ���������� �������� ����������� �����������.
        /// </summary>
        public virtual int Resendings { get; set; }

        /// <summary>
        /// ���������� ��� ����� ������ ��� ������� ����������� ��������.
        /// </summary>
        public virtual Maintenance Task { get; set; }

        ///// <summary>
        ///// ���������� ������ ����������� ��� ��������.
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