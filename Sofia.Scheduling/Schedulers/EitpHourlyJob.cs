using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Npgsql;
using PSI_API;
using Sofia.Data;
using Sofia.Data.Logic;
using Sofia.Domain.Setting.Appointment.Sheduling;
using Sofia.Domain.Setting.Log;
using log4net;
using log4net.Config;

namespace Sofia.Scheduling.Schedulers
{
    public class EitpHourlyJob : SchedulingEntity<Sheduler>
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(EitpHourlyJob));

        public EitpHourlyJob(int id) : base(id)
        {
            XmlConfigurator.Configure(new System.IO.FileInfo("log4net.Config"));

        }

        public override void DoWork(object idTaskStatus)
        {
            log.Debug("Stating job execution");
            var session = Db<WebEnvironmentFiact>.NSession;
            var status = session.Get<StatusTask>((int)idTaskStatus);
            try
            {

                log.Debug("Setting up PSI API");
                cls_IPSIAPI api = new PSI_API.cls_IPSIAPIClass();
                var loginInfo = new typLoginInfo
                {
                    strUser = ConfigurationManager.AppSettings["psiLogin"],
                    strPassword = ConfigurationManager.AppSettings["psiPassword"],
                    strUserClass = ConfigurationManager.AppSettings["psiUserClass"],
                    strView = ConfigurationManager.AppSettings["psiView"]
                };
                api.vbLogin(loginInfo);
                log.DebugFormat("Working with API Server: {0}",api.GetActiveServer);
                log.DebugFormat("PSI API Session id:{0}",api.GetSessionID);

                using (OleDbConnection excelConnection = new OleDbConnection(String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=Excel 12.0;", ConfigurationManager.AppSettings["eitpConfigFile"])))
                {

                    log.DebugFormat("Opening excel configuration file:{0}",ConfigurationManager.AppSettings["configSheetName"]);
                    excelConnection.Open();


                    using (OleDbCommand excelCommand = new OleDbCommand(String.Format("select * from [{0}$]", ConfigurationManager.AppSettings["configSheetName"]), excelConnection))
                    {
                        log.DebugFormat("Querying excel with Query: {0}",String.Format("select * from [{0}$]", ConfigurationManager.AppSettings["configSheetName"]));
                        using (OleDbDataReader excelReader = excelCommand.ExecuteReader())
                        {
                            log.DebugFormat("Result of execution, extracted {0} fields and {1} records from excel file",excelReader.FieldCount,excelReader.RecordsAffected);
                            while (excelReader.Read())
                            {
                                log.Debug("Trying fetch excel record");
                                if (excelReader.GetValue(5) == DBNull.Value)
                                {
                                    log.Error("REcord had empty value in excel cell, trying to precess next one...");
                                    continue;
                                }
                                String pbTag = excelReader.GetString(5) +
                                               ConfigurationManager.AppSettings["pbTagSuffix"];
                                String pTag = excelReader.GetString(5) + ConfigurationManager.AppSettings["ptagSuffix"];
                                String qTag = excelReader.GetString(5) + ConfigurationManager.AppSettings["qTagSuffix"];
                                String tTag = excelReader.GetString(5) + ConfigurationManager.AppSettings["tTagSuffix"];

                                String internalTag = excelReader.GetString(1);
                                log.Debug("Excel cell value found.");
                                log.Debug("Generating tags:");
                                log.DebugFormat("   PB Tag:{0}",pbTag);
                                log.DebugFormat("   P Tag :{0}",pTag);
                                log.DebugFormat("   T Tag  :{0}",tTag);
                                log.DebugFormat("Integral EITP Tag:{0}",internalTag);

                                if (String.IsNullOrEmpty(internalTag))
                                {
                                    log.Error("EITP Tag is empty going to the next record.");
                                    continue;
                                }

                                log.Debug("Opening Postgres connection");

                                using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["eitp"].ConnectionString))
                                {

                                    String sql = String.Format(ConfigurationManager.AppSettings["eitpHOURsql"],
                                        internalTag);
                                    log.DebugFormat("Extracting values with query: {0}",sql);
                                    try
                                    {
                                        using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                                        {
                                      
                                             connection.Open();

                                              
                                           
                                            using (NpgsqlDataReader reader = command.ExecuteReader())
                                            {
                                                log.DebugFormat("Query returned {0} records",reader.RecordsAffected);
                                                while (reader.Read())
                                              
                                                {
                                                    
                                                    if (reader.GetValue(1) != DBNull.Value)
                                                    {
                                                        api.vbSetStdValue(pbTag,"H",
                                                            new typeValue
                                                            {
                                                                Wert = reader.GetDouble(1),
                                                                Zeit = reader.GetDateTime(0)
                                                            });
                                                        log.DebugFormat("Setting value:{0} for tag:{1}, and time {2}",reader.GetDouble(1),pbTag,reader.GetDateTime(0));
                                                    }
                                                    if (reader.GetValue(2) != DBNull.Value)
                                                    {
                                                        api.vbSetStdValue(pTag, "H",
                                                            new typeValue
                                                            {
                                                                Wert = reader.GetDouble(2),
                                                                Zeit = reader.GetDateTime(0)
                                                            });
                                                        log.DebugFormat("Setting value:{0} for tag:{1}, and time {2}", reader.GetDouble(1), pTag, reader.GetDateTime(0));
                                                    }
                                                    if (reader.GetValue(3) != DBNull.Value)
                                                    {
                                                        api.vbSetStdValue(tTag, "H",
                                                            new typeValue
                                                            {
                                                                Wert = reader.GetDouble(3),
                                                                Zeit = reader.GetDateTime(0)
                                                            });
                                                        log.DebugFormat("Setting value:{0} for tag:{1}, and time {2}", reader.GetDouble(1), tTag, reader.GetDateTime(0));
                                                    }
                                                  

                                                }
                                            }
                                        }
                                    }
                                    catch (COMException ex)
                                    {
                                        log.ErrorFormat("COM Error occurred while setting values: {0}",ex.Message);
                                    }
                                }
                            }
                        }
                    }
                }
                log.Debug("Logout from PSI");
                log.Debug("Job completed");
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
