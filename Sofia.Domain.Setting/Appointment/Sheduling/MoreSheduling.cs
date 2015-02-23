using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Setting.Appointment.Sheduling
{
    public class MoreSheduling : Sheduler
    {
         /// <summary>
        /// Инициализирует экземпляр задания, выполняющегося один раз.
        /// </summary>
        public MoreSheduling() : this(false)
        {
        }

        /// <summary>
        /// Инициализирует экземпляр задания указанного характера выполнения.
        /// </summary>
        /// <param name="isPeriodicity">Параметр, определяющий характер выполнения(True - периодический запуск, False - единичный запуск).</param>
        public MoreSheduling(bool isPeriodicity)
            : base("more",isPeriodicity)
        {
        }
    }
}
