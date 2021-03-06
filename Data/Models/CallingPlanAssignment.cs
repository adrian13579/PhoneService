using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class CallingPlanAssignment
    {
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Phone Line")]
        public PhoneLine PhoneLine { get; set; }

        [Display(Name = "Calling Plan")]
        public string CallingPlanId { get; set; }
        public CallingPlan CallingPlan { get; set; }

        [Display(Name = "Month")]
        public int Month { get; set; }

        [Display(Name = "Year")]
        public int Year { get; set; }

    }
}