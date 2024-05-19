using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Concrete.EfCore
{
	public class EfCoreProductDal : EfCoreGenericRepository<Product, ShopContext>, IProductDal
	{
        public Product GetByIdWithCategories(int id)
        {
            using (var context = new ShopContext())
            {
                return context.Products
                    .Where(p => p.Id == id)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(id => id.Category)
                    .FirstOrDefault();
            }
        }

        public int GetCountByCategory(string category)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.AsQueryable();
                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                        .Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Category)
                        .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                }
                return (int)products.Count();
            }
        }

        public IEnumerable<Product> GetPopularProducts()
		{
			throw new NotImplementedException();
		}

        public Product GetProductDetails(int id)
        {
		using (var context = new ShopContext())
			{
				return context.Products
					.Where(p => p.Id == id)
					.Include(p => p.ProductCategories)
					.ThenInclude(p => p.Category)
					.FirstOrDefault();
			}
		}

        public List<Product> GetProductsByCategory(string category, int page, int pageSize)
        {
            using(var context = new ShopContext())
			{
				var products = context.Products.AsQueryable();
				if (!string.IsNullOrEmpty(category))
				{
					products = products
						.Include(i => i.ProductCategories)
						.ThenInclude(i => i.Category)
						.Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
				}
				return products.Skip((page - 1)*pageSize).Take(pageSize).ToList();
			}
        }

		public void Update(Product entity, int[] categoryIds)
		{
			using (var context = new ShopContext())
			{
                var product = context.Products
                    .Include(i => i.ProductCategories)
                    .FirstOrDefault(i=>i.Id == entity.Id);
				if (product != null)
				{
					product.Name = entity.Name;
					product.Description = entity.Description;
					product.Price = entity.Price;
					product.ImageUrl = entity.ImageUrl;

					product.ProductCategories = categoryIds.Select(i => new ProductCategory()
					{
						CategoryId = i,
						ProductId = entity.Id,
					}).ToList();
					context.SaveChanges();
				}
			}
		}
	}
}
