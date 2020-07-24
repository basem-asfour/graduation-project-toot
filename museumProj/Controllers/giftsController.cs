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
    public class giftsController : Controller
    {
        private tootEntities db = new tootEntities();

        // GET: gifts
        public ActionResult Index()
        {
            var gifts = db.gifts.Include(g => g.house);
            return View(gifts.ToList());
        }

        // GET: gifts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gift gift = db.gifts.Find(id);
            if (gift == null)
            {
                return HttpNotFound();
            }
            return View(gift);
        }

        // GET: gifts/Create
        public ActionResult Create()
        {
            ViewBag.houseId = new SelectList(db.houses, "id", "name");
            return View();
        }

        // POST: gifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(gift gift)
        {
            if (gift.photo_file != null)
            {
                string filename = Path.GetFileNameWithoutExtension(gift.photo_file.FileName.Replace(" ", string.Empty));
                string extention = Path.GetExtension(gift.photo_file.FileName.Replace(" ", string.Empty));
                filename = filename + DateTime.Now.ToString("yymmddssfff") + extention;
                gift.photo = "~/Content/images/" + filename;
                filename = Path.Combine(Server.MapPath("~/Content/images/"), filename);
                gift.photo_file.SaveAs(filename);
            }
            if (ModelState.IsValid)
            {
                db.gifts.Add(gift);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.houseId = new SelectList(db.houses, "id", "name", gift.houseId);
            return View(gift);
        }

        // GET: gifts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gift gift = db.gifts.Find(id);
            if (gift == null)
            {
                return HttpNotFound();
            }
            ViewBag.houseId = new SelectList(db.houses, "id", "name", gift.houseId);
            return View(gift);
        }

        // POST: gifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description,cost,material,photo,houseId,available")] gift gift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.houseId = new SelectList(db.houses, "id", "name", gift.houseId);
            return View(gift);
        }

        // GET: gifts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gift gift = db.gifts.Find(id);
            if (gift == null)
            {
                return HttpNotFound();
            }
            return View(gift);
        }

        // POST: gifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gift gift = db.gifts.Find(id);
            db.gifts.Remove(gift);
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
