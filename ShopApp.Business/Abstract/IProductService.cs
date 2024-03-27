﻿using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Business.Abstract
{
	public interface IProductService
	{
		Product GetById(int id);
		Product GetProductDetails(int id);
		List<Product> GetAll();
		List<Product> GetPopularProducts();
		void Create(Product entity);
		void Update(Product entity);
		void Delete(Product entity);
	}
}
