using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sofia.Data;
using Sofia.Data.Logic;
using Sofia.Domain.Setting.Training;
using Telerik.Web.Mvc;

namespace Sofia.WebSite.Areas.Admin.Controllers
{
    public class TrainingController : Controller
    {
        private IQueryable<IdsForTraining> Source
        {
            get { return Db<WebEnvironmentFiact>.Get<IdsForTraining>(); }
        }
        //
        // GET: /Admin/Training/

        public ActionResult Index()
        {
            return View();
        }
        [GridAction]
        public ActionResult Select()
        {
            return View(new GridModel(Source));
        }
        [GridAction]
        public ActionResult Delete(string id)
        {
            var itemForDelete = Db<WebEnvironmentFiact>.NSession.Get<IdsForTraining>(id);
            if(itemForDelete!=null)
                Db<WebEnvironmentFiact>.NSession.Delete(itemForDelete);
            Db<WebEnvironmentFiact>.NSession.Close();
            return View(new GridModel(Source));
        }
        [GridAction]
        public ActionResult Save(IdsForTraining model)
        {
            var itemForSave = Db<WebEnvironmentFiact>.NSession.Get<IdsForTraining>(model.Id)??model;
            Db<WebEnvironmentFiact>.NSession.Save(itemForSave);
            Db<WebEnvironmentFiact>.NSession.Flush();
            Db<WebEnvironmentFiact>.NSession.Close();
            return View(new GridModel(Source));
        }
    }
}
