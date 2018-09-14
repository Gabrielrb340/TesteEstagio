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
    public class ClientesController : ApiController
    {
        private BDTesteEntities1 db = new BDTesteEntities1();

        // GET: api/Clientes
        //sem nenhuma condição.
        public IQueryable<Cliente> GetCliente()
        {
            return db.Clientes;
        }

        //caso queira ordenar
        public IQueryable<Cliente> GetCliente(String OrdenarNome)
        {
            IQueryable<Cliente> clientes;
            switch (OrdenarNome)
            {
                case "AZ":
                    clientes = db.Clientes.OrderBy(n => n.Nome);
                    break;
                case "ZA":
                    clientes = db.Clientes.OrderByDescending(n => n.Nome);
                    break;
                default:
                    clientes = db.Clientes;
                    break;
            }

            return clientes;
        }

        //caso queira procurar pelo nome
        public List<Cliente> GetClientesnome(string BuscaNome)
        {
            var clientes = db.Clientes.Where(c => c.Nome.StartsWith(BuscaNome));
            return clientes.ToList();
        }

        // GET: api/Clientes/5
        [ResponseType(typeof(Cliente))]
        public async Task<IHttpActionResult> GetCliente(int id)
        {
            Cliente cliente = await db.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }

        // PUT: api/Clientes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCliente(int id, Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cliente.Id)
            {
                return BadRequest();
            }

            db.Entry(cliente).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
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

        // POST: api/Clientes
        [ResponseType(typeof(Cliente))]
        public async Task<IHttpActionResult> PostCliente(Cliente cliente)
        {
            String email = cliente.Email.ToString();
            var c = db.Clientes.Where(v => v.Email.Contains(email));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (c.Equals(email))
            {
                BadRequest("Email Ja Cadastrado");

            }
            else
            {
                db.Clientes.Add(cliente);
                await db.SaveChangesAsync();

            }
                return CreatedAtRoute("DefaultApi", new { id = cliente.Id }, cliente);
            
        }

        // DELETE: api/Clientes/5
        [ResponseType(typeof(Cliente))]
        public async Task<IHttpActionResult> DeleteCliente(int id)
        {
            Cliente cliente = await db.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            db.Clientes.Remove(cliente);
            await db.SaveChangesAsync();

            return Ok(cliente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClienteExists(int id)
        {
            return db.Clientes.Count(e => e.Id == id) > 0;
        }
    }
}