using System;
using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using Oracle.DataAccess.Client;
using PSI_API;
using Sofia.Data;
using Sofia.Data.Logic;
using Sofia.Domain.Setting.Appointment.Sheduling;
using Sofia.Domain.Setting.Log;

namespace Sofia.Scheduling.Schedulers
{
    /// <summary>
    ///     Представляет описание задания
    /// </summary>
    public class More : SchedulingEntity<Sheduler>
    {
        /// <summary>
        ///     Формат записи строки в файл  ИдентификаторОбъекта.Идентификаторпараметра\tЗначениеПараметра\tЧисло\tВремя\n, где \t
        ///     означает знак табуляции, а \n – перевод строки.
        /// </summary>
        private const string RowFormat = "{0}\t{1}\t{2:dd.MM.yy}\t{2:HH:mm:ss}\n";

        /// <summary>
        ///     Инициализирует экземпляр плана выполнения задания .
        /// </summary>
        /// <param name="id">Идентификатор задания.</param>
        public More(int id)
            : base(id)
        {
        }

        /// <summary>
        ///     Полный путь к файлу
        /// </summary>
        private string FullPath
        {
            get { return Entity.Param + @"\" + string.Format("{0:yyMMdd}", DateTime.Now) + @"\" + NameFile; }
        }

        /// <summary>
        ///     Возвращает имя файла в который будет производиться запись данных
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
        ///     Возвращает имя файла в который будет производиться запись данных
        /// </summary>
        private string NameFileOriginal
        {
            get { return string.Format("IN_OUT_{0:yyMMdd_HHmm}.original.dat", Entity.TimeGetData ?? DateTime.Now); }
        }

        public override void DoWork(object idTaskStatus)
        {
            var session = Db<WebEnvironmentFiact>.NSession;
            var status = session.Get<StatusTask>((int) idTaskStatus);

            try
            {
                cls_IPSIAPI api = new cls_IPSIAPIClass();
                var loginInfo = new typLoginInfo
                {
                    strUser = ConfigurationManager.AppSettings["psiLogin"],
                    strPassword = ConfigurationManager.AppSettings["psiPassword"],
                    strUserClass = ConfigurationManager.AppSettings["psiUserClass"],
                    strView = ConfigurationManager.AppSettings["psiView"]
                };
                api.vbLogin(loginInfo);
                var timerTag = String.Format("SY.SNMP.{0}.PKOPET...TIMER", ConfigurationManager.AppSettings["lpuKey"]);
                api.vbSetPAValue(ref timerTag, new typeValue {Wert = DateTime.UtcNow.ToOADate()});
                using (var oracleConnection = new OracleConnection())
                {
                    oracleConnection.ConnectionString = ConfigurationManager.AppSettings["oracle"];
                    try
                    {
                        oracleConnection.Open();
                        var tag = String.Format("SY.SNMP.{0}.PKOPET...LINK1", ConfigurationManager.AppSettings["lpuKey"]);
                        api.vbSetPAValue(ref tag, new typeValue {Wert = 1});
                    }
                    catch (Exception e)
                    {
                        var tag = String.Format("SY.SNMP.{0}.PKOPET...LINK1", ConfigurationManager.AppSettings["lpuKey"]);
                        api.vbSetPAValue(ref tag, new typeValue {Wert = 0});
                    }

                    if (oracleConnection.State != ConnectionState.Open)
                        oracleConnection.Open();
                    var oracleCommand = new OracleCommand(@"select object_label,val from BALANS.OPETI_DU_FOR_PSI_LIVE")
                    {
                        Connection = oracleConnection,
                        CommandType = CommandType.Text
                    };

                    var reader = oracleCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            if (!reader.IsDBNull(1))
                            {
                                var val = new typeValue {Wert = reader.GetDouble(1)};
                                api.vbSetPAValue(reader.GetString(0), val);
                            }
                        }
                        catch (COMException e)
                        {
                        }
                    }
                    reader.Close();
                }

                api.vbLogout();

                session.Refresh(status);
                status = session.Get<StatusTask>(idTaskStatus);
            }
            catch (Exception e)
            {
                session.Refresh(status);
                status = session.Get<StatusTask>(idTaskStatus);
                status.Error = e.ToString();
                status.TaskExecType = TaskExecType.Failure;

                cls_IPSIAPI api = new cls_IPSIAPIClass();
                var loginInfo = new typLoginInfo
                {
                    strUser = ConfigurationManager.AppSettings["psiLogin"],
                    strPassword = ConfigurationManager.AppSettings["psiPassword"],
                    strUserClass = ConfigurationManager.AppSettings["psiUserClass"],
                    strView = ConfigurationManager.AppSettings["psiView"]
                };
                api.vbLogin(loginInfo);
                var errorTag = String.Format("SY.SNMP.{0}.PKOPET...ERROR", ConfigurationManager.AppSettings["lpuKey"]);
                api.vbSetPAValue(ref errorTag, new typeValue {Wert = 1});
                api.vbLogout();
            }
            status.EndRun = DateTime.Now;
            session.Save(status);
            session.Flush();

            cls_IPSIAPI globalApi = new cls_IPSIAPIClass();
            var globalLoginInfo = new typLoginInfo
            {
                strUser = ConfigurationManager.AppSettings["psiLogin"],
                strPassword = ConfigurationManager.AppSettings["psiPassword"],
                strUserClass = ConfigurationManager.AppSettings["psiUserClass"],
                strView = ConfigurationManager.AppSettings["psiView"]
            };
            globalApi.vbLogin(globalLoginInfo);
            var globalErrorTag = String.Format("SY.SNMP.{0}.PKOPET...ERROR", ConfigurationManager.AppSettings["lpuKey"]);
            globalApi.vbSetPAValue(ref globalErrorTag, new typeValue {Wert = 0});
            globalApi.vbLogout();
        }
    }
}