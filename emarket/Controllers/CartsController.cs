using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using emarket.Models;
using emarket.ViewModels;

namespace emarket.Controllers
{
    public class CartsController : Controller
    {
        private EMarketDbContext db = new EMarketDbContext();

        // GET: Carts
        public ActionResult Index()
        {
            CartModalViewModel viewModel = new CartModalViewModel();
            viewModel.Carts = new List<CartViewModel>();

            var carts = db.Carts.ToList().OrderByDescending(x => x.AddedAt);

            foreach (var item in carts)
            {
                CartViewModel cartViewModel = new CartViewModel()
                {
                    Product = db.Products.FirstOrDefault(x => x.Id == item.ProductId),
                    Cart = item,
                    AddedAt = ToRelativeDate(item.AddedAt)
                };
                viewModel.Carts.Add(cartViewModel);
            }
            return PartialView(viewModel);
        }

        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id)
        {
            if (ModelState.IsValid)
            {
                var isExist = db.Carts.Any(x => x.ProductId == id);
                if (isExist)
                {
                    return RedirectToAction("Index", "Products");
                }

                Cart cart = new Cart()
                {
                    AddedAt = DateTime.Now,
                    ProductId = id,
                };
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index", "Products");
            }

            return RedirectToAction("Index", "Products");
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,AddedAt")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cart);
        }


        // POST: Carts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Index", "Products");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public string ToRelativeDate(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan <= TimeSpan.FromSeconds(60))
                return string.Format("{0} seconds ago", timeSpan.Seconds);

            if (timeSpan <= TimeSpan.FromMinutes(60))
                return timeSpan.Minutes > 1 ? String.Format("Added to cart {0} minutes ago", timeSpan.Minutes) : "Added to cart a minute ago";

            if (timeSpan <= TimeSpan.FromHours(24))
                return timeSpan.Hours > 1 ? String.Format("Added to cart {0} hours ago", timeSpan.Hours) : "Added to cart an hour ago";

            if (timeSpan <= TimeSpan.FromDays(30))
                return timeSpan.Days > 1 ? String.Format("Added to cart {0} days ago", timeSpan.Days) : "Added to cart yesterday";

            if (timeSpan <= TimeSpan.FromDays(365))
                return timeSpan.Days > 30 ? String.Format("Added to cart {0} months ago", timeSpan.Days / 30) : "Added to cart a month ago";

            return timeSpan.Days > 365 ? String.Format("Added to cart {0} years ago", timeSpan.Days / 365) : "Added to cart a year ago";
        }
    }
}
