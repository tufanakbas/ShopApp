﻿using ShopApp.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
	public class ProductModel
	{
		public int Id { get; set; }

		[Required]
		[StringLength(60,MinimumLength =10,ErrorMessage ="Ürün ismi minimum 10 karakter ve maksimum 60 karakter olmalıdır")]
		public string Name { get; set; }

		[Required]
		public string ImageUrl { get; set; }

		[Required]
		[StringLength(10000, MinimumLength = 20, ErrorMessage = "Ürün ismi minimum 20 karakter ve maksimum 100 karakter olmalıdır")]
		public string Description { get; set; }

		[Required(ErrorMessage ="Fiyat Belirtiniz")]
		[Range(1,100000)]
		public decimal? Price { get; set; }
		public List<Category> SelectedCategories { get; set; }
	}
}
