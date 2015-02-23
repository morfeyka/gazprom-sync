namespace Sofia.Domain.Setting.Appointment.Sheduling
{
    public class ImportSheduling:Sheduler
    {
         /// <summary>
        /// Инициализирует экземпляр задания, выполняющегося один раз.
        /// </summary>
        public ImportSheduling() : this(false)
        {
        }

        /// <summary>
        /// Инициализирует экземпляр задания указанного характера выполнения.
        /// </summary>
        /// <param name="isPeriodicity">Параметр, определяющий характер выполнения(True - периодический запуск, False - единичный запуск).</param>
        public ImportSheduling(bool isPeriodicity)
            : base("import", isPeriodicity)
        {
        }

    }
}
