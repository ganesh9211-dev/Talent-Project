using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnboardingProj.Models
{
    public class SalesViewModel
    {
        public int SaleId { get; set; }

        public int SaleProductId { get; set; }
        public string SaleProductName { get; set; }

        public int SaleCustomerId { get; set; }
        public string SaleCustomerName { get; set; }

        public int SaleStoreId { get; set; }
        public string SaleStoreName { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public String SaleDateSold { get; set; }
    }
}