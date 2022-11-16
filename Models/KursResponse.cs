namespace partner_aluro.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class MyArray
    {
        public string table { get; set; }
        public string no { get; set; }
        public string effectiveDate { get; set; }
        public List<Rate> rates { get; set; }
    }

    public class KursResponse
    {
        public MyArray MyArray { get; set; }
        //public List<MyArray> MyArray { get; set; }
    }


}
