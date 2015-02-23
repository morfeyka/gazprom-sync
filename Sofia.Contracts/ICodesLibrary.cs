using System.Collections.Generic;
using System.ServiceModel;

namespace Sofia.Contracts
{
    /// <summary>
    /// Представляет описание контракта хранилища кодов.
    /// </summary>
    [ServiceContract]
    public interface ICodesLibrary
    {
        /// <summary>
        /// Выполняет обновление хранилища.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void Refresh();

        /// <summary>
        /// Выполняет поиск направления для указанного кода.
        /// </summary>
        /// <param name="code">Код.</param>
        /// <returns>Строковое представление направления или <see cref="string.Empty"/>, если поиск не дал результатов.</returns>
        [OperationContract]
        string Find(long code);

        /// <summary>
        /// Выполняет поиск направления для указанного набора из кодов.
        /// </summary>
        /// <param name="codes">Набор кодов.</param>
        /// <returns>Словарь из пар ключ-значение, где ключом служит код, значением - строковое представление направления для этого кода.</returns>
        [OperationContract]
        Dictionary<long, string> FindFor(IList<long> codes);

    }
}