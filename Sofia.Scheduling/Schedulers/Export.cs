using System;
using System.Threading;
using PSI_API;
using Sofia.Data;
using Sofia.Data.Logic;
using Sofia.Domain.Setting.Appointment.Sheduling;
using Sofia.Domain.Setting.Log;

namespace Sofia.Scheduling.Schedulers
{
    ///<summary>
    ///Представляет описание задания формирования прайс-листов.
    ///</summary>
    public class Export : SchedulingEntity<Sheduler>
    {
        /// <summary>
        /// Инициализирует экземпляр плана выполнения задания формирования прайс-листов с указанным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор задания.</param>
        public Export(int id)
            : base(id)
        {
        }


        public override void DoWork(object idTaskStatus)
        {
            var session = Db<WebEnvironmentFiact>.NSession;
            var status = session.Get<StatusTask>((int)idTaskStatus);
            try
            {
                try
                {
                    cls_IPSIAPI api = new PSI_API.cls_IPSIAPIClass();
                    var loginInfo = new typLoginInfo
                    {
                        strUser = System.Configuration.ConfigurationManager.AppSettings["psiLogin"],
                        strPassword = System.Configuration.ConfigurationManager.AppSettings["psiPassword"],
                        strUserClass = System.Configuration.ConfigurationManager.AppSettings["psiUserClass"],
                        strView = System.Configuration.ConfigurationManager.AppSettings["psidisplay"]
                    };
                    api.vbLogin(loginInfo);
                    String timerTag = String.Format(@"SY.SNMP.{0}.PKOPET...TIMER",System.Configuration.ConfigurationManager.AppSettings["lpuKey"]);
                    api.vbSetPAValue(ref timerTag, new typeValue { Wert = 1 });
                } catch(Exception) {}
                




                
                session.Evict(status);
                status = session.Get<StatusTask>(idTaskStatus);
                status.TaskExecType = TaskExecType.Succeed;
            }
            catch (Exception e)
            {
                session.Evict(status);
                status = session.Get<StatusTask>(idTaskStatus);
                status.Error = e.ToString();
                status.TaskExecType = TaskExecType.Failure;
            }

            status.EndRun = DateTime.Now;
            session.Save(status);
            session.Flush();
        }
    }
}