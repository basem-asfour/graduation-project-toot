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

namespace museumProj.Controllers
{
    public class ApiPlacesController : ApiController
    {
        private tootEntities db = new tootEntities();

        // GET: api/ApiPlaces
        public IHttpActionResult GetHome()
        {
            var places = db.places.Select(x => new { x.id, x.name, x.governorate,
                Images = x.place_images.Select(
                y => new { y.id, y.image, y.altr })
            });
            var Menuments = db.menuments.Select(x => new { x.id, x.name,MainPlace= x.place.name,
                Images = x.menument_images.Select(
                y => new { y.id, y.image, y.altr })
            });
            var gifts = db.gifts.Select(x => new { x.id, x.name, x.photo });

            var all = new { places, Menuments, gifts };
            return Ok(all);
        }
        public IHttpActionResult Getplaces()
        {
            var places = db.places.Select(x => new { x.id, x.name, x.governorate,Images= x.place_images.Select(
                y=>new {y.id,y.image,y.altr }) });
            return Ok(places);
        }

        // GET: api/ApiPlaces/5
        [ResponseType(typeof(place))]
        public IHttpActionResult Getplace(int id)
        {
            var place = db.places.Select(x => new {
                x.id,
                x.name,
                x.governorate,
                x.Lattitude,
                x.Longitude,
                x.map,
                Catogegories= x.places_cat.Select(z=>new { z.catogery.name,z.cat_id}),
                monuments=x.menuments.Select(c=>new { c.id,c.name,c.menument_images.FirstOrDefault().image}),
                Images = x.place_images.Select(
                y => new { y.id, y.image, y.altr })
            }).FirstOrDefault(a=>a.id==id);
            if (place == null)
            {
                return NotFound();
            }

            return Ok(place);
        }

        // PUT: api/ApiPlaces/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putplace(int id, place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != place.id)
            {
                return BadRequest();
            }

            db.Entry(place).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!placeExists(id))
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

        // POST: api/ApiPlaces
        [ResponseType(typeof(place))]
        public IHttpActionResult Postplace(place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.places.Add(place);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = place.id }, place);
        }

        // DELETE: api/ApiPlaces/5
        [ResponseType(typeof(place))]
        public IHttpActionResult Deleteplace(int id)
        {
            place place = db.places.Find(id);
            if (place == null)
            {
                return NotFound();
            }

            db.places.Remove(place);
            db.SaveChanges();

            return Ok(place);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool placeExists(int id)
        {
            return db.places.Count(e => e.id == id) > 0;
        }
    }
}