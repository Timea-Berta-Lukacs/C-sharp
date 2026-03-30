using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Biblioteca.Models;

namespace Biblioteca.Controllers
{
    public class ClientiController : Controller
    {
        private BibliotecaContext db = new BibliotecaContext();

        // GET: Clienti
        public ActionResult Index(string nume)
        {
            var clienti = from c in db.Clienti select c;
            if(!string.IsNullOrEmpty(nume))
            {
                clienti = clienti.Where(c => c.Nume.Contains(nume));
            }
            return View(clienti);
        }

        // GET: Clienti/Details/5
        public ActionResult Details(int? id)
        {
 
            int a;
            DateTime d;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clienti.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            int.TryParse(client.CNP.Substring(1, 2), out int an);
            int.TryParse(client.CNP.Substring(3, 2), out int luna);
            int.TryParse(client.CNP.Substring(5, 2), out int zi);
            if(an<=22)
            {
                d = new DateTime(2000 + an, luna, zi);
            }
             else
            {
                d = new DateTime(1900 + an, luna, zi);
            }

            long ticks = DateTime.Now.Ticks - d.Ticks;
            DateTime D = new DateTime(ticks);
            a = D.Year - 1;

            ViewBag.varsta = a;
            ViewBag.Data = d;            
            return View(client);
        }

        // GET: Clienti/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clienti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientId,Nume,CNP,Adresa,Telefon")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clienti.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: Clienti/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clienti.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clienti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClientId,Nume,CNP,Adresa,Telefon")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clienti/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clienti.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clienti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clienti.Find(id);
            db.Clienti.Remove(client);
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
