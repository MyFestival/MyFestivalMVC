using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using MyFestival.Models;
using MyFestival.Filters;
using PagedList;
using WebMatrix.WebData;

namespace MyFestival.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class FestivalController : Controller
    {
        private MyFestivalDb db = new MyFestivalDb();

        #region Festival/Index

        //
        // GET: /Festival/

        public ActionResult Index(string sortOrder, string searchString, int? page)
        {
            //return View(db.Festivals.ToList());
            int CurrentID = WebSecurity.CurrentUserId;
            List<Festival> TempStore = db.Festivals.Where(a => a.UserID == CurrentID).ToList();

            if (TempStore.Count != 0)
            {
                /*List<Festival> festivals = (from f in db.Festivals
                                            where f.UserID == WebSecurity.CurrentUserId
                                            select f).ToList();*/
                if (searchString != null)
                {
                    page = 1;
                }

                int pageSize = 6;
                int pageNumber = (page ?? 1);

                return View(TempStore.OrderBy(festival => festival.StartDate).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Create2", "Festival");
            }
        }

        #endregion

        #region Festival/Details

        //
        // GET: /Festival/Details/5

        public ActionResult Details(int id, DataTable tbl)
        {
            int CurrentID = WebSecurity.CurrentUserId;
            Festival festival = db.Festivals.Find(id);

            if (festival == null)
            {
                return HttpNotFound();
            }
            else
            {
                festival.Events = (from e in db.Events
                                   where e.FestivalID.Equals(id) && festival.UserID == CurrentID
                                   select e).ToList();
            }
            return View(festival);
        }

        #endregion

        #region Create ViewModel

        [HttpGet]
        public ActionResult Create2()
        {
            FestivalVM ViewModel = new FestivalVM();

            ViewModel.County = db.Counties.ToDictionary(p => p.ID, q => q.Name);

            ViewModel.FestivalType = db.FestivalTypes.ToDictionary(p => p.ID, q => q.FType);
            ViewModel.FestivalType.Add(-1, "--- Add New Festival Type ---");

            ViewModel.Towns = db.Towns.ToDictionary(p => p.ID, q => q.Name);
            ViewModel.startDate = DateTime.Now;
            ViewModel.endDate = DateTime.Now;

            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2(FestivalVM model)
        {
            if (ModelState.IsValid != true)
            {
                if (model.SelectedFestivalType != -1)
                {
                    //db.save stuff from create.
                    Festival Newfestival = new Festival();
                    Newfestival.EndDate = model.endDate.Date;
                    Newfestival.FestivalCounty = db.Counties.Where(p => p.ID == model.SelectedCounty).Single();
                    Newfestival.FestivalName = model.FestivalName;
                    Newfestival.Description = model.sDescription;
                    Newfestival.FType = db.FestivalTypes.Where(p => p.ID == model.SelectedFestivalType).Single();
                    Newfestival.StartDate = model.startDate.Date;
                    Newfestival.Location = model.Location;
                    Newfestival.FestivalTown = db.Towns.Where(p => p.ID == model.SelectedTown).Single();
                    Newfestival.UserID = WebSecurity.CurrentUserId;

                    if (Request.Files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        string serverPath = Server.MapPath("~\\Content\\FestivalLogo");
                        Bitmap newImage = new Bitmap(Request.Files[0].InputStream);
                        newImage.Save(serverPath + "\\" + fileName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                        model.festivalLogo = "Content/FestivalLogo/" + fileName + ".jpg";

                        db.Festivals.Add(Newfestival);
                        db.SaveChanges();
                        return RedirectToAction("Details", new {id = Newfestival.FestivalId});
                    }
                    else
                    {
                        db.Festivals.Add(Newfestival);
                        db.SaveChanges();
                        return RedirectToAction("Details", new { id = Newfestival.FestivalId });   
                    }
                }
                ModelState.AddModelError("", "No Festival Type Picked");
            }
            model.County = db.Counties.ToDictionary(p => p.ID, q => q.Name);
            model.FestivalType = db.FestivalTypes.ToDictionary(p => p.ID, q => q.FType);
            model.FestivalType.Add(-1, "--- Add New Festival Type ---");
            model.Towns = db.Towns.ToDictionary(p => p.ID, q => q.Name);
            model.startDate = DateTime.Now;
            model.endDate = DateTime.Now;
            return View(model);
        }

        #endregion

        #region Edit ViewModel

        [HttpGet]
        public ActionResult Edit2(int? id)
        {
            Festival fest = db.Festivals.Single(x => x.FestivalId == id);

            if (fest == null)
            {
                return RedirectToAction("Create2");
            }
            else
            {
                FestivalVM festival = new FestivalVM
                {
                    FestivalID = fest.FestivalId,
                    SelectedCounty = fest.FestivalCounty.ID,
                    endDate = fest.EndDate,
                    sDescription = fest.Description,
                    FestivalName = fest.FestivalName,
                    SelectedFestivalType = fest.FType.ID,
                    startDate = fest.StartDate,
                    Location = fest.Location,
                    SelectedTown = fest.FestivalTown.ID
                };

                festival.County = db.Counties.ToDictionary(p => p.ID, q => q.Name);
                festival.FestivalType = db.FestivalTypes.ToDictionary(p => p.ID, q => q.FType);
                festival.FestivalType.Add(-1, "----- Add New -----");
                festival.Towns = db.Towns.ToDictionary(p => p.ID, q => q.Name);
                return View(festival);
            }
        }

        [HttpPost]
        public ActionResult Edit2(FestivalVM model)
        {
            if (ModelState.IsValid != true)
            {
                if (model.SelectedFestivalType != -1)
                {
                    if (Request.Files.Count != 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        string serverPath = Server.MapPath("~\\Content\\Images\\Uploaded");
                        Bitmap newImage = new Bitmap(Request.Files[0].InputStream);
                        newImage.Save(serverPath + "\\" + fileName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                        model.festivalLogo = "/Content/Images/Uploaded/" + fileName + ".jpg";

                        Festival fest = db.Festivals.Single(x => x.FestivalId == model.FestivalID);

                        fest.FestivalId = model.FestivalID;
                        fest.EndDate = model.endDate.Date;
                        fest.FestivalCounty = db.Counties.SingleOrDefault(p => p.ID == model.SelectedCounty);
                        fest.FestivalName = model.FestivalName;
                        fest.Description = model.sDescription;
                        fest.FType = db.FestivalTypes.SingleOrDefault(p => p.ID == model.SelectedFestivalType);
                        fest.StartDate = model.startDate.Date;
                        fest.Location = model.Location;
                        fest.FestivalTown = db.Towns.SingleOrDefault(p => p.ID == model.SelectedTown);
                        fest.UserID = WebSecurity.CurrentUserId;

                        db.Entry(fest).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index", "Festival");
                    }
                    else
                    {
                        Festival fest = db.Festivals.Single(x => x.FestivalId == model.FestivalID);

                        fest.FestivalId = model.FestivalID;
                        fest.EndDate = model.endDate.Date;
                        fest.FestivalCounty = db.Counties.SingleOrDefault(p => p.ID == model.SelectedCounty);
                        fest.FestivalName = model.FestivalName;
                        fest.Description = model.sDescription;
                        fest.FType = db.FestivalTypes.SingleOrDefault(p => p.ID == model.SelectedFestivalType);
                        fest.StartDate = model.startDate.Date;
                        fest.Location = model.Location;
                        fest.FestivalTown = db.Towns.SingleOrDefault(p => p.ID == model.SelectedTown);
                        fest.UserID = WebSecurity.CurrentUserId;
                        
                        //                        Festival f = new Festival
                        //{
                        //    FestivalId = model.FestivalID,
                        //    EndDate = model.endDate.Date,
                        //    FestivalCounty = db.Counties.Where(p => p.ID == model.SelectedCounty).Single(),
                        //    FestivalName = model.FestivalName,
                        //    FType = db.FestivalTypes.Where(p => p.ID == model.SelectedFestivalType).Single(),
                        //    StartDate = model.startDate.Date,
                        //    FestivalTown = db.Towns.Where(p => p.ID == model.SelectedTown).Single(),
                        //    UserID = WebSecurity.CurrentUserId,
                        //    FestivalLogo = model.festivalLogo
                        //};

                        db.Entry(fest).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index", "Festival");
                    }
                }
                ModelState.AddModelError("", "No Festival Type Picked");
            }

            return View(model);
        }

        #endregion

        #region Festival/Delete

        //
        // GET: /Festival/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Festival festival = db.Festivals.Find(id);
            if (festival == null)
            {
                return HttpNotFound();
            }
            return View(festival);
        }

        //
        // POST: /Festival/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Festival festival = db.Festivals.Find(id);
            db.Festivals.Remove(festival);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        #region Add a new Festival Type

        [HttpPost]
        public JsonResult CreateFestivalType(FestivalTypeVM model)
        {
            if (ModelState.IsValid)
            {
                if (db.FestivalTypes.Where(ft => ft.FType.ToLower() == model.Name.ToLower()).ToList().Count == 0)
                {
                    db.FestivalTypes.Add(new FestivalType { FType = model.Name });
                    db.SaveChanges();
                    model.FestivalTypeID = db.FestivalTypes.Where(ft => ft.FType.ToLower() == model.Name.ToLower()).Single().ID;
                    return Json(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Festival type already exsist");
                    return Json(model);
                }
            }
            return Json(model);
        }

        #endregion

        #region Check dates

        /*public ActionResult SearchFree(DateTime? .StartDate, DateTime? EndDate)
        {
            if (!StartDate.HasValue || !EndDate.HasValue)
            {
                ModelState.AddModelError("Date", "Dates are empty");
                return View();
            }

            if(StartDate.Value > EndDate.HasValue)
            {
                ModelState.AddModelError("Date", "Start date must be before End Date");
                return View();
            }
        }*/

        #endregion
    }
}