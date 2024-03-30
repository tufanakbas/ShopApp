using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace ShopApp.WebUI.Models
{
	public class ProductListModel
	{
		public class PageInfo
		{
            public int TotalItems { get; set; }
            public int ItemsPerPage { get; set; }
            public int CurrentPage { get; set; }
            public string CurrentCategory { get; set; }
            public int TotalPages()
            {
                return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
            }
        }
        public PageInfo PageModel { get; set; }
        public IEnumerable<Product> Products { get; set; }
	}
}
