namespace partner_aluro.Models
{
    public class ContactPrestashop
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int id_shop_group { get; set; }

        public int id_shop { get; set; }

        public int id_gender { get; set; }

        public int id_default_group { get; set; }

        public int id_id_lang { get; set; }

        public int id_risk { get; set; }

        public string company { get; set; }

        public string email { get; set; }

        public string passwd { get; set; }

        public string website { get; set; }

        public string secure_key { get; set; }

        public string note { get; set; }

        public int active { get; set; }

        public int is_quest { get; set; }

        public int deleted { get; set; }


    }
}
