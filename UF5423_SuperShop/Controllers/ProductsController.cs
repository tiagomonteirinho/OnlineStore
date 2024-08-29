using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UF5423_SuperShop.Data;
using UF5423_SuperShop.Data.Entities;

namespace UF5423_SuperShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IRepository _repository;

        public ProductsController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_repository.GetProducts());
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _repository.GetProduct(id.Value);
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
        public async Task<IActionResult> Create(/*[Bind("ProductId,ProductName,ProductPrice,ProductImageUrl,ProductLastPurchase,ProductLastSale,ProductIsAvailable,ProductStock")]*/ Product product)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateProduct(product);
                await _repository.SaveAllAsync();
                //return RedirectToAction("Index"); // Define custom action name.
                return RedirectToAction(nameof(Index)); // Redirect to products list action.
            }
            return View(product); // Keep input changes even if product is invalid.
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _repository.GetProduct(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product) // Gets product ID seperately.
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.UpdateProduct(product);
                    await _repository.SaveAllAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repository.ProductExists(product.ProductId))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) // Check if product ID exists.
            {
                return NotFound();
            }

            var product = _repository.GetProduct(id.Value);
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
            var product = _repository.GetProduct(id);
            _repository.RemoveProduct(product); //Delete product from data base.
            await _repository.SaveAllAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
