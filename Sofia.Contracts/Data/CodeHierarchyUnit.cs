using System.Runtime.Serialization;

namespace Sofia.Contracts.Data
{
    /// <summary>
    /// ������������ �������� �������� �������� ����.
    /// </summary>
    [DataContract]
    public class CodeHierarchyUnit
    {
        /// <summary>
        /// �������������� ������� �������� ���� � ���������� �����������.
        /// </summary>
        /// <param name="clusterId">������������� ��������.</param>
        /// <param name="groupId">������������� ������.</param>
        /// <param name="code">���.</param>
        public CodeHierarchyUnit(int clusterId, int groupId, long code)
        {
            GroupId = groupId;
            ClusterId = clusterId;
            Code = code;
        }

        /// <summary>
        /// ���������� ������������� ���-������ �������� �������� ��������.
        /// </summary>
        [DataMember]
        public int GroupId { get; private set; }
        /// <summary>
        /// ���������� ������������� ���-�������� �������� �������� ��������.
        /// </summary>
        [DataMember]
        public int ClusterId { get; private set; }
        /// <summary>
        /// ���������� ��� �������� �������� ��������.
        /// </summary>
        [DataMember]
        public long Code { get; private set; }
        /// <summary>
        /// ���������� ��� ����� ��������� ������������� �����������.
        /// </summary>
        [DataMember]
        public string Destination { get; set; }

        public bool IsTemporary
        {
            get { return GroupId == -1 && ClusterId == -1; }
        }
    }
}