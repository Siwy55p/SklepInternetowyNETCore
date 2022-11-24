using System.ComponentModel.DataAnnotations;

namespace partner_aluro.Models
{
    public class Setting
    {
        [Key]
        public int SettingID { get; set; }
        public int? SliderHome1 { get; set; }
        public int? SliderHome2 { get; set; }
        public int? SliderHome3 { get; set; }

    }
}
