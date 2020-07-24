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
    //[RoutePrefix("rpc/Houses/[action]")]

    public class ApiHousesController : ApiController
    {
        private tootEntities db = new tootEntities();

        // GET: api/ApiHouses
        public IHttpActionResult Gethouses()
        {
            return Ok(db.houses.Select(
                x => new {
                    x.id,
                    x.name,
                    x.photo,
                    x.governorate,
                    x.description,
                    RelatedWith= x.place.name })); 
        }

        // GET: api/ApiHouses/5
        [ResponseType(typeof(house))]
        public IHttpActionResult Gethouse(int id)
        {
            var house = db.houses.Select(x =>
            new { x.id, x.name, x.photo, x.governorate, x.description,x.Lattitude
            ,x.Longitude,x.address,RelatedWith= x.place.name,GiftsList= x.gifts
            }).FirstOrDefault(y=>y.id==id);
            if (house == null)
            {
                return NotFound();
            }

            return Ok(house);
        }

        // PUT: api/ApiHouses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puthouse(int id, house house)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != house.id)
            {
                return BadRequest();
            }

            db.Entry(house).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!houseExists(id))
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

        // POST: api/ApiHouses
        [ResponseType(typeof(house))]
        public IHttpActionResult Posthouse(house house)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.houses.Add(house);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = house.id }, house);
        }

        // DELETE: api/ApiHouses/5
        [ResponseType(typeof(house))]
        public IHttpActionResult Deletehouse(int id)
        {
            house house = db.houses.Find(id);
            if (house == null)
            {
                return NotFound();
            }

            db.houses.Remove(house);
            db.SaveChanges();

            return Ok(house);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool houseExists(int id)
        {
            return db.houses.Count(e => e.id == id) > 0;
        }
    }
}