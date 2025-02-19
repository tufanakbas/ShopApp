﻿using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Business.Concrete
{
	public class OrderManager : IOrderService
	{
		private readonly IOrderDal _orderDal;
        public OrderManager(IOrderDal orderDal)
        {
            _orderDal= orderDal;
        }
        public void Create(Order entity)
		{
			_orderDal.Create(entity);
		}

		public List<Order> GetOrders(string userId)
		{
			return _orderDal.GetOrders(userId);
		}
	}
}
