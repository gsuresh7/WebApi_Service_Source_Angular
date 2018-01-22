﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BookService.Models;
using System.Web.Http.Cors;
using BookService.BGC.Click.Services;

namespace BookService.Controllers
{
   // [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]-to enbale for singal controller.Globally enbaled @webapiconfig
    public class AuthorsController : ApiController
    {
        private BookServiceContext db = new BookServiceContext();
        Dictionary[] _stockItemNumbers;
        // GET: api/Authors
        public IQueryable<Author> GetAuthors()
        {
            return db.Authors;
        }

        // GET: api/Authors/5
        [ResponseType(typeof(Author))]
        public async Task<IHttpActionResult> GetAuthor(int id)
        {
            Author author = await db.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

       
      
        public IQueryable<Author> GetAuthorbySearch(String queryOptions)
        {
            return db.Authors.Where(b => b.Name.Contains(queryOptions));
           
        }

        public List<StockNumber> GetAuthorsbyTask(string gettaskstest)
        {
            ServiceOptimizationServiceClient soc = new ServiceOptimizationServiceClient("CustomBinding_ServiceOptimizationService1");
            DictionaryType[] dictionaryTypes = new DictionaryType[] { new DictionaryType() { Name = "StockItemNumber" } };
            string[] requestedProperties = new string[] { "IsStockAvailableCheckRequired", "Name", "Key" };
            _stockItemNumbers = soc.GetDictionaryItem(null, dictionaryTypes, requestedProperties);
            List<StockNumber> StockNumbers = new List<StockNumber>();
            foreach (Dictionary dict in _stockItemNumbers)
            {
                StockNumbers.Add(
                    new StockNumber { Key=dict.Key,Name=dict.Name});
            }
            
            return StockNumbers.GetRange(0,5);
        }


        // PUT: api/Authors/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAuthor(int id, Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != author.Id)
            {
                return BadRequest();
            }

            db.Entry(author).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // POST: api/Authors
        [ResponseType(typeof(Author))]
        public async Task<IHttpActionResult> PostAuthor(Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Authors.Add(author);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [ResponseType(typeof(Author))]
        public async Task<IHttpActionResult> DeleteAuthor(int id)
        {
            Author author = await db.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            db.Authors.Remove(author);
            await db.SaveChangesAsync();

            return Ok(author);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AuthorExists(int id)
        {
            return db.Authors.Count(e => e.Id == id) > 0;
        }
    }
}