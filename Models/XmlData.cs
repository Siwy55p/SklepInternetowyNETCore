/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace partner_aluro
{
    [XmlRoot(ElementName = "product")]
    public class ProductXML
    {
        [XmlElement(ElementName = "symbol")]
        public string Symbol { get; set; }
        [XmlElement(ElementName = "product_name")]
        public string Product_name { get; set; }
        [XmlElement(ElementName = "images")]
        public string Images { get; set; }
        [XmlElement(ElementName = "stock")]
        public string Stock { get; set; }
        [XmlElement(ElementName = "cena_detaliczna")]
        public string Cena_detaliczna { get; set; }
        [XmlElement(ElementName = "EAN13")]
        public string EAN13 { get; set; }
        [XmlElement(ElementName = "opis")]
        public string Opis { get; set; }
        [XmlElement(ElementName = "kategoria_domyslna")]
        public string Kategoria_domyslna { get; set; }
        [XmlElement(ElementName = "szerokosc")]
        public string Szerokosc { get; set; }
        [XmlElement(ElementName = "wysokosc")]
        public string Wysokosc { get; set; }
        [XmlElement(ElementName = "glebokosc")]
        public string Glebokosc { get; set; }
        [XmlElement(ElementName = "waga_z_opakowaniem")]
        public string Waga_z_opakowaniem { get; set; }
        [XmlElement(ElementName = "cechy")]
        public string Cechy { get; set; }
    }

    [XmlRoot(ElementName = "export_products")]
    public class Export_products
    {
        [XmlElement(ElementName = "product")]
        public List<ProductXML> Product { get; set; }
    }

}
