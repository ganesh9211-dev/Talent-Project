using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnboardingProj.Models
{
    public class CustomerViewModel
    {
        [Required]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        public Nullable<bool> IsDeleted { get; set; }
    }
}