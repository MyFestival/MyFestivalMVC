using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyFestival.Models
{
    public class EventsVM
    {
        #region Event Type

        [Required]
        [Display(Name = "Event Type")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public Dictionary<int, string> eType { get; set; }

        public int selectedEType { get; set; }

        #endregion

        [Required]
        public int ID { get; set; }

        [Required]
        public int festivalID { get; set; }

        [Required]
        [Display(Name = "Event Name"), StringLength(100)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string EventsName { get; set; }

        [Required]
        [Display(Name = "Event Date")/*, DataType(DataType.Date)*/]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime eventsDate { get; set; }

        [Display(Name = "Start Time"), DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        [Required(ErrorMessage = "Please input the Event's Start Time.")]
        public DateTime startTime { get; set; }

        [Display(Name = "End Time"), DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        [Required(ErrorMessage = "Please input the Event's end time.")]
        public DateTime endTime { get; set; }

        [Required]
        [Display(Name = "Location")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string Location { get; set; }

        public HttpPostedFileWrapper imageFile { get; set; }
        [Display(Name="Event Image")]
        public string eventsImage { get; set; }
    }
}