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
    public class placesController : Controller
    {
        private tootEntities db = new tootEntities();
        public ActionResult AssignCat(int? id)
        {
            ViewBag.place_id = new SelectList(db.places, "id", "name");
            ViewBag.cat_id = new SelectList(db.catogeries, "id", "name");
            /// var my_place = db.places_cat.Where(x=>x.place_id==id);
            return View();
        }
        public IEnumerable<places_cat> get_place_cat(int? id)
        {
            var cats = db.places_cat.Where(x => x.place_id == id);
            return cats;
        }
        [HttpPost]
        public ActionResult AssignCat(places_cat cat)
        {

            if (ModelState.IsValid)
            {
                cat.rank =0;
                db.places_cat.Add(cat);
                db.SaveChanges();
            }
            return View("AssignCat", db.places.FirstOrDefault(x => x.id == cat.place_id).places_cat);
        }

        [HttpGet]
        public ActionResult AddImages(int id)
        {
            var images = db.place_images.Where(x => x.place_id == id);
            ViewBag.placeid = id;
            return View(images);
        }
        [HttpPost]
        public ActionResult AddImages(place_images img)
        {
            //img.place_id =ViewBag.placeid;
            string filename = Path.GetFileNameWithoutExtension(img.image_file.FileName);
            string extention = Path.GetExtension(img.image_file.FileName);
            filename = filename + DateTime.Now.ToString("yymmddssfff") + extention;
            img.image = "~/Content/images/places/" + filename;
            filename = Path.Combine(Server.MapPath("~/Content/images/places/"), filename);
            img.image_file.SaveAs(filename);

            db.place_images.Add(img);
            db.SaveChanges();
            return View("AddImages",db.place_images.Where(x=>x.place_id== img.place_id));
        }

        // GET: places
        public ActionResult Index()
        {
            return View(db.places.ToList());
        }

        // GET: places/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            place place = db.places.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // GET: places/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: places/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "id,name,address,dates,fees,map,map_image,logo_image")]*/ place place)
        {
            if (ModelState.IsValid)
            {
                string filename = Path.GetFileNameWithoutExtension(place.map_image.FileName);
                string extention = Path.GetExtension(place.map_image.FileName);
                filename = filename + DateTime.Now.ToString("yymmddssfff") + extention;
                place.map = "~/Content/images/map/" + filename;
                filename = Path.Combine(Server.MapPath("~/Content/images/map/"), filename);
                place.map_image.SaveAs(filename);

                db.places.Add(place);
                db.SaveChanges();

                var new_imageLogo = new place_images();
                new_imageLogo.place_id = db.places.OrderByDescending(x => x.id).FirstOrDefault().id;
                string filename2 = Path.GetFileNameWithoutExtension(place.logo_image.FileName);
                string extention2 = Path.GetExtension(place.logo_image.FileName);
                filename2 = filename2 + DateTime.Now.ToString("yymmddssfff") + extention2;
                new_imageLogo.image = "~/Content/images/places/" + filename2;
                filename2 = Path.Combine(Server.MapPath("~/Content/images/places/"), filename2);
                place.logo_image.SaveAs(filename2);
                new_imageLogo.altr = "Logo Image";
                db.place_images.Add(new_imageLogo);
                db.SaveChanges();



                return RedirectToAction("Index");
            }

            return View(place);
        }

        // GET: places/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            place place = db.places.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // POST: places/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,address,dates,fees,map")] place place)
        {
            if (ModelState.IsValid)
            {
                db.Entry(place).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(place);
        }

        // GET: places/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            place place = db.places.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // POST: places/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            place place = db.places.Find(id);
            db.places.Remove(place);
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
