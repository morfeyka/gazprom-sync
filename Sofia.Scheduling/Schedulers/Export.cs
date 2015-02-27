using System;
using System.Data.OleDb;
using Npgsql;
using System.Configuration;
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

                cls_IPSIAPI api = new PSI_API.cls_IPSIAPIClass();
                var loginInfo = new typLoginInfo
                {
                    strUser = ConfigurationManager.AppSettings["psiLogin"],
                    strPassword = ConfigurationManager.AppSettings["psiPassword"],
                    strUserClass = ConfigurationManager.AppSettings["psiUserClass"],
                    strView = ConfigurationManager.AppSettings["psiView"]
                };
                api.vbLogin(loginInfo);


                using (OleDbConnection excelConnection = new OleDbConnection(String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=Excel 12.0;", ConfigurationManager.AppSettings["eitpConfigFile"])))
                {
                 
                    excelConnection.Open();

                    using (OleDbCommand excelCommand = new OleDbCommand(String.Format("select * from [{0}$]",ConfigurationManager.AppSettings["configSheetName"]),excelConnection))
                    {
                        using (OleDbDataReader excelReader=excelCommand.ExecuteReader())
                        {
                            while (excelReader.Read())
                            {
                                String pbTag = excelReader.GetString(5) +
                                               ConfigurationManager.AppSettings["pbTagSuffix"];
                                String pTag = excelReader.GetString(5) + ConfigurationManager.AppSettings["ptagSuffix"];
                                String qTag = excelReader.GetString(5) + ConfigurationManager.AppSettings["qTagSuffix"];
                                String tTag = excelReader.GetString(5) + ConfigurationManager.AppSettings["tTagSuffix"];

                                String internalTag = excelReader.GetString(1);
                                if (String.IsNullOrEmpty(internalTag)) continue;


                                using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["eitp"].ConnectionString))
                                {
                                    String sql = String.Format(ConfigurationManager.AppSettings["eitpRTsql"],
                                        internalTag);
                                    using (NpgsqlCommand command=new NpgsqlCommand(sql,connection))
                                    {
                                        connection.Open();
                                        using (NpgsqlDataReader reader=command.ExecuteReader())
                                        {
                                            if (reader.HasRows)
                                            {
                                                reader.Read();
                                                if (reader.GetValue(1) != DBNull.Value)
                                                {
                                                    api.vbSetPAValue(pbTag,new typeValue{Wert = reader.GetDouble(1),Zeit = reader.GetDateTime(0)});
                                                }
                                                if (reader.GetValue(2) != DBNull.Value)
                                                {
                                                    api.vbSetPAValue(pTag, new typeValue { Wert = reader.GetDouble(2), Zeit = reader.GetDateTime(0) });
                                                }
                                                if (reader.GetValue(3) != DBNull.Value)
                                                {
                                                    api.vbSetPAValue(qTag, new typeValue { Wert = reader.GetDouble(3), Zeit = reader.GetDateTime(0) });
                                                }
                                                if (reader.GetValue(4) != DBNull.Value)
                                                {
                                                    api.vbSetPAValue(tTag, new typeValue { Wert = reader.GetDouble(4), Zeit = reader.GetDateTime(0) });
                                                }

                                            }
                                        }                
                                    }
                                }
                            }
                        }
                    }
                }
                api.vbLogout();

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