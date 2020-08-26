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
    public class SalesController : Controller
    {
        OnboardingDBEntities db = new OnboardingDBEntities();

        // GET: Sales
        public ActionResult Index()
        {
            List<Customer> CustList = db.Customers.ToList();
            ViewBag.ListOfCustomers = new SelectList(CustList, "Id", "Name");
            
            List<Product> ProdList = db.Products.ToList();
            ViewBag.ListOfProducts = new SelectList(ProdList, "Id", "Name");

            List<Store> StoreList = db.Stores.ToList();
            ViewBag.ListOfStores = new SelectList(StoreList, "Id", "Name");

            return View();
        }

        public JsonResult GetSalesList()
        {
            //Pass The All Sales Records From Controller To View For Show The All Data For User
            List<SalesViewModel> SaleList = db.ProductSolds.Select(x => new SalesViewModel
            {
                SaleId = x.Id,

                SaleCustomerId = x.CustomerId,
                SaleCustomerName = x.Customer.Name,

                SaleProductId = x.ProductId,
                SaleProductName = x.Product.Name,

                SaleStoreId = x.StoreId,
                SaleStoreName = x.Store.Name,

                SaleDateSold = x.DateSold,
            }).ToList();

            return Json(SaleList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSaleById(int SaleId)
        {
            ProductSold model = db.ProductSolds.Where(x => x.Id == SaleId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDataInDatabase(SalesViewModel model)
        {
            var result = false;
            try
            {
                if (model.SaleId > 0)
                {
                    ProductSold PrSd = db.ProductSolds.SingleOrDefault(x => x.Id == model.SaleId);
                    PrSd.ProductId = model.SaleProductId;
                    PrSd.CustomerId = model.SaleCustomerId;
                    PrSd.StoreId = model.SaleStoreId;
                    PrSd.DateSold = model.SaleDateSold;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    ProductSold PrSd = new ProductSold();
                    if (db.ProductSolds.Count() == 0)
                    {
                        PrSd.Id = 1;
                    }
                    else
                    {
                        PrSd.Id = db.ProductSolds.Max(x => x.Id) + 1;
                    }
                    PrSd.ProductId = model.SaleProductId;
                    PrSd.CustomerId = model.SaleCustomerId;
                    PrSd.StoreId = model.SaleStoreId;
                    PrSd.DateSold = model.SaleDateSold;
                    db.ProductSolds.Add(PrSd);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSaleRecord(int SaleId)
        {
            bool result = false;
            ProductSold PrSd = db.ProductSolds.SingleOrDefault(x => x.Id == SaleId);
            if (PrSd != null)
            {
                db.ProductSolds.Remove(PrSd);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}