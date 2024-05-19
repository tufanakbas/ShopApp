﻿using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Abstract
{
    public interface ICartDal : IRepository<Cart>
    {
		void ClearCart(int cartId);
		void DeleteFromCart(int cartId, int productId);
        Cart GetByUserId(string userId);
    }
}
