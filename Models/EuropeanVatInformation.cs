using System.Net;
using System.Xml;

namespace partner_aluro.Models
{
    public class EuropeanVatInformation
    {
        private EuropeanVatInformation() { }

        public string CountryCode { get; private set; }
        public string VatNumber { get; private set; }
        public string Address { get; private set; }
        public string Name { get; private set; }
        public override string ToString() => CountryCode + " " + VatNumber + ": " + Name + ", " + Address.Replace("\n", ", ");

        public static EuropeanVatInformation Get(string countryCodeAndVatNumber)
        {
            if (countryCodeAndVatNumber == null)
                throw new ArgumentNullException(nameof(countryCodeAndVatNumber));

            if (countryCodeAndVatNumber.Length < 3)
                return null;

            return Get(countryCodeAndVatNumber.Substring(0, 2), countryCodeAndVatNumber.Substring(2));
        }

        public static EuropeanVatInformation Get(string countryCode, string vatNumber)
        {
            if (countryCode == null)
                throw new ArgumentNullException(nameof(countryCode));

            if (vatNumber == null)
                throw new ArgumentNullException(nameof(vatNumber));

            countryCode = countryCode.Trim();
            vatNumber = vatNumber.Trim().Replace(" ", string.Empty);

            const string ns = "urn:ec.europa.eu:taxud:vies:services:checkVat:types";
            const string url = "http://ec.europa.eu/taxation_customs/vies/services/checkVatService";
            const string xml = @"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'><s:Body><checkVat xmlns='" + ns + "'><countryCode>{0}</countryCode><vatNumber>{1}</vatNumber></checkVat></s:Body></s:Envelope>";

            try
            {
                using (var client = new WebClient())
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(client.UploadString(url, string.Format(xml, countryCode, vatNumber)));
                    var response = doc.SelectSingleNode("//*[local-name()='checkVatResponse']") as XmlElement;
                    if (response == null || response["valid", ns]?.InnerText != "true")
                        return null;

                    var info = new EuropeanVatInformation
                    {
                        CountryCode = response["countryCode", ns].InnerText,
                        VatNumber = response["vatNumber", ns].InnerText,
                        Name = response["name", ns]?.InnerText,
                        Address = response["address", ns]?.InnerText
                    };
                    return info;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
