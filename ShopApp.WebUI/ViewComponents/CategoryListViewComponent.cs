﻿using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.Models;
using System.Net;

namespace ShopApp.WebUI.ViewComponents
{
	public class CategoryListViewComponent:ViewComponent
	{
		private ICategoryService _categoryService;
        public CategoryListViewComponent(ICategoryService categoryService)
        {
			_categoryService = categoryService;
        }
        public IViewComponentResult Invoke() 
		{
			return View(new CategoryListViewModel()
			{
				SelectedCategory = RouteData.Values["category"]?.ToString(),
				Categories = _categoryService.GetAll()
			}) ; 
		}
	}
}
