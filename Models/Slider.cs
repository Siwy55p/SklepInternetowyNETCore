
using Microsoft.EntityFrameworkCore;
using partner_aluro.Services.Interfaces;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;


namespace partner_aluro.Models
{

    public class Slider
    {
        [Key]
        public int ImageSliderID { get; set; }

        public string Name { get; set; }

        public virtual ImageModel? ObrazekSlidera { get; set; } = new ImageModel();
        //public IFormFile? FrontImage { get; set; }

        public virtual List<ImageModel>? ObrazkiDostepneWSliderze { get; set; } = new List<ImageModel>();


    }
}
