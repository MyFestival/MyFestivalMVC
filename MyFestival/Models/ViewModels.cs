using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyFestival.Models
{
    public class FestivalTypeVM
    {
        public int FestivalTypeID { get; set; }
        [Display(Name = "New Festival Type:")]
        [Required(ErrorMessage = "Please input the Festival Type.")]
        public string Name { get; set; }
    }

    public class EventTypeVM
    {
        public int EventTypeID { get; set; }
        [Display(Name = "New Event Type:")]
        [Required(ErrorMessage = "Please input the Event Type.")]
        public string Name { get; set; }
    }
}