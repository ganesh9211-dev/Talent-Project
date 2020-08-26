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
    public class CustomerController : Controller
    {
        OnboardingDBEntities db = new OnboardingDBEntities();

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCustomerList()
        {
            //Pass The All Customer Record From Controller To View For Show The All Data For User
            List<CustomerViewModel> CustList = db.Customers.Select(x => new CustomerViewModel
            {
                CustomerId = x.Id,
                CustomerName = x.Name,
                Address = x.Address,
            }).ToList();

            return Json(CustList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerById(int CustomerId)
        {
            Customer model = db.Customers.Where(x => x.Id == CustomerId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDataInDatabase(CustomerViewModel model)
        {
            var result = false;
            try
            {
                if (model.CustomerId > 0)
                {
                    Customer Cust = db.Customers.SingleOrDefault(x => x.Id == model.CustomerId);
                    Cust.Name = model.CustomerName;
                    Cust.Address = model.Address;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    Customer Cust = new Customer();
                    if (db.Customers.Count() == 0)
                    {
                        Cust.Id = 1;
                    }
                    else
                    {
                        Cust.Id = db.Customers.Max(x => x.Id) + 1;
                    }
                    Cust.Name = model.CustomerName;
                    Cust.Address = model.Address;
                    db.Customers.Add(Cust);
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

        public JsonResult DeleteCustomerRecord(int CustomerId)
        {
            bool result = false;
            Customer Cust = db.Customers.SingleOrDefault(x => x.Id == CustomerId);
            if (Cust != null)
            {
                db.Customers.Remove(Cust);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}