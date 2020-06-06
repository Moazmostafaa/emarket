using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using emarket.Filters;
using emarket.Models;
using emarket.ViewModels.Product;

namespace emarket.Controllers
{
    public class ProductsController : Controller
    {
        private EMarketDbContext db = new EMarketDbContext();

        // GET: Products
        public ActionResult Index()
        {
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.Products = ProductsFilter(new ProductFilter());
            ViewBag.Categories = db.Categories.ToList();

            return PartialView(viewModel);
        }

        [HttpPost]
        public ActionResult Index(ProductViewModel viewModel)
        {
            viewModel.Products = ProductsFilter(viewModel.Filter);
            ViewBag.Categories = db.Categories.ToList();
            return PartialView(viewModel);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ProductViewModel viewModel = new ProductViewModel()
            {
                CategoryName = db.Categories.FirstOrDefault(x => x.Id == product.CategoryId).Name,
                Product = product
            };
            return PartialView(viewModel);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                upload.SaveAs(path);
                product.Image = upload.FileName;

                db.Products.Add(product);

                var category = db.Categories.FirstOrDefault(x => x.Id == product.CategoryId);
                category.NumberOfProducts += 1;
                db.Entry(category).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = db.Categories.ToList();

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = db.Categories.ToList();
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string oldPath = Path.Combine(Server.MapPath("~/Uploads"), product.Image);
                if (upload != null)
                {
                    string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                    upload.SaveAs(path);
                    product.Image = upload.FileName;
                    db.Products.Add(product);
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = db.Categories.ToList();
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);

            var category = db.Categories.FirstOrDefault(x => x.Id == product.CategoryId);
            category.NumberOfProducts -= 1;
            db.Entry(category).State = EntityState.Modified;

            var cartProduct = db.Carts.FirstOrDefault(x => x.ProductId == id);
            db.Carts.Remove(cartProduct);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<Product> ProductsFilter(ProductFilter filter)
        {
            var products = db.Products.ToList();

            if (filter.CategoryId > 0)
                products = products.Where(x => x.CategoryId == filter.CategoryId).ToList();

            if(!string.IsNullOrEmpty(filter.ProductName))
                products = products.Where(x => x.Name.ToLower().Contains(filter.ProductName.ToLower())).ToList();

            return products;
        }
    }
}
