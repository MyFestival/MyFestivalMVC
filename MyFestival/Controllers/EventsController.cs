﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyFestival.Models;
using System.Net.Http;
using System.Net;

namespace MyFestival.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        MyFestivalDb db = new MyFestivalDb();

        #region Create

        [HttpGet]
        public ActionResult Create2(int festID)
        {
            EventsVM events = new EventsVM { festivalID = festID };

            events.eType = db.EType.ToDictionary(p => p.ID, q => q.EType);
            events.eType.Add(-1, "----- Add New Event Type -----");

            events.eventsDate = DateTime.Now;
            events.startTime = DateTime.Now;
            events.endTime = DateTime.Now;

            return View(events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2(EventsVM model, int festID)
        {
            if (ModelState.IsValid != true)
            {
                if (model.selectedEType != -1)
                {
                    //db.save stuff from create.
                    Events Newevent = new Events();
                    Newevent.EndTime = model.endTime;
                    Newevent.StartTime = model.startTime;
                    Newevent.EventsDate = model.eventsDate = DateTime.Now;
                    Newevent.EventsName = model.EventsName;
                    Newevent.EType = db.EType.Where(p => p.ID == model.selectedEType).Single();
                    Newevent.Location = model.Location;
                    if (Request.Files.Count != 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        string serverPath = Server.MapPath("~\\Content\\EventPicture");
                        Bitmap newImage = new Bitmap(Request.Files[0].InputStream);
                        newImage.Save(serverPath + "\\" + fileName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                        model.eventsImage = "Content/EventPicture/" + fileName + ".jpg";

                        Newevent.FestivalID = model.festivalID = festID;

                        db.Events.Add(Newevent);
                        db.SaveChanges();
                        //Change the model.festivalID to Newevent.FestivalID
                        return RedirectToAction("Details", "Festival", new { id = Newevent.FestivalID });
                    }
                    else
                    {
                        db.Events.Add(Newevent);
                        db.SaveChanges();
                        //Change the model.festivalID to Newevent.FestivalID
                        return RedirectToAction("Details", "Festival", new { id = Newevent.FestivalID });
                    }
                }
                ModelState.AddModelError("", "No Event Type Picked");
            }

            model.eType = db.EType.ToDictionary(p => p.ID, q => q.EType);
            model.eType.Add(-1, "----- Add New Event Type -----");
            model.eventsDate = DateTime.Now;
            model.startTime = DateTime.Now;
            model.endTime = DateTime.Now;

            return View(model);
        }
        #endregion

        #region Create a new Event Type

        [HttpPost]
        public JsonResult CreateEventType(EventTypeVM model)
        {
            if (ModelState.IsValid)
            {
                if (db.EType.Where(et => et.EType.ToLower() == model.Name.ToLower()).ToList().Count == 0)
                {
                    db.EType.Add(new EventType { EType = model.Name });
                    db.SaveChanges();
                    model.EventTypeID = db.EType.Where(et => et.EType.ToLower() == model.Name.ToLower()).Single().ID;
                    return Json(model);
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Event type already exsist.");
                    return Json(model);
                }
            }
            return Json(model);
        }

        #endregion

        #region Delete

        // GET: /Festival/Delete/5
        public ActionResult Delete(int id)
        {
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        // POST: /Festival/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Events events = db.Events.Find(id);
            db.Events.Remove(events);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        #region Edit ViewModel

        [HttpGet]
        public ActionResult Edit2(int? id)
        {
            Events eve = db.Events.Single(x => x.ID == id);

            if (eve == null)
            {
                return RedirectToAction("Create2");
            }

            EventsVM events = new EventsVM
            {
                selectedEType = eve.EType.ID,
                endTime = eve.EndTime,
                startTime = eve.StartTime,
                festivalID = eve.FestivalID,
                EventsName = eve.EventsName,
                eventsDate = eve.EventsDate,
                Location = eve.Location,
                ID = eve.ID
            };
            events.eType = db.EType.ToDictionary(p => p.ID, q => q.EType);
            events.eType.Add(-1, "----- Add New -----");
            return View(events);

        }

        [HttpPost]
        public ActionResult Edit2(EventsVM model)
        {
            if (ModelState.IsValid != true)
            {
                if (model.selectedEType != -1)
                {
                    if (Request.Files.Count != 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        string serverPath = Server.MapPath("~\\Content\\Images\\Uploaded");
                        Bitmap newImage = new Bitmap(Request.Files[0].InputStream);
                        newImage.Save(serverPath + "\\" + fileName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                        model.eventsImage = "/Content/Images/Uploaded/" + fileName + ".jpg";

                        Events events = db.Events.Single(x => x.ID == model.ID);
                        events.ID = model.ID;
                        events.EType = db.EType.Where(p => p.ID == model.selectedEType).Single();
                        events.EventsName = model.EventsName;
                        events.StartTime = model.startTime;
                        events.EndTime = model.endTime;
                        events.Location = model.Location;
                        events.EventLogo = model.eventsImage;   
                        events.EventsDate = model.eventsDate;
                        events.FestivalID = model.festivalID;

                        db.Entry(model).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index", "Festival");
                    }
                    else
                    {
                        Events eve = db.Events.Single(x => x.ID == model.ID);

                        eve.ID = model.ID;
                        eve.EType = db.EType.Where(p => p.ID == model.selectedEType).Single();
                        eve.EventsName = model.EventsName;
                        eve.StartTime = model.startTime;
                        eve.EndTime = model.endTime;
                        eve.Location = model.Location;
                        eve.EventLogo = model.eventsImage;
                        eve.EventsDate = new DateTime(model.eventsDate.Year, model.eventsDate.Month,
                            model.eventsDate.Day);//model.eventsDate.ToString()
                        eve.FestivalID = model.festivalID;


                        db.Entry(eve).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index", "Festival");
                    }
                }
                ModelState.AddModelError("", "No Event Type Picked");
            }

            return View(model);
        }

        #endregion

        #region Details

        public ActionResult Details(int? id)
        {
            Events events = db.Events.Find(id);

            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        #endregion

    }
}