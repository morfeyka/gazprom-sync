using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Oracle.DataAccess.Client;
using PSI_API;
using Sofia.Data;
using Sofia.Data.Logic;
using Sofia.Domain.Setting.Appointment.Sheduling;
using Sofia.Domain.Setting.Log;
using Sofia.Scheduling.data;

namespace Sofia.Scheduling.Schedulers
{
    public class Import : SchedulingEntity<Sheduler>
    {

        /// <summary>
        /// Инициализирует экземпляр плана выполнения задания формирования прайс-листов с указанным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор задания.</param>
        public Import(int id)
            : base(id)
        {
        }




        public override void DoWork(object idTaskStatus)
        {
            var session = Db<WebEnvironmentFiact>.NSession;
            var status = session.Get<StatusTask>((int) idTaskStatus);
            var listItems = Db<WebEnvironmentFiact>.Get<Domain.Setting.Training.IdsForTraining>();
            var sbForFile = new StringBuilder(RowFormat.Length*listItems.Count());
            var sbForFileOriginal = new StringBuilder(RowFormat.Length*listItems.Count());

            var item = String.Empty;
            try
            {
                int countValue = 0;

                cls_IPSIAPI api = new PSI_API.cls_IPSIAPIClass();

                var loginInfo = new typLoginInfo
                {
                    strUser = System.Configuration.ConfigurationManager.AppSettings["psiLogin"],
                    strPassword = System.Configuration.ConfigurationManager.AppSettings["psiPassword"],
                    strUserClass = System.Configuration.ConfigurationManager.AppSettings["psiUserClass"],
                    strView = System.Configuration.ConfigurationManager.AppSettings["psidisplay"]
                };
                api.vbLogin(loginInfo);

                ConverterManager mgr = ConverterManager.Instance;
                foreach (var idsForTraining in listItems)
                {

                    if (Entity.TimeGetData.HasValue)
                    {

                    }

                    item = idsForTraining.Id;


                    try
                    {
                        var value = api.vbGetPAValue(idsForTraining.Id);
                        sbForFile.AppendFormat(RowFormat, idsForTraining.Value,
                                               mgr.GetVestaValue(idsForTraining.Id, value.Wert),
                                               DateTime.Now);
                        sbForFileOriginal.AppendFormat(RowFormat, idsForTraining.Value, value.Wert,
                                                       DateTime.Now);

                    }
                    catch (Exception e)
                    {
                        status.Error += "Problem with tag: " + item + Environment.NewLine + e.Message;
                    }

                }

                api.vbLogout();

                session.Refresh(status);
                status = session.Get<StatusTask>(idTaskStatus);

                String dir = Entity.Param + @"\" + string.Format("{0:yyMMdd}", DateTime.Now);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);


                if (!File.Exists(FullPath))
                {
                    using (var streamFile = File.CreateText(FullPath))
                    {
                        streamFile.Write(sbForFile.ToString());
                    }
                    using (var streamFile = File.CreateText(FullPathOriginal))
                    {
                        streamFile.Write(sbForFileOriginal.ToString());
                    }
                    status.TaskExecType = TaskExecType.Succeed;
                }
                else
                {
                    status.TaskExecType = TaskExecType.Failure;
                    status.ErrorRows = 1;
                    status.Error = string.Format("Файл с именем {0} уже существует",
                                                 FullPath);
                }


            }
            catch (Exception e)
            {
                session.Refresh(status);
                status = session.Get<StatusTask>(idTaskStatus);
                status.Error = status.Error + Environment.NewLine + e.ToString();
                status.TaskExecType = TaskExecType.Failure;
            }
            status.EndRun = DateTime.Now;
            session.Save(status);
            session.Flush();
        }

        /// <summary>
        ///Формат записи строки в файл  ИдентификаторОбъекта.Идентификаторпараметра\tЗначениеПараметра\tЧисло\tВремя\n, где \t означает знак табуляции, а \n – перевод строки.
        /// </summary>
        private const string RowFormat = "{0}\t{1}\t{2:dd.MM.yy}\t{2:HH:mm:ss}\n";

        /// <summary>
        /// Полный путь к файлу
        /// </summary>
        private string FullPath
        {
            get { return Entity.Param + @"\" + string.Format("{0:yyMMdd}", DateTime.Now) + @"\" + NameFile; }
        }


        /// <summary>
        /// Возвращает имя файла в который будет производиться запись данных
        /// </summary>
        private string NameFile
        {
            get { return string.Format("IN_OUT_{0:yyMMdd_HHmm}.dat", Entity.TimeGetData ?? DateTime.Now); }
        }

        private string FullPathOriginal
        {
            get { return Entity.Param + @"\" + string.Format("{0:yyMMdd}", DateTime.Now) + @"\" + NameFileOriginal; }
        }


        /// <summary>
        /// Возвращает имя файла в который будет производиться запись данных
        /// </summary>
        private string NameFileOriginal
        {
            get { return string.Format("IN_OUT_{0:yyMMdd_HHmm}.original.dat", Entity.TimeGetData ?? DateTime.Now); }
        }

    }

}