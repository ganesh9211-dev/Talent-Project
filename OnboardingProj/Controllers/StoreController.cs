using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnboardingProj.Models;
using Newtonsoft.Json;
using System.Data.Entity.Validation;

namespace OnboardingProj.Controllers
{
    public class StoreController : Controller
    {
        OnboardingDBEntities db = new OnboardingDBEntities();

        // GET: Store
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetStoreList()
        {
            //Pass The All Store Records From Controller To View For Show The All Data For User
            List<StoreViewModel> StoreList = db.Stores.Select(x => new StoreViewModel
            {
                StoreId = x.Id,
                StoreName = x.Name,
                StoreAddress = x.Address,
            }).ToList();

            return Json(StoreList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStoreById(int StoreId)
        {
            Store model = db.Stores.Where(x => x.Id == StoreId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDataInDatabase(StoreViewModel model)
        {
            var result = false;
            try
            {
                if (model.StoreId > 0)
                {
                    Store Sto = db.Stores.SingleOrDefault(x => x.Id == model.StoreId);
                    Sto.Name = model.StoreName;
                    Sto.Address = model.StoreAddress;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    Store Sto = new Store();
                    if (db.Stores.Count() == 0)
                    {
                        Sto.Id = 1;
                    }
                    else
                    {
                        Sto.Id = db.Stores.Max(x => x.Id) + 1;
                    }
                    Sto.Name = model.StoreName;
                    Sto.Address = model.StoreAddress;
                    db.Stores.Add(Sto);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteStoreRecord(int StoreId)
        {
            bool result = false;
            Store Sto = db.Stores.SingleOrDefault(x => x.Id == StoreId);
            if (Sto != null)
            {
                db.Stores.Remove(Sto);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}