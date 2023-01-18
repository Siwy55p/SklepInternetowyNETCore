using partner_aluro.Core.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? NrZamowienia { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();

        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderTotal { get; set; }
        public DateTime OrderPlaced { get; set; }

        public string? MessageToOrder { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? RabatZamowienia { get; set; }

        public string UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        [Display(Name = "User")]
        [InverseProperty("Orders")]
        public virtual ApplicationUser? User { get; set; }


        public StanZamowienia StanZamowienia { get; set; }

        public string? Komentarz { get; set; }

        public string? MetodaDostawy { get; set; }
        public string? MetodaPlatnosci { get; set; }

        public bool AdresDostawyInny { get; set; } = true;

        public Adress1rozliczeniowy adresRozliczeniowy { get; set; }


        public Adress2dostawy AdressDostawy { get; set; }



    }
    //public enum MetodaPlatnosci
    //{
    //    Przelew,
    //    [Display(Name = "Gotówka")]
    //    Gotowka
    //}

    public enum StanZamowienia
    {
        Anulowane,
        Nowe,
        [Display(Name = "W realizacji")]
        Wrealizacji,
        Zrealizowane,
        Wysłane
    }
}