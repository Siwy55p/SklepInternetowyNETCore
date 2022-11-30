using Microsoft.EntityFrameworkCore;
using partner_aluro.Services.Interfaces;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace partner_aluro.Models
{
    public class MetodyPlatnosci
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public string Opis { get; set; }
        public string SzczegolowyOpis { get; set; }
    }

}
