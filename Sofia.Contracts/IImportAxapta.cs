using System.Collections.Generic;
using System.ServiceModel;
using Sofia.Dto;

namespace Sofia.Contracts
{
    /// <summary>
    /// ������������ �������� ��������� ������������ �����-������ ��������� �������� ������.
    /// </summary>
    [ServiceContract]
    public interface IImportAxapta
    {
        /// <summary>
        /// ��������� ���������� ������ ������� � ������ ����� ��������.
        /// </summary>
        /// <param name="id"></param>
        [OperationContract(IsOneWay=true)]
        void AddShedule(int id);

        /// <summary>
        /// ���������� ����� ������ �� �������� �������� � ��������� �����.
        /// </summary>
        /// <returns>������ �� ������������ �������� �������� � ���������� �������.</returns>
        [OperationContract]
        List<SchedulerDto> GetSummaryInfo();

        /// <summary>
        /// ��������� �������� ������� �� ������� ����� ��������.
        /// </summary>
        /// <param name="id"></param>
        [OperationContract(IsOneWay = true)]
        void RemoveShedule(int id);
        /// <summary>
        /// ��������� ��������� ���������� ������� ������������� � ������ �����.
        /// </summary>
        /// <param name="id"></param>
        [OperationContract(IsOneWay = true)]
        void KillShedule(int id);
    }
}