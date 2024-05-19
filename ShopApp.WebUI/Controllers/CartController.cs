using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopApp.WebUI.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		private ICartService _cartService;
		private IOrderService _orderService;
		private UserManager<ApplicationUser> _userManager;
		public CartController(ICartService cartService, UserManager<ApplicationUser> userManager, IOrderService orderService)
		{
			_cartService = cartService;
			_userManager = userManager;
			_orderService = orderService;
		}
		public IActionResult Index()
		{
			var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));
			return View(new CartModel()
			{
				CartId = cart.Id,
				CartItems = cart.CartItems.Select(i => new CartItemModel()
				{
					CartItemId = i.Id,
					ProductId = i.Product.Id,
					Name = i.Product.Name,
					Price = (decimal)i.Product.Price,
					ImageUrl = i.Product.ImageUrl,
					Quantity = i.Quantity
				}).ToList()
			});
		}
		[HttpPost]
		public IActionResult AddToCart(int productId, int quantity)
		{
			_cartService.AddToCart(_userManager.GetUserId(User), productId, quantity);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult DeleteFromCart(int productId)
		{
			_cartService.DeleteFromCart(_userManager.GetUserId(User), productId);
			return RedirectToAction("Index");
		}

		public IActionResult Checkout()
		{
			var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

			var orderModel = new OrderModel();

			orderModel.CartModel = new CartModel()
			{
				CartId = cart.Id,
				CartItems = cart.CartItems.Select(i => new CartItemModel()
				{
					CartItemId = i.Id,
					ProductId = i.Product.Id,
					Name = i.Product.Name,
					Price = (decimal)i.Product.Price,
					ImageUrl = i.Product.ImageUrl,
					Quantity = i.Quantity
				}).ToList()
			};
			return View(orderModel);
		}
		[HttpPost]
		public IActionResult Checkout(OrderModel orderModel)
		{
			if (!ModelState.IsValid)
			{
				return View(orderModel);
			}
			var userId = _userManager.GetUserId(User);
			var cart = _cartService.GetCartByUserId(userId);
			orderModel.CartModel = new CartModel()
			{
				CartId = cart.Id,
				CartItems = cart.CartItems.Select(i => new CartItemModel()
				{
					CartItemId = i.Id,
					ProductId = i.Product.Id,
					Name = i.Product.Name,
					Price = (decimal)i.Product.Price,
					ImageUrl = i.Product.ImageUrl,
					Quantity = i.Quantity
				}).ToList()
			};
			SaveOrder(orderModel, userId);
			ClearCart(cart.Id);
			return View("Success");
		}

		private void ClearCart(int cartId)
		{
			_cartService.ClearCart(cartId);
		}

		private void SaveOrder(OrderModel orderModel, string userId)
		{
			var order = new Order();
			order.OrderNumber = new Random().Next(111111, 999999).ToString();
			order.OrderState = EnumOrderState.Completed;
			order.PaymentTypes = EnumPaymentTypes.CreditCart;
			order.PaymentId = Guid.NewGuid().ToString();

			order.OrderDate = DateTime.Now;
			order.FirstName = orderModel.FirstName;
			order.LastName = orderModel.LastName;
			order.Email = orderModel.Email;
			order.Phone = orderModel.Phone;
			order.City = orderModel.City;
			order.Address = orderModel.Address;
			order.UserId = userId;

			foreach (var item in orderModel.CartModel.CartItems)
			{
				var orderItem = new OrderItem()
				{
					Price = item.Price,
					Quantity = item.Quantity,
					ProductId = item.ProductId,

				};
				order.OrderItems.Add(orderItem);
			}
			_orderService.Create(order);
		}

		public IActionResult GetOrders()
		{
			var orders = _orderService.GetOrders(_userManager.GetUserId(User));
			var orderListModel = new List<OrderListModel>();
			OrderListModel orderModel;

			foreach (var order in orders)
			{
				orderModel = new OrderListModel();
				orderModel.OrderId = order.Id;
				orderModel.OrderNumber	= order.OrderNumber;
				orderModel.OrderDate = order.OrderDate;
				orderModel.OrderNote = order.OrderNote;
				orderModel.FirstName = order.FirstName;
				orderModel.LastName = order.LastName;
				orderModel.Email = order.Email;
				orderModel.Phone = order.Phone;
				orderModel.City = order.City;
				orderModel.Address = order.Address;
				orderModel.City = order.City;

				orderModel.OrderItems = order.OrderItems.Select(i => new OrderItemModel()
				{
					OrderItemId = i.Id,
					Name = i.Product.Name,
					Price = i.Price,
					Quantity = i.Quantity,
					ImageUrl = i.Product.ImageUrl,
				}).ToList();

				orderListModel.Add(orderModel);
			}
			return View(orderListModel);
		}
	}
}
