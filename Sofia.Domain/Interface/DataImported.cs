using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sofia.Domain.Interface;

namespace Sofia.Domain.Interface
{
    /// <summary>
    /// Предоставляет информацию о данных, которые необходимо импортировать
    /// </summary>
    /// <remarks>table fiaxIntfTable</remarks>
    public abstract class DataImported:EntityWithTypedId<long>
    {
        private TypeData _typeData;

        protected DataImported(TypeData typeData)
        {
            _typeData = typeData;
        }

        protected DataImported() { }
        /// <summary>
        /// Возвращает или задает идентификатор области данных
        /// </summary>
        /// <example>sof</example>
        /// <remarks>field DataAreaId</remarks>
        public virtual string DataAreaId { get; set; }

        /// <summary>
        /// Возвращает или задает тип действия <see cref="Interface.Action"/>
        /// </summary>
        /// <remarks>field Action</remarks>
        public virtual Action Action { get; set; }

        /// <summary>
        /// Возвращает или задает статус данных <see cref="Interface.Status"/>
        /// </summary>
        /// <remarks>field Status</remarks>
        public virtual Status Status { get; set; }
        /// <summary>
        /// Возвращает или задает идентификатор необходимой таблицы
        /// </summary>
        /// <remarks>field reftableid</remarks>
        public virtual TypeData TypeData { get { return _typeData; } }
        /// <summary>
        /// Возвращает или задает идентификатор записи 
        /// </summary>
        /// <remarks>field RefRecId</remarks>
        public virtual long RecId { get; set; }
        /// <summary>
        /// Возвращает или задает дату модификации
        /// </summary>
        /// <remarks>field ModifiedDate CONVERT(DATETIME,'" & Format(dDateTime, "DD.MM.YYYY") & "',104), т.е дата без времени</remarks>
        public virtual DateTime ModifiedDate { get; set; }
        /// <summary>
        /// Возвращает или задает время модификации
        /// </summary>
        /// <remarks>field  modifiedtime CLng(Format(dDateTime, "HH")) * 3600 + CLng(Format(dDateTime, "mm")) * 60 + CLng(Format(dDateTime, "ss"))</remarks>
        public virtual long ModifiedTime { get; set; }
        /// <summary>
        /// Возвращает или задает сообщение о соостоянии, ошибке
        /// </summary>
        /// <remarks>field Message maxLenght = 250, Replace(Mid(sError, 1, 250), "'", "`")</remarks>
        public virtual string Message { get; set; }
        /// <summary>
        /// Возвращает или задает идентификатор пользователя, создателя заказа
        /// </summary>
        /// <remarks>field createdBy, можно получить идентификатор работника goAxapta.CallStaticRecordMethod("EmplTable", "userId2EmplId", CreatedBy)</remarks>
        public virtual string CreatedBy { get; set; }
    }
}
