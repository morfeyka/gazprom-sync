using System.Runtime.Serialization;

namespace Sofia.Contracts.Data
{
    /// <summary>
    /// Представляет описание элемента иерархии кода.
    /// </summary>
    [DataContract]
    public class CodeHierarchyUnit
    {
        /// <summary>
        /// Инициализирует элемент иерархии кода с указанными параметрами.
        /// </summary>
        /// <param name="clusterId">Идентификатор кластера.</param>
        /// <param name="groupId">Идентификатор группы.</param>
        /// <param name="code">Код.</param>
        public CodeHierarchyUnit(int clusterId, int groupId, long code)
        {
            GroupId = groupId;
            ClusterId = clusterId;
            Code = code;
        }

        /// <summary>
        /// Возвращает идентификатор код-группы текущего элемента иерархии.
        /// </summary>
        [DataMember]
        public int GroupId { get; private set; }
        /// <summary>
        /// Возвращает идентификатор код-кластера текущего элемента иерархии.
        /// </summary>
        [DataMember]
        public int ClusterId { get; private set; }
        /// <summary>
        /// Возвращает код текущего элемента иерархии.
        /// </summary>
        [DataMember]
        public long Code { get; private set; }
        /// <summary>
        /// Возвращает или задаёт строковое представление направления.
        /// </summary>
        [DataMember]
        public string Destination { get; set; }

        public bool IsTemporary
        {
            get { return GroupId == -1 && ClusterId == -1; }
        }
    }
}