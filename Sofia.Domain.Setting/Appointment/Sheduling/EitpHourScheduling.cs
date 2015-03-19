namespace Sofia.Domain.Setting.Appointment.Sheduling
{
    public class EitpHourScheduling : Sheduler
    {
         public EitpHourScheduling():this(false)
        {
            
        }

         public EitpHourScheduling(bool isPeriodicity)
            : base("eitp_hour",isPeriodicity)
        {
        }
    }
}