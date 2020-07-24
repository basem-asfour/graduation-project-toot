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
   // [RoutePrefix("rpc/Gifts/[action]")]
    public class ApiGiftsController : ApiController
    {
        private tootEntities db = new tootEntities();

        // GET: api/ApiGifts
        public IHttpActionResult Getgifts()
        {
            return Ok(db.gifts.Select(x=>new  { x.id,x.name,x.photo,x.description,x.cost,x.material,x.available,x.houseId,houseName=x.house.name}));
        }

        // GET: api/ApiGifts/5
        [ResponseType(typeof(gift))]
        public IHttpActionResult Getgift(int id)
        {
            var gift = db.gifts.Select(x => new { x.id, x.name, x.photo, x.description, x.cost, x.material, x.available, x.houseId, houseName = x.house.name }).FirstOrDefault(x=>x.id==id);
            if (gift == null)
            {
                return NotFound();
            }

            return Ok(gift);
        }

        // PUT: api/ApiGifts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putgift(int id, gift gift)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gift.id)
            {
                return BadRequest();
            }

            db.Entry(gift).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!giftExists(id))
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

        // POST: api/ApiGifts
        [ResponseType(typeof(gift))]
        public IHttpActionResult Postgift(gift gift)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.gifts.Add(gift);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = gift.id }, gift);
        }

        // DELETE: api/ApiGifts/5
        [ResponseType(typeof(gift))]
        public IHttpActionResult Deletegift(int id)
        {
            gift gift = db.gifts.Find(id);
            if (gift == null)
            {
                return NotFound();
            }

            db.gifts.Remove(gift);
            db.SaveChanges();

            return Ok(gift);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool giftExists(int id)
        {
            return db.gifts.Count(e => e.id == id) > 0;
        }
    }
}