using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Data;
using OnlineStore.Helpers;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    //[Authorize] // Prevent access to non-authenticated users. // Replace view with login view and return to previous view after authentication.
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository; // Use class interface because dependency injection already gets methods from class.
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public ProductsController(IProductRepository productRepository, IUserHelper userHelper, IImageHelper imageHelper, IConverterHelper converterHelper)
        {
            _productRepository = productRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_productRepository.GetAll().OrderBy(p => p.Name)); // Order by entity exclusive property.
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                return new NotFoundViewResult("ProductNotFound"); // Replace product not found view.
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")] // Set exclusive access to certain roles.
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
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "products"); // Upload image file and get path to save to database.
                }

                var product = _converterHelper.ConvertToProduct(model, path, true);
                product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name); // Set product user as current user.
                await _productRepository.CreateAsync(product);

                //return RedirectToAction("Index"); // Define custom action name.
                return RedirectToAction(nameof(Index)); // Redirect to products list action.
            }

            return View(model); // Keep input changes if model data is invalid.
        }

        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var model = _converterHelper.ConvertToProductViewModel(product); // Get product view model to show image file.

            return View(model);
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
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "products");
                    }

                    var product = _converterHelper.ConvertToProduct(model, path, false);
                    product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name); // Set product user as current user.
                    
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
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // Check if product ID exists.
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound"); // Check if product still exists.
            }

            return View(product); // Delete product from memory.
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")] // Set POST process for action Delete.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id); // Ensure product still exists.

            try
            {
                //throw new Exception("Test exception");
                await _productRepository.DeleteAsync(product); //Delete product from data base.
                return RedirectToAction(nameof(Index));
            }
            //catch (Exception ex) // Generic error exception.
            catch (DbUpdateException ex) // Database update table dependency error exception.
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE")) // If exception message contains 'DELETE'
                {
                    ViewBag.ErrorTitle = $"Unable to delete product.";
                    ViewBag.ErrorMessage = $"Product {product.Name} could not be deleted. Please ensure that the product is not being used in any existing orders.</br></br>";
                }

                return View("Error");
            }
        }

        public IActionResult ProductNotFound()
        {
            return View();
        }
    }
}
