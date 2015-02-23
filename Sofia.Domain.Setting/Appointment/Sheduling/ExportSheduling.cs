
namespace Sofia.Domain.Setting.Appointment.Sheduling
{
    public class ExportSheduling : Sheduler
    {
         /// <summary>
        /// Инициализирует экземпляр задания, выполняющегося один раз.
        /// </summary>
        public ExportSheduling() : this(false)
        {
        }

        /// <summary>
        /// Инициализирует экземпляр задания указанного характера выполнения.
        /// </summary>
        /// <param name="isPeriodicity">Параметр, определяющий характер выполнения(True - периодический запуск, False - единичный запуск).</param>
        public ExportSheduling(bool isPeriodicity)
            : base("export", isPeriodicity)
        {
        }
    }
}
