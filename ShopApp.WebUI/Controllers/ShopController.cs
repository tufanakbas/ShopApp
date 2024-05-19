using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
using ShopApp.Entities;
using ShopApp.WebUI.Models;
using System.Linq;
using static ShopApp.WebUI.Models.ProductListModel;

namespace ShopApp.WebUI.Controllers
{
	public class ShopController : Controller
	{
        private IProductService _productService;
        private readonly ShopContext _shopContext;
        public ShopController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Details(int? id)
		{
            if (id == null)
            {
                return NotFound();
            }
            Product product = _productService.GetProductDetails((int)id);
            if (product == null)
            {
                return NotFound();
            }
			return View(new ProductDetailsModel()
            {
                Product = product,
                Categories = product.ProductCategories.Select(c => c.Category).ToList()
            });
		}
        public IActionResult List(string category, int page = 1)
        {
            const int pageSize = 12;
            return View(new ProductListModel()
            {
                PageModel = new PageInfo() 
                {
                    TotalItems = _productService.GetCountByCategory(category),
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    CurrentCategory = category
                },
                Products = _productService.GetProductsByCategory(category, page,pageSize)
            });
        }
    }
}
