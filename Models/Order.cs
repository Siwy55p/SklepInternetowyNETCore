using partner_aluro.Core.Repositories;
using System;
using System.Collections.Generic;

namespace partner_aluro.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
        public int OrderTotal { get; set; }
        public DateTime OrderPlaced { get; set; }

        public string? MessageToOrder { get; set; }

        public decimal? RabatZamowienia { get; set; }

        public string UserID { get; set; }
        public StanZamowienia StanZamowienia { get; set; }

        public string? Komentarz { get; set; }

        public virtual ApplicationUser User { get; set; }



    }


    public enum StanZamowienia
    {
        Nowe,
        Wrealizacji,
        Zrealizowane
    }
}