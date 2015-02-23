using System.Collections.Generic;
using System.ServiceModel;

namespace Sofia.Contracts
{
    /// <summary>
    /// ������������ �������� ��������� ��������� �����.
    /// </summary>
    [ServiceContract]
    public interface ICodesLibrary
    {
        /// <summary>
        /// ��������� ���������� ���������.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void Refresh();

        /// <summary>
        /// ��������� ����� ����������� ��� ���������� ����.
        /// </summary>
        /// <param name="code">���.</param>
        /// <returns>��������� ������������� ����������� ��� <see cref="string.Empty"/>, ���� ����� �� ��� �����������.</returns>
        [OperationContract]
        string Find(long code);

        /// <summary>
        /// ��������� ����� ����������� ��� ���������� ������ �� �����.
        /// </summary>
        /// <param name="codes">����� �����.</param>
        /// <returns>������� �� ��� ����-��������, ��� ������ ������ ���, ��������� - ��������� ������������� ����������� ��� ����� ����.</returns>
        [OperationContract]
        Dictionary<long, string> FindFor(IList<long> codes);

    }
}