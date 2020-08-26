using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnboardingProj.Models
{
    public class StoreViewModel
    {
        [Required]
        public int StoreId { get; set; }

        [Required(ErrorMessage = "Store Name is required")]
        public string StoreName { get; set; }

        [Required(ErrorMessage = "Store Address is required")]
        public string StoreAddress { get; set; }
    }
}