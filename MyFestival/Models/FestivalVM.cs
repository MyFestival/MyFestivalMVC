using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace MyFestival.Models
{
    public class FestivalVM
    {
        public int FestivalID { get; set; }

        [Required]
        [Display(Name = "Town")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public Dictionary<int, string> Towns { get; set; }

        [Required]
        [Display(Name = "County")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public Dictionary<int, string> County { get; set; }

        [Required]
        [Display(Name = "Festival Type")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public Dictionary<int, string> FestivalType { get; set; }

        public int SelectedTown { get; set; }
        public int SelectedCounty { get; set; }
        public int SelectedFestivalType { get; set; }

        [Required]
        [Display(Name = "Festival Name"), StringLength(100)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string FestivalName { get; set; }

        [Required]
        [Display(Name = "Start Date")/*, DataType(DataType.Date)*/]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime startDate { get; set; }

        [Required]
        [Display(Name = "End Date")/*, DataType(DataType.Date)*/]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime endDate { get; set; }

        public HttpPostedFileWrapper imageFile { get; set; }

        [Display(Name = "Festival Logo")]
        public string festivalLogo { get; set; }

        public UserProfile UserID { get; set; }
        [Display(Name = "Description"), StringLength(200)]
        public string sDescription { get; set; }

        [Required]
        [Display(Name = "Location")]
        public DbGeography Location { get; set; }
    }
}