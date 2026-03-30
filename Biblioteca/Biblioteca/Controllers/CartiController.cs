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
    public class CartiController : Controller
    {
        private BibliotecaContext db = new BibliotecaContext();

        // GET: Carti
        public ActionResult Index()
        {
            return View(db.Carti.ToList());
        }

        // GET: Carti/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carte carte = db.Carti.Find(id);
            if (carte == null)
            {
                return HttpNotFound();
            }
            return View(carte);
        }

        // GET: Carti/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CarteId,Titlu,Autor,Editura,AnAparitie")] Carte carte)
        {
            if (ModelState.IsValid)
            {
                db.Carti.Add(carte);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carte);
        }

        // GET: Carti/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carte carte = db.Carti.Find(id);
            if (carte == null)
            {
                return HttpNotFound();
            }
            return View(carte);
        }

        // POST: Carti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CarteId,Titlu,Autor,Editura,AnAparitie")] Carte carte)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carte).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carte);
        }

        // GET: Carti/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carte carte = db.Carti.Find(id);
            if (carte == null)
            {
                return HttpNotFound();
            }
            return View(carte);
        }

        // POST: Carti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Carte carte = db.Carti.Find(id);
            db.Carti.Remove(carte);
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
