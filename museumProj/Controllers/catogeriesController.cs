using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using museumProj.Models;

namespace museumProj.Controllers
{
    public class catogeriesController : Controller
    {
        private tootEntities db = new tootEntities();

        // GET: catogeries
        public ActionResult Index()
        {
            return View(db.catogeries.ToList());
        }

        // GET: catogeries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catogery catogery = db.catogeries.Find(id);
            if (catogery == null)
            {
                return HttpNotFound();
            }
            return View(catogery);
        }

        // GET: catogeries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: catogeries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name")] catogery catogery)
        {
            if (ModelState.IsValid)
            {
                db.catogeries.Add(catogery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(catogery);
        }

        // GET: catogeries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catogery catogery = db.catogeries.Find(id);
            if (catogery == null)
            {
                return HttpNotFound();
            }
            return View(catogery);
        }

        // POST: catogeries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name")] catogery catogery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(catogery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(catogery);
        }

        // GET: catogeries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catogery catogery = db.catogeries.Find(id);
            if (catogery == null)
            {
                return HttpNotFound();
            }
            return View(catogery);
        }

        // POST: catogeries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            catogery catogery = db.catogeries.Find(id);
            db.catogeries.Remove(catogery);
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
