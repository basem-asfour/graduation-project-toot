using MultipartDataMediaFormatter;
using MultipartDataMediaFormatter.Infrastructure;
using museumProj.Models;
using museumProj.Models.ClasessForReturn;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ZXing;

namespace museumProj.Controllers
{
    public class QRCodeController : ApiController
    {
        private tootEntities db = new tootEntities();
        [HttpGet]
        public cls_menument Read(string name)
        {

            string men_name = name;//ReadQRCode(file).QRCodeText;
            var my_menument = db.menuments.Select(x => new cls_menument()
            {
                id = x.id,
                name = x.name,
                description = x.description,
                voice_note = x.voice_note,
                QR_image = x.QR_image,
                no_of_scans = x.no_of_scans,
                place_id = x.place_id,
                place_name = x.place.name,
                menument_images = x.menument_images.Select(y => new cls_menument_images()
                {
                    id = y.id,
                    image = y.image,
                    altr = y.altr
                }).ToList()
            }).FirstOrDefault(x => x.name.Contains(men_name));
            if (my_menument != null)
            {
                var menTo_edit = db.menuments.FirstOrDefault(x => x.name == men_name);
                menTo_edit.no_of_scans += 1;
                db.Entry(menTo_edit).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                }
            }
            return my_menument;
        }
        // POST: api/QRCode
        [HttpPost]
        public cls_menument Read()
        {
            var file = HttpContext.Current.Request.Files.Count > 0 ?
        HttpContext.Current.Request.Files[0] : null;

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);

                var path = Path.Combine(
                    HttpContext.Current.Server.MapPath("~/uploads"),
                    fileName
                );
                //file.SaveAs(path);
            }

            string test= file != null ? "/uploads/" + file.FileName : null;

            string men_name = ReadQRCode(file).QRCodeText;
            var my_menument = db.menuments.Select(x => new cls_menument() {
                id = x.id,
                name = x.name,
                description = x.description,
                voice_note = x.voice_note,
                QR_image = x.QR_image,
                no_of_scans = x.no_of_scans,
                place_id=x.place_id,
                place_name=x.place.name,
                menument_images =x.menument_images.Select(y=>new cls_menument_images() {
                    id=y.id,
                    image=y.image,
                    altr=y.altr
                }).ToList()
                }).FirstOrDefault(x => x.name == men_name);
            if (my_menument !=null)
            {
                var menTo_edit = db.menuments.FirstOrDefault(x => x.name == men_name);
                menTo_edit.no_of_scans += 1;
                db.Entry(menTo_edit).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                }
            }
            return my_menument;
        }
        private QRCodeModel ReadQRCode(HttpPostedFile Image)
        {
            QRCodeModel barcodeModel = new QRCodeModel();
            string barcodeText = "";
            string imagePath = Image.FileName;
            //string barcodePath =Server.MapPath(imagePath);
            var barcodeReader = new BarcodeReader();

            var result = barcodeReader.Decode(new Bitmap(Image.InputStream));
            if (result != null)
            {
                barcodeText = result.Text;
            }
            return new QRCodeModel() { QRCodeText = barcodeText, QRCodeImagePath = imagePath };
        }
    }
}
