﻿using EasyEventSourcing.Domain.Read.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EasyEventSourcing.Domain.Messages;
using EasyEventSourcing.Domain.Read.Products;

namespace EasyEventSourcing.Ui.Api.Controllers
{
    [RoutePrefix("api/baskets")]
    public class BasketsController : BaseController
    {
        [HttpPost]
        [Route("init")]
        public void InitBasket()
        {
            InitClientId();
        }

        [HttpGet]
        [Route("")]
        public BasketReadModel Get(DateTime timestamp)
        {
            InitClientId();

            var carts = App.Read<BasketRequest, BasketsReadModel>(new BasketRequest { Timestamp = timestamp });
            var cart = carts.GetCart(ClientId);
            return cart;
        }

        [HttpGet]
        [Route("")]
        public BasketReadModel Get()
        {
            InitClientId();

            var carts = App.Read<BasketRequest, BasketsReadModel>(new BasketRequest { Timestamp = null });
            var cart = carts.GetCart(ClientId);
            return cart;
        }

        [HttpPut]
        [Route("")]
        public int Put(ProductReadModel product)
        {
            InitClientId();

            var carts = App.Read<BasketRequest, BasketsReadModel>(new BasketRequest());
            App.Send(new AddProductToCart { ProductId = product.ProductId, Quantity = 1, CartId = carts.GetCart(ClientId).CartId });

            return carts.GetCart(ClientId).Items.FirstOrDefault(item => item.ProductId == product.ProductId)?.Quantity ?? 0;
        }

        [HttpGet]
        [Route("events")]
        public ICollection<DateTime> GetEvent()
        {
            InitClientId();

            var carts = App.Read<BasketRequest, BasketsReadModel>(new BasketRequest());
            var cart = carts.GetCart(ClientId);
            return cart.Timestamps;
        }
    }
}
