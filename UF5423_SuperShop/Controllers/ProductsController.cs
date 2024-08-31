using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UF5423_SuperShop.Data;
using UF5423_SuperShop.Data.Entities;
using UF5423_SuperShop.Helpers;
using UF5423_SuperShop.Models;

namespace UF5423_SuperShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserHelper _userHelper;

        public ProductsController(IProductRepository productRepository, IUserHelper userHelper)
        {
            _productRepository = productRepository;
            _userHelper = userHelper;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_productRepository.GetAll().OrderBy(p => p.Name)); // Unique case of ordering property values.
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create() // Automatically set as create action and view by having action name 'Create' of type IActionResult.
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;
                if (model.ImageFile != null && model.ImageFile.Length > 0) // If model image file exists
                {
                    var guid = Guid.NewGuid().ToString(); // Globally unique identifier string.
                    var fileName = $"{guid}.png"; // Prevent file name repetition.

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(), // Application directory.
                        "wwwroot\\images\\products", // File directory within application.
                        fileName // File name.
                        );

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream); // Save file to path.
                    }

                    path = $"~/images/products/{fileName}"; // File path to save to database.
                }

                var product = this.ConvertToProduct(model, path);

                //TODO: define product user as logged-in user.
                product.User = await _userHelper.GetUserByEmailAsync("tiagomonteirinho.spam@gmail.com");
                await _productRepository.CreateAsync(product);
                //return RedirectToAction("Index"); // Define custom action name.
                return RedirectToAction(nameof(Index)); // Redirect to products list action.
            }
            return View(model); // Keep input changes if model data is invalid.
        }

        private Product ConvertToProduct(ProductViewModel model, string path) // Convert product view model to product model.
        {
            return new Product
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                ImageUrl = path,
                LastPurchase = model.LastPurchase,
                LastSale = model.LastSale,
                IsAvailable = model.IsAvailable,
                Stock = model.Stock,
                User = model.User
            };
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            var model = this.ConvertToProductViewModel(product); // Get product view model to show image file.

            return View(model);
        }

        private ProductViewModel ConvertToProductViewModel(Product product) // Convert product model to product view model.
        {
            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                IsAvailable = product.IsAvailable,
                Stock = product.Stock,
                User = product.User
            };
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model) // Gets product ID seperately.
        {
            //if (id != product.Id)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        var guid = Guid.NewGuid().ToString();
                        var fileName = $"{guid}.png";

                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\products",
                            fileName
                            );

                        using(var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/products/{fileName}";
                    }

                    var product = this.ConvertToProduct(model, path);

                    //TODO: define product user as logged-in user.
                    product.User = await _userHelper.GetUserByEmailAsync("tiagomonteirinho.spam@gmail.com"); // Prevent from removing product user on update.
                    await _productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productRepository.ExistsAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // Check if product ID exists.
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound(); // Check if product exists.
            }

            return View(product); // Delete product from memory.
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")] // Set POST process for action Delete.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Ensure product still exists.
        {
            var product = await _productRepository.GetByIdAsync(id);
            await _productRepository.DeleteAsync(product); //Delete product from data base.
            return RedirectToAction(nameof(Index));
        }
    }
}
