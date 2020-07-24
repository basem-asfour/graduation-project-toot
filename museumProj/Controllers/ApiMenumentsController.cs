using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using museumProj.Models;
using museumProj.Models.ClasessForReturn;

namespace museumProj.Controllers
{
   // [RoutePrefix("rpc/Menuments/[action]")]

    public class ApiMenumentsController : ApiController
    {
        private tootEntities db = new tootEntities();

        // GET: api/ApiMenuments
        public IQueryable<cls_menument> Getmenuments()
        {
            var my_menuments = db.menuments.Select(x => new cls_menument()
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
            });
            return my_menuments;
        }

        // GET: api/ApiMenuments/5
        [ResponseType(typeof(menument))]
        public IHttpActionResult Getmenument(int id)
        {
            var menument = db.menuments.Select(x => new cls_menument()
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
            }).FirstOrDefault(z=>z.id==id);

            if (menument == null)
            {
                return NotFound();
            }

            return Ok(menument);
        }

        // PUT: api/ApiMenuments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putmenument(int id, menument menument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != menument.id)
            {
                return BadRequest();
            }

            db.Entry(menument).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!menumentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ApiMenuments
        [ResponseType(typeof(menument))]
        public IHttpActionResult Postmenument(menument menument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.menuments.Add(menument);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = menument.id }, menument);
        }

        // DELETE: api/ApiMenuments/5
        [ResponseType(typeof(menument))]
        public IHttpActionResult Deletemenument(int id)
        {
            menument menument = db.menuments.Find(id);
            if (menument == null)
            {
                return NotFound();
            }

            db.menuments.Remove(menument);
            db.SaveChanges();

            return Ok(menument);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool menumentExists(int id)
        {
            return db.menuments.Count(e => e.id == id) > 0;
        }
    }
}