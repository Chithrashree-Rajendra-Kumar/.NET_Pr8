using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using Database.NET.Models;

namespace Database.NET.Controllers
{
    // CONTROLLER: receives requests, calls the model (ADO.NET), returns views
    public class ProductController : Controller
    {
        private readonly ProductRepository repo = new ProductRepository();

        private const string DbError =
            "Could not reach the database. Check the connection string in Web.config " +
            "and make sure SQL/InventoryDB.sql was executed. Details: ";

        // READ: list all products
        public ActionResult Index()
        {
            try
            {
                return View(repo.GetAll());
            }
            catch (SqlException ex)
            {
                ViewBag.Error = DbError + ex.Message;
                return View(new List<Product>());
            }
        }

        // CREATE - GET
        public ActionResult Create()
        {
            return View(new Product());
        }

        // CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            try
            {
                repo.Add(product);
                TempData["Message"] = "Product added successfully.";
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", DbError + ex.Message);
                return View(product);
            }
        }

        // UPDATE - GET
        public ActionResult Edit(int id)
        {
            try
            {
                Product product = repo.GetById(id);
                if (product == null)
                    return HttpNotFound();
                return View(product);
            }
            catch (SqlException ex)
            {
                TempData["Error"] = DbError + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            try
            {
                if (!repo.Update(product))
                    return HttpNotFound();

                TempData["Message"] = "Product updated successfully.";
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", DbError + ex.Message);
                return View(product);
            }
        }

        // DELETE (POST only, so a link or crawler can never delete data)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                TempData["Message"] = repo.Delete(id)
                    ? "Product deleted successfully."
                    : "Product not found.";
            }
            catch (SqlException ex)
            {
                TempData["Error"] = DbError + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
