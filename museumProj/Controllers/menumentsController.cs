using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using museumProj.Models;
using ZXing;

namespace museumProj.Controllers
{
    public class menumentsController : Controller
    {
        private tootEntities db = new tootEntities();

        [HttpGet]
        public ActionResult AddImages(int id)
        {
            var images = db.menument_images.Where(x => x.menument_id == id);
            return View(images);
        }
        [HttpPost]
        public ActionResult AddImages(menument_images img)
        {
            string filename = Path.GetFileNameWithoutExtension(img.image_file.FileName);
            string extention = Path.GetExtension(img.image_file.FileName);
            filename = filename + DateTime.Now.ToString("yymmddssfff") + extention;
            img.image = "~/Content/images/Menuments/" + filename;
            filename = Path.Combine(Server.MapPath("~/Content/images/Menuments/"), filename);
            img.image_file.SaveAs(filename);

            db.menument_images.Add(img);
            db.SaveChanges();
            return View("AddImages", db.menument_images.Where(x => x.menument_id == img.menument_id));
        }


        // GET: menuments
        public ActionResult Index()
        {
            var menuments = db.menuments.Include(m => m.place).ToList();
            return View(menuments.ToList());
        }

        // GET: menuments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            menument menument = db.menuments.Find(id);
            if (menument == null)
            {
                return HttpNotFound();
            }
            return View(menument);
        }

        // GET: menuments/Create
        public ActionResult Create()
        {
            ViewBag.place_id = new SelectList(db.places, "id", "name");
            return View();
        }

        // POST: menuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "id,name,description,voice_note,QR_image,no_of_scans,place_id")]*/ menument menument)
        {
            if (ModelState.IsValid)
            {
                if (menument.Audio_file!=null)
                {
                    string filename = Path.GetFileNameWithoutExtension(menument.Audio_file.FileName.Replace(" ", string.Empty));
                    string extention = Path.GetExtension(menument.Audio_file.FileName.Replace(" ", string.Empty));
                    filename = filename + DateTime.Now.ToString("yymmddssfff") + extention;
                    menument.voice_note = "~/Content/Audio/" + filename;
                    filename = Path.Combine(Server.MapPath("~/Content/Audio/"), filename);
                    menument.Audio_file.SaveAs(filename);
                }
                

                menument.QR_image = GenerateQRCode(menument.name);
                menument.no_of_scans = 0;

                db.menuments.Add(menument);
                db.SaveChanges();

                if (menument.Logo_file!=null)
                {
                    var new_imageLogo = new menument_images();
                    new_imageLogo.menument_id = db.menuments.OrderByDescending(x => x.id).FirstOrDefault().id;
                    string filename2 = Path.GetFileNameWithoutExtension(menument.Logo_file.FileName.Replace(" ", string.Empty));
                    string extention2 = Path.GetExtension(menument.Logo_file.FileName.Replace(" ", string.Empty));
                    filename2 = filename2 + DateTime.Now.ToString("yymmddssfff") + extention2;
                    new_imageLogo.image = "~/Content/images/Menuments/" + filename2;
                    filename2 = Path.Combine(Server.MapPath("~/Content/images/Menuments/"), filename2);
                    menument.Logo_file.SaveAs(filename2);
                    new_imageLogo.altr = "Logo Image";
                    db.menument_images.Add(new_imageLogo);
                    db.SaveChanges();
                }
                
                /////////////////////////////////////////////////

                return RedirectToAction("Index");
            }

            ViewBag.place_id = new SelectList(db.places, "id", "name", menument.place_id);
            return View(menument);
        }
        private string GenerateQRCode(string qrcodeText)
        {
            string filename = qrcodeText.Replace(" ",string.Empty);
            string extention = ".jpg";
            filename = filename + DateTime.Now.ToString("yymmddssfff") + extention;
            string name_to_return = filename;
            filename = Path.Combine(Server.MapPath("~/Content/Images/QR/"), filename);

            string folderPath = "~/Content/Images/QR/";
            string imagePath = filename;
            // If the directory doesn't exist then create it.
            if (!Directory.Exists(Server.MapPath(folderPath)))
            {
                Directory.CreateDirectory(Server.MapPath(folderPath));
            }

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            var result = barcodeWriter.Write(qrcodeText);

            string barcodePath = imagePath;
            var barcodeBitmap = new Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            return folderPath+name_to_return;
        }

        // GET: menuments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            menument menument = db.menuments.Find(id);
            if (menument == null)
            {
                return HttpNotFound();
            }
            ViewBag.place_id = new SelectList(db.places, "id", "name", menument.place_id);
            return View(menument);
        }

        // POST: menuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description,voice_note,QR_image,no_of_scans,place_id")] menument menument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.place_id = new SelectList(db.places, "id", "name", menument.place_id);
            return View(menument);
        }

        // GET: menuments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            menument menument = db.menuments.Find(id);
            if (menument == null)
            {
                return HttpNotFound();
            }
            return View(menument);
        }

        // POST: menuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            menument menument = db.menuments.Find(id);
            db.menuments.Remove(menument);
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
