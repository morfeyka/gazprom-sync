using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Sofia.Connect.Proxy;
using Sofia.Contracts;
using Sofia.Data;
using Sofia.Data.Logic;
using Sofia.Domain.Setting.Appointment.Sheduling;
using Sofia.Domain.Setting.Training;
using Sofia.Dto;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Sofia.WebSite.Areas.Admin.Controllers
{
    public class GeneralController : Controller
    {
        //
        // GET: /Admin/General/

        public ActionResult Index()
        {
            
        
            List<SchedulerDto> list = new List<SchedulerDto>();
            try
            {
                using (var factory = new ChannelFactory<IImportAxapta>("MyImportAxapta"))
                {
                    if (factory.State != CommunicationState.Faulted)
                    {
                        IImportAxapta channel = factory.CreateChannel();
                        factory.Faulted += new EventHandler(factory_Faulted);
                        list = channel.GetSummaryInfo();
                        ViewBag.IsService = true;
                    }
                }
            }
            catch(Exception e)
            {
                ViewBag.IsService = false;
            }
            var model =
                Db<WebEnvironmentFiact>.Get<Sheduler>().Select(
                    x =>
                    new SchedulerDto
                        {
                            CountLaunches = x.Iteration,
                            LastDuration = x.Duration ?? 0,
                            LastRun = x.LastRun,
                            NextRun = x.NextRun ?? new DateTime(),
                            Id = x.Id,
                            State = TaskExecutionType.Idle,
                            Name = x.Name,
                            Type = x.Type
                        }).ToList();
            model = model.GroupJoin(list, x => x.Id, y => y.Id, (x, y) => new SchedulerDto
            {
                CountLaunches = x.CountLaunches,
                LastDuration = x.LastDuration ,
                LastRun = y.FirstOrDefault()!=null?y.FirstOrDefault().LastRun:x.LastRun,
                NextRun = y.FirstOrDefault()!=null?y.FirstOrDefault().NextRun:x.NextRun,
                Id = x.Id,
                State = y.FirstOrDefault() != null?y.FirstOrDefault().State:x.State,
                Name = x.Name,
                Type = x.Type
            }).ToList();
            return View(model.AsQueryable());
        }

        [HttpPost]
        public JsonResult KillShedule(int id)
        {
            try
            {
                using (var factory = new ChannelFactory<IImportAxapta>("MyImportAxapta"))
                {
                    if (factory.State != CommunicationState.Faulted)
                    {
                        factory.Faulted += new EventHandler(factory_Faulted);
                        IImportAxapta channel = factory.CreateChannel();
                        channel.KillShedule(id);
                    }
                    else
                        return Json(false, JsonRequestBehavior.AllowGet);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        void factory_Faulted(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        public ActionResult LoadFile()
        {
            return View();
        }

        private static EasyXLS.ExcelDocument doc;
        public ActionResult Save(HttpPostedFileBase attachments)
        {
            if (attachments.ContentLength > 0)
            {
                var fileName = Path.GetFileName(attachments.FileName);
               // var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                //attachments.SaveAs(physicalPath);
                var st = new MemoryStream();
                var buf = new byte[1024];
                var i = 0;
                while ((i = attachments.InputStream.Read(buf, 0, buf.Length)) > 0)
                {
                    st.Write(buf, 0, i);
                }
                st.Position = 0;
                doc = new EasyXLS.ExcelDocument();
                var isLoad = fileName.EndsWith("xlsx", StringComparison.InvariantCultureIgnoreCase) ? doc.easy_LoadXLSXFile(st) : doc.easy_LoadXLSFile(st);
                if(isLoad)
                {
                    var model = new ModelExel {CountSheet = doc.SheetCount()};
                    model.SheetBody = new List<string>(model.CountSheet);
                    model.SheetNames = new List<string>(model.CountSheet);
                    for (int j = 0; j < model.CountSheet; j++)
                    {
                        var st1 = new MemoryStream();
                        var sw1 = new StreamWriter(st1, Encoding.Default);
                        var nameSheet = doc.easy_getSheetAt(j).getSheetName();
                        doc.easy_WriteHTMLFile(sw1, nameSheet);
                        var str = Encoding.Default.GetString(st1.ToArray());
                        model.SheetBody.Add(str);
                        model.SheetNames.Add(nameSheet);
                    }
                    return this.Json(model, int.MaxValue);
                }

            }
            return Content("");
        }
        public ActionResult Remove(string[] fileNames)
        {
            foreach (var fullName in fileNames)
            {
                var fileName = Path.GetFileName(fullName);
                var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }
            return Content("");
        }

        public ActionResult InBase(List<ModelSettingExel> list)
        {
            var result = new List<ModelSettingExel>();
            var nsession = Db<WebEnvironmentFiact>.NSession;
            if(doc!=null)
            {
               var resuslt = nsession.CreateSQLQuery("TRUNCATE TABLE IdsForTraining").UniqueResult();
                foreach (var twoItems in list)
                {
                    var xlsTable1 = ((EasyXLS.ExcelWorksheet)doc.easy_getSheetAt(twoItems.Index-1)).easy_getExcelTable();
                    var key = twoItems.Key.Split(new[] { "R", "C" }, 2, StringSplitOptions.RemoveEmptyEntries).ToArray();
                    var val = twoItems.Val.Split(new[] { "R", "C" }, 2, StringSplitOptions.RemoveEmptyEntries).ToArray();
                    var cellIndex = int.Parse(key[1]) - 1;
                    const short countCell = short.MaxValue;
                    for (int rowIndex = int.Parse(key[0]); rowIndex < countCell; rowIndex++)
                    {
                        var id = xlsTable1.easy_getCell(rowIndex, cellIndex).getValue();
                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            var value = xlsTable1.easy_getCell(rowIndex, int.Parse(val[1]) - 1).getValue();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                var newItem = new ModelSettingExel
                                {
                                    Index = twoItems.Index,
                                    Key = "R" + rowIndex + "C" + cellIndex + ":Key=" + id,
                                    Val = "R" + rowIndex + "C" + cellIndex + ":Val=" + value
                                };
                                var item = nsession.Get<IdsForTraining>(id) ?? new IdsForTraining { Id = id, Value = value };
                                result.Add(newItem);
                                nsession.Save(item);
                            }
                        }
                    }
                    
                }
            }
            nsession.Flush();
            nsession.Close();
            return this.Json(result, int.MaxValue);
        }
    }

    public class ModelSettingExel
    {
        public int Index { get; set; }
        public string Key { get; set; }
        public string Val { get; set; }
    }

    public class ModelExel
    {
        public int CountSheet { get; set; }
        public List<string> SheetNames { get; set; }
        public List<string> SheetBody { get; set; }
    }

    public static class Extension
    {
        /// <summary>
        /// Возвращает данные в формате JSON (с возможностью указать макимальную длину JSON-строки)
        /// </summary>
        /// <param name="controller">Контроллер</param>
        /// <param name="data">Объект, для сериализации</param>
        /// <param name="maxLength">Максимальная длина JSON-строки</param>
        /// <returns></returns>
        public static JsonResultCorrected Json(this Controller controller, object data, int maxLength)
        {
            return new JsonResultCorrected()
            {
                Data = data,
                ContentType = null,
                ContentEncoding = null,
                JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                MaxJsonLength = maxLength
            };
        }
    }
    /// <summary>
    /// JsonResult с возможностью задать макимальную длину JSON-строки
    /// </summary>
    public class JsonResultCorrected : JsonResult
    {
        /// <summary>
        /// Макимальная длина JSON-строки
        /// </summary>
        public int MaxJsonLength { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("JsonRequest_GetNotAllowed");
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;
            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            if (Data == null)
                return;
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer { MaxJsonLength = MaxJsonLength };

            response.Write(scriptSerializer.Serialize(Data));
        }
    }
}
