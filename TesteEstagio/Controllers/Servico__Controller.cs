using System;
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
using TesteEstagio.Models;

namespace TesteEstagio.Controllers
{
    public class Servico__Controller : ApiController
    {
        private BDTesteEntities1 db = new BDTesteEntities1();

        // GET: api/Servico__
        public IQueryable<Servico__> GetServico__(String Opcoes)
        {
            IQueryable<Servico__> servico;
            switch(Opcoes)
            {
                case "Solicitacaomaior":
                    db.Servico__.OrderBy(b => b.Nome.).GroupBy(t=> t.Nome);
                    break;
                case "Solicitacaomenor":
                    db.Servico__.OrderByDescending(b => b.Nome).GroupBy(t => t.Nome);
                    break;
                case "Normal":
                    db.Servico__.ToList();
                    break;
            }
            return db.Servico__;

        }


        // GET: api/Servico__/5
        [ResponseType(typeof(Servico__))]
        public async Task<IHttpActionResult> GetServico__(int id)
        {
            Servico__ servico__ = await db.Servico__.FindAsync(id);
            if (servico__ == null)
            {
                return NotFound();
            }

            return Ok(servico__);
        }

        // PUT: api/Servico__/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutServico__(int id, Servico__ servico__)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != servico__.Id)
            {
                return BadRequest();
            }

            db.Entry(servico__).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Servico__Exists(id))
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

        // POST: api/Servico__
        [ResponseType(typeof(Servico__))]
        public async Task<IHttpActionResult> PostServico__(Servico__ servico__)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Servico__.Add(servico__);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = servico__.Id }, servico__);
        }

        // DELETE: api/Servico__/5
        [ResponseType(typeof(Servico__))]
        public async Task<IHttpActionResult> DeleteServico__(int id)
        {
            Servico__ servico__ = await db.Servico__.FindAsync(id);
            if (servico__ == null)
            {
                return NotFound();
            }

            db.Servico__.Remove(servico__);
            await db.SaveChangesAsync();

            return Ok(servico__);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Servico__Exists(int id)
        {
            return db.Servico__.Count(e => e.Id == id) > 0;
        }
    }
}