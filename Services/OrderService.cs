﻿
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System.Reflection;
using System.Xml.Linq;

namespace partner_aluro.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
        public OrderItem GetItem(int id)
        {
            var orderItem = _context.OrderItems.Find(id);

            return orderItem;
        }

        public async Task<Order> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(u => u.User)
                .Include(a1=> a1.adresRozliczeniowy)
                .Include(a2=> a2.AdressDostawy)
                .FirstOrDefaultAsync(o=> o.Id == id);

            var OrderItems = await _context.OrderItems.Where(o => o.OrderId == id).ToListAsync();
            order.OrderItems = OrderItems;



            return order;
        }

        public Adress1rozliczeniowy GetUserAdress1(string UserID)
        {
            List<Adress1rozliczeniowy> ListaAdresow = _context.Adress1rozliczeniowy.ToList();
            Adress1rozliczeniowy adress1 = new Adress1rozliczeniowy();

            foreach (var adres in ListaAdresow)
            {
                if (adres.UserID == UserID)
                {
                    adress1 = adres;
                    break;
                }
            }


            return adress1;
        }

        public Adress2dostawy GetUserAdress2(string UserID)
        {
            List<Adress2dostawy> ListaAdresow = _context.Adress2dostawy.ToList();
            Adress2dostawy adress2 = new Adress2dostawy();

            foreach (var adres in ListaAdresow)
            {
                if (adres.UserID == UserID)
                {
                    adress2 = adres;
                    break;
                }
            }


            return adress2;
        }

        public List<OrderItem> List()
        {
            var OrderItems = _context.OrderItems.ToList();
            return OrderItems;
        }

        public List<OrderItem> List(int id)
        {
            var OrderItem = _context.OrderItems.ToList();

            List<OrderItem> OrderId = new List<OrderItem>();

            foreach(var item in OrderItem)
            {
                if(item.OrderId == id)
                {
                    OrderId.Add(item);
                }
            }

            var ListaProduktow = _context.Products.ToList();

            foreach (var itemOrd in OrderItem)
            {
                foreach (var produkt in ListaProduktow)
                {
                    if (itemOrd.ProductId == produkt.ProductId)
                    {
                        itemOrd.Product = produkt;
                    }
                }
            }

            return OrderId;
        }

        public List<Order> ListOrdersAll()
        {
            List<Order> listaZamowien = _context.Orders
                .Include(user => user.User).ToList();

            return listaZamowien;
        }

        public List<Order> ListOrdersUser(string UserID)
        {
            List<Order> listaZamowienUzytkownika = _context.Orders.Where(u => u.UserID == UserID).Include(user => user.User).ToList();
            return listaZamowienUzytkownika;
        }


        public Order Update(Order order)
        {

            _context.Update(order);
            _context.SaveChanges();

            return order;
        }

    }
}
