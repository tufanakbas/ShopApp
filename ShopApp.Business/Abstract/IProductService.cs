﻿using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Business.Abstract
{
	public interface IProductService:IValidator<Product>
	{
		Product GetById(int id);
		Product GetProductDetails(int id);
		List<Product> GetAll();
		List<Product> GetPopularProducts();
        List<Product> GetProductsByCategory(string category, int page, int pageSize);
        bool Create(Product entity);
		void Update(Product entity);
		void Delete(Product entity);
		int GetCountByCategory(string category);
        Product GetByIdWithCategories(int id);
		void Update(Product entity, int[] categoryIds);
	}
}
