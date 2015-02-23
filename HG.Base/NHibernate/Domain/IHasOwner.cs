namespace HG.Base.NHibernate.Domain
{
    /// <summary>
    /// Представляет правило, в соответствии с которым экземпляр обретает ассоциацию на элемент-родитель доменной модели,
    /// без которого существовать не может.
    /// </summary>
    /// <typeparam name="TEntity">Тип элемента-родителя.</typeparam>
    public interface IHasOwner<TEntity> where TEntity : IEntityWithTypedId<int>
    {
        /// <summary>
        /// Возвращает родительский элемент.
        /// </summary>
        TEntity Owner { get; }

        /// <summary>
        /// Устанавливает объект-родитель для данного элемента.
        /// </summary>
        /// <param name="parent">Объект-родитель.</param>
        void SetOwner(TEntity parent);
    }
}