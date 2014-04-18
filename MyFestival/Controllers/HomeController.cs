using MyFestival.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace MyFestival.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "What do we have to offer you?";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Festival(string county, string searchString, string currentFilter, int? page)
        {
            MyFestivalDb db = new MyFestivalDb();

            var festivals = from f in db.Festivals
                            orderby f.StartDate
                            where f.StartDate >= System.DateTime.Now
                            select f;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!String.IsNullOrEmpty(county))
            {
                festivals = festivals.Where(
                    f => f.FestivalCounty.Name.ToUpper().Contains(county.ToUpper()) /*|| f => f.FestivalName.ToUpper().Contains(searchString.ToUpper())*/
                );
            }
            
            if (!String.IsNullOrEmpty(searchString))
            {
                festivals = festivals.Where(
                    f => f.FestivalName.ToUpper().Contains(searchString.ToUpper()) /*|| f => f.FestivalName.ToUpper().Contains(searchString.ToUpper())*/
                );
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            //return View(festivals.OrderBy(festival => festival.StartDate).ToList());
            return View(festivals.OrderBy(festival => festival.StartDate).ToPagedList(pageNumber, pageSize));

        }

        public ActionResult Details(int id)
        {
            MyFestivalDb db = new MyFestivalDb();

            Festival festival = db.Festivals.Find(id);

            if (festival == null)
            {
                return HttpNotFound();
            }
            else
            {
                festival.Events = (from e in db.Events
                                   where e.FestivalID.Equals(id)
                                   select e).ToList();
            }
            return View(festival);
        }

        public ActionResult Event(int id)
        {
            MyFestivalDb db = new MyFestivalDb();

            Events events = db.Events.Find(id);

            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }
    }
}
