using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sofia.Data;
using Sofia.Data.Logic;
using Sofia.Domain.Setting.Log;
using Sofia.WebSite.Areas.Admin.Models;

namespace Sofia.WebSite.Areas.Admin.Controllers
{
    public class StatusLogController : Controller
    {
        //
        // GET: /Admin/StatusLog/

        public ActionResult Index(int id)
        {
            var items = Db<WebEnvironmentFiact>.Get<StatusTask>().Where(x => x.Sheduler.Id == id).ToList().Select(x => new StatusTaskModel
            {
                Id = x.Id,
                Duration = x.EndRun.HasValue ? (TimeSpan)(x.EndRun - x.StartRun) : (new TimeSpan(0)),
                EndRun = x.EndRun,
                Error = x.Error,
                ShedulerId = x.Sheduler.Id,
                ShedulerName = x.Sheduler.Name,
                StartRun = x.StartRun,
                TaskExecType = x.TaskExecType,
                TotalRows = x.TotalRows,
                ErrorRows = x.ErrorRows
            }).AsQueryable();
            return View(items);
        }

    }
}
