using ShopApp.Entities;
using System.Collections.Generic;

namespace ShopApp.WebUI.Models
{
	public class ProductListModel
	{
		public IEnumerable<Product> Products { get; set; }
	}
}
