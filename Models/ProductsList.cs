namespace partner_aluro.Models
{
    public class ProductsList
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string pathImageUrl250x250 { get; set; }
        public string Symbol { get; set; }
        public decimal CenaProduktuBrutto { get; set; }
        public decimal CenaProduktuDetal { get; set; }
        public Category? CategoryNavigation { get; set; }
        public int CategoryId { get; set; }
        public bool Ukryty { get; set; }
        public int Ilosc { get; set; }
    }
}
