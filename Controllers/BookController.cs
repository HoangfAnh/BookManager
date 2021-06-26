using BookManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.ModelBinding;
using System.Data;

namespace BookManager.Controllers
{
    public class BookController : Controller
    {
        BookManagerContext context = new BookManagerContext();
        // GET: Book
        public ActionResult  ListBook()
        {
            return View(context.Books.ToList());
        }

        [Authorize]
        public ActionResult Buy(int id)
        {
            //BookManagerContext context = new BookManagerContext();
            Book book = context.Books.SingleOrDefault(p => p.ID == id);
            if(book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        [Authorize]
        public ActionResult CreateBook()
        {
            //BookManagerContext context = new BookManagerContext();
            return View();
        }
        [HttpPost, ActionName("CreateBook")]
        [ValidateAntiForgeryToken]
        public ActionResult Contact([Bind(Include ="Id, Title, Author, Description, Images, Price")]Book book)
        {
            //BookManagerContext context = new BookManagerContext();
            try
            {
                if (ModelState.IsValid)
                {
                    context.Books.Add(book);
                    context.SaveChanges();
                    return RedirectToAction("ListBook");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Error Save Data");
            }
            return View(book);
        }

        [Authorize]
        public ActionResult EditBook(int? id)
        {
            //BookManagerContext context = new BookManagerContext();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = context.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }
        [HttpPost, ActionName("EditBook")]
        [ValidateAntiForgeryToken]
        public ActionResult EditBook([Bind(Include = "ID, Title, Author, Description, Images, Price")]Book book)
        {
            //BookManagerContext context = new BookManagerContext();
            if (ModelState.IsValid)
            {
                context.Entry(book).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("ListBook");
            }
            return View(book);
        }

        [Authorize]
        public ActionResult DeleteBook(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = context.Books.Find(id);
            if(book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }
        [HttpPost, ActionName("DeleteBook")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = context.Books.Find(id);
            context.Books.Remove(book);
            context.SaveChanges();
            return RedirectToAction("ListBook");
        }
    }
}