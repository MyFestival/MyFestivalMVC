using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Spatial;
using System.Linq;
using System.Web;

namespace MyFestival.Models
{

    #region Festival

    public class Festival
    {
        [Key]
        public int FestivalId { get; set; }

        [Required]
        [Display(Name = "Festival Name"), StringLength(100)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string FestivalName { get; set; }

        [Required]
        [Display(Name = "Start Date")/*, DataType(DataType.Date)*/]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")/*, DataType(DataType.Date)*/]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        /*[Display(Name = "Festival Location")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public DbGeography Location { get; set; }*/

        [Required]
        [Display(Name = "County")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public virtual County FestivalCounty { get; set; }

        [Required]
        [Display(Name = "Town")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public virtual Town FestivalTown { get; set; }

        [Required]
        [Display(Name = "Festival Type")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public virtual FestivalType FType { get; set; }

        [Display(Name = "Festival Logo")]
        [DataType(DataType.Upload)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string FestivalLogo { get; set; }

        [Display(Name = "Description"), StringLength(200)]
        public string Description { get; set; }

        public ICollection<Events> Events { get; set; }
        public IEnumerable<Events> EventsOrdered
        {
            get { return Events.OrderBy(e => e.EventsDate); }
        }

        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual UserProfile User { get; set; }

    }

    #endregion

    #region Events

    public class Events
    {
        [Required]
        public int ID { get; set; }
        
        [Required]
        public int FestivalID { get; set; }

        [Required(ErrorMessage="Please input the Event Name.")]
        [Display(Name = "Event name"), StringLength(100)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string EventsName { get; set; }

        [Display(Name = "Event date")/*, DataType(DataType.Date)*/]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please input the Event's Date.")]
        public DateTime EventsDate { get; set; }

        [Display(Name = "Start Time"), DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        [Required(ErrorMessage = "Please input the Event's Start Time.")]
        public DateTime StartTime { get; set; }

        [Display(Name = "End Time"), DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        [Required(ErrorMessage = "Please input the Event's end time.")]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Location")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Event Type")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public virtual EventType EType { get; set; }

        [Display(Name = "Event Logo")]
        [DataType(DataType.Upload)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string EventLogo { get; set; }
    }

    #endregion

    #region County

    public class County
    {
        public int ID { get; set; }
        //[Required]
        [Display(Name = "County")]
        public string Name { get; set; }
    }

    #endregion

    #region Town

    public class Town
    {
        public int ID { get; set; }
        //[Display(Name = "Town")]
        public string Name { get; set; }
    }

    #endregion

    #region Festival Type

    public class FestivalType
    {
        public int ID { get; set; }
        [Display(Name = "Festival Type")]
        public string FType { get; set; }
    }

    #endregion

    #region EventType

    public class EventType
    {
        public int ID { get; set; }
        [Display(Name = "Event Type")]
        public string EType { get; set; }
    }

    #endregion

}