namespace HG.Base.NHibernate.Domain
{
    public interface IHasAssignedId<in TId>
    {
        void SetAssignedIdTo(TId assignedId);
    }
}