using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using museumProj.Models;

namespace museumProj.Controllers
{
    public class housesController : Controller
    {
        private tootEntities db = new tootEntities();

        // GET: houses
        public ActionResult Index()
        {
            var houses = db.houses.Include(h => h.house1).Include(h => h.place);
            return View(houses.ToList());
        }

        // GET: houses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            house house = db.houses.Find(id);
            if (house == null)
            {
                return HttpNotFound();
            }
            return View(house);
        }

        // GET: houses/Create
        public ActionResult Create()
        {
            ViewBag.placeId = new SelectList(db.houses, "id", "name");
            ViewBag.placeId = new SelectList(db.places, "id", "name");
            return View();
        }

        // POST: houses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( house house)
        {

            if (house.photo_file != null)
            {
                string filename = Path.GetFileNameWithoutExtension(house.photo_file.FileName.Replace(" ", string.Empty));
                string extention = Path.GetExtension(house.photo_file.FileName.Replace(" ", string.Empty));
                filename = filename + DateTime.Now.ToString("yymmddssfff") + extention;
                house.photo = "~/Content/images/" + filename;
                filename = Path.Combine(Server.MapPath("~/Content/images/"), filename);
                house.photo_file.SaveAs(filename);
            }
            if (ModelState.IsValid)
            {
                db.houses.Add(house);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.placeId = new SelectList(db.houses, "id", "name", house.placeId);
            ViewBag.placeId = new SelectList(db.places, "id", "name", house.placeId);
            return View(house);
        }

        // GET: houses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            house house = db.houses.Find(id);
            if (house == null)
            {
                return HttpNotFound();
            }
            ViewBag.placeId = new SelectList(db.houses, "id", "name", house.placeId);
            ViewBag.placeId = new SelectList(db.places, "id", "name", house.placeId);
            return View(house);
        }

        // POST: houses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,photo,governorate,address,Lattitude,Longitude,placeId,description")] house house)
        {
            if (ModelState.IsValid)
            {
                db.Entry(house).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.placeId = new SelectList(db.houses, "id", "name", house.placeId);
            ViewBag.placeId = new SelectList(db.places, "id", "name", house.placeId);
            return View(house);
        }

        // GET: houses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            house house = db.houses.Find(id);
            if (house == null)
            {
                return HttpNotFound();
            }
            return View(house);
        }

        // POST: houses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            house house = db.houses.Find(id);
            db.houses.Remove(house);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
