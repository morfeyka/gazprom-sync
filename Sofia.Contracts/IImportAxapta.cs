using System.Collections.Generic;
using System.ServiceModel;
using Sofia.Dto;

namespace Sofia.Contracts
{
    /// <summary>
    /// ѕредставл€ет описание контракта формировани€ прайс-листов отпускных тарифных планов.
    /// </summary>
    [ServiceContract]
    public interface IImportAxapta
    {
        /// <summary>
        /// ¬ыполн€ет добавление нового задани€ в список задач шедулера.
        /// </summary>
        /// <param name="id"></param>
        [OperationContract(IsOneWay=true)]
        void AddShedule(int id);

        /// <summary>
        /// ¬озвращает набор данных об активных задани€х в насто€щее врем€.
        /// </summary>
        /// <returns>—писок из транспортных объектов сведений о выполнении заданий.</returns>
        [OperationContract]
        List<SchedulerDto> GetSummaryInfo();

        /// <summary>
        /// ¬ыполн€ет удаление задани€ из списока задач шедулера.
        /// </summary>
        /// <param name="id"></param>
        [OperationContract(IsOneWay = true)]
        void RemoveShedule(int id);
        /// <summary>
        /// ¬ыполн€ет остановку выполнени€ задани€ содержащегос€ в списке задач.
        /// </summary>
        /// <param name="id"></param>
        [OperationContract(IsOneWay = true)]
        void KillShedule(int id);
    }
}