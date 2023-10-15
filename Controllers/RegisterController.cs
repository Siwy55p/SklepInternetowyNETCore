using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Padi.Vies;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Xml;
using System.Net.Http;
using System.Xml;
using System.Net.Http.Headers;

namespace partner_aluro.Controllers
{
    public class RegisterController : Controller
    {
        private readonly RegonService RegonService;

        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IEmailSender _emailSender;
        private readonly IEmailService _emailService;


        private readonly ApplicationDbContext _context;

        public RegisterController(RegonService regonService, IEmailService emailService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            //ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol
            //                                        | SecurityProtocolTypeExtensions.Tls12;

            RegonService = regonService;
            _userManager = userManager;
            _emailService = emailService;
            _context = context;
        }
        [Route("Login")]
        [Route("Identity/Account")]
        [Route("logowanie")]
        public IActionResult LoginRedirect(string ReturnUrl)
        {
            return Redirect("Identity/Account/Login?ReturnUrl=" + ReturnUrl);
        }
        [Route("pl/logowanie")]
        [Route("en/logowanie")]
        public IActionResult LoginRedirect2(string ReturnUrl)
        {
            return Redirect("../Identity/Account/Login?ReturnUrl=" + ReturnUrl);
        }
        public IActionResult Index()
        {
            return Redirect("Identity/Account/Login?ReturnUrl=" + "");
        }

        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult ChangeValue(string culture)
        {

            if (culture == "zl")
            {
                NumberFormatInfo myNumberFormatInfo1 = new CultureInfo("pl-PL", false).NumberFormat;
                Core.Constants.myNumberFormatInfo = myNumberFormatInfo1;
            }
            else
            {
                NumberFormatInfo myNumberFormatInfo2 = new CultureInfo("de-DE", false).NumberFormat;
                Core.Constants.myNumberFormatInfo = myNumberFormatInfo2;
            }


            return Redirect(Request.Headers["Referer"].ToString());
        }

        //https://localhost:44315/Register/unsubscribe

        //https://localhost:44315/Register/unsubscribe/?Email=marcin@aluro.pl
        public async Task<IActionResult> Unsubscribe(string Email)
        {
            //var user = await _userManager.FindByEmailAsync(Email);

            var user = _context.Users.Where(x => x.UserName == Email).FirstOrDefault();
            user.Newsletter = false;
            _context.Users.Update(user);
            _context.SaveChanges();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> ResetPassSend(string Email)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Email);
                //if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                if (user == null )
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("/Identity/Account/ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);



                EmailDto newClint = new EmailDto()
                {
                    Subject = "Reset hasła",
                    To = Email,
                    Body = $"<table style=\"background-color: #ececec; font-family: Arial, sans-serif; width: 100%;\">\r\n<tbody>\r\n<tr>\r\n<td>\r\n<table class=\"newsletter-pro-content\" style=\"margin: 1% auto; width: 646px; background-color: rgb(255, 255, 255); height: 549.25px;\" align=\"center\">\r\n<tbody>\r\n<tr style=\"height: 549.25px;\">\r\n<td style=\"height: 549.25px;\">\r\n<table style=\"border-collapse: collapse; width: 100%; height: 402.719px;\" border=\"0\">\r\n<tbody>\r\n<tr style=\"height: 181.391px;\">\r\n<td style=\"text-align: center; height: 181.391px;\">\r\n<p><span style=\"font-family: Arial, Helvetica, sans-serif;\"><img src=\"http://www.partner.aluro.pl/img/cms/log-png.png\" alt=\"\" width=\"358\" height=\"211\"></span></p>\r\n</td>\r\n</tr>\r\n<tr style=\"height: 159.953px;\">\r\n<td style=\"text-align: left; height: 159.953px;\" align=\"center\">\r\n<p style=\"text-align: center;\"><strong><span style=\"font-family: Arial, Helvetica, sans-serif;\">Twoje konto zostało aktywnowane.</span></strong></p>\r\n<p style=\"text-align: center;\"><span style=\"font-family: Arial, Helvetica, sans-serif;\">Proszę zresetuj swoje hasło: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> klikacjąc tutaj.</a></p>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>"
                };

                _emailService.SendEmailAsync(newClint); //Bardzo specjalnie tak jest jak jest zrobione. Musi tak zostać.

                //await _emailSender.SendEmailAsync(
                //    Input.Email,
                //    "Reset Password",
                //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToPage("/Identity/Account/ForgotPasswordConfirmation");
            }

            return RedirectToPage("/Identity/Account/ForgotPasswordConfirmation");
        }


        [HttpPost]
        public async Task<List<string>> SprawdzNIPAsync(string? Vat = null)
        {
            var komunikat = "Brak danych";
            var Regon = "";
            var Wojewodztwo1 = "";
            var Powiat1 = "";
            var Ulica1 = "";
            var nazwa_firmy = "Nie znaleziono takiej firmy";
            var Kraj1 = "PL";
            var adres = "";
            var Miasto1 = "";
            var KodPocztowy1 = "";


            //document.getElementById("komunikat").value = result[0];
            //document.getElementById("NazwaFirmy").value = result[1];
            //document.getElementById("adres").value = result[2];
            //document.getElementById("miasto").value = result[3];
            //document.getElementById("kod_pocztowy").value = result[4];

            //nie sprawdzaj po vat
            
            EuropeanVatInformation companyEuropa = EuropeanVatInformation.Get(Vat);

            if (companyEuropa != null)
            {
                string SplitAdres = companyEuropa.Address;

                string[] AdresSplit = SplitAdres.Split("\n");
                string ulicaS = AdresSplit[0].ToString();
                string kod_pocztowyS = AdresSplit[1].ToString();
                string krajE = AdresSplit[2].ToString();

                string KodPocztowyZagraniczny = new String(kod_pocztowyS.Where(Char.IsDigit).ToArray());
                var MiastoS = new String(kod_pocztowyS.Where(Char.IsLetter).ToArray());

                komunikat = "Aktywne";
                Regon = companyEuropa.VatNumber;
                nazwa_firmy = companyEuropa.Name;
                adres = ulicaS;
                Kraj1 = companyEuropa.CountryCode + " " + krajE;
                Miasto1 = MiastoS.ToString();
                KodPocztowy1 = KodPocztowyZagraniczny;


                List<string> list1 = new List<string>();

                list1.Add(komunikat);
                list1.Add(nazwa_firmy);
                list1.Add(adres);
                list1.Add(Miasto1);
                list1.Add(KodPocztowy1);

                return list1;
            }else if(companyEuropa == null)
            { 
                CompanyModel _model = new CompanyModel();

                _model.Vat = Vat;
                _model = await RegonService.GetCompanyDataByNipAsync(_model.Vat);

                komunikat = "Brak danych";
                nazwa_firmy = "Nie znaleziono takiej firmy";
                adres = "";
                Miasto1 = "";
                KodPocztowy1 = "";

                if (_model.Errors.Count > 0)
                {
                    komunikat = _model.Errors[0].ErrorMessagePl;

                }
                else
                {
                    Regon = _model.Regon;
                    Wojewodztwo1 = _model.Wojewodztwo;
                    nazwa_firmy = _model.Name;
                    adres = _model.Ulica;
                    Miasto1 = _model.Miejscowosc;
                    KodPocztowy1 = _model.KodPocztowy;
                }
            }
            
            
            if(nazwa_firmy == "Nie znaleziono takiej firmy")
            {
                var listaGetVat = await GetVatInfo3Async(Vat);
                return listaGetVat;

            }


            List<string> list = new List<string>();

            list.Add(komunikat);
            list.Add(nazwa_firmy);
            list.Add(adres);
            list.Add(Miasto1);
            list.Add(KodPocztowy1);
            
            //list.Add(Regon);
            //list.Add(Wojewodztwo1);
            //list.Add(Powiat1);
            //list.Add(Ulica1);
            //list.Add(komunikat);
            //list.Add(nazwa_firmy);
            //list.Add(adres);
            //list.Add(Kraj1);
            //list.Add(Miasto1);
            //list.Add(KodPocztowy1);
            return list;
        }

        public async Task<string> GetCompanyInfo(string vatNumber)
        {
            string apiUrl = "http://ec.europa.eu/taxation_customs/vies/services/checkVatService";

            // create HTTP client and request content
            using (var httpClient = new HttpClient())
            using (var content = new StringContent($"<?xml version=\"1.0\" encoding=\"UTF-8\"?><SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ns1=\"urn:ec.europa.eu:taxud:vies:services:checkVat:types\"><SOAP-ENV:Body><ns1:checkVat><ns1:countryCode>{vatNumber.Substring(0, 2)}</ns1:countryCode><ns1:vatNumber>{vatNumber.Substring(2)}</ns1:vatNumber></ns1:checkVat></SOAP-ENV:Body></SOAP-ENV:Envelope>", Encoding.UTF8, "text/xml"))
            {
                // set request headers
                content.Headers.ContentType = new MediaTypeHeaderValue("text/xml;charset=UTF-8");
                content.Headers.Add("SOAPAction", "urn:ec.europa.eu:taxud:vies:services:checkVat/checkVat");

                // send request
                var response = await httpClient.PostAsync(apiUrl, content);

                // parse XML response
                var responseContent = await response.Content.ReadAsStringAsync();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseContent);
                var nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
                nsManager.AddNamespace("ns", "urn:ec.europa.eu:taxud:vies:services:checkVat:types");
                var valid = bool.Parse(xmlDoc.SelectSingleNode("//ns:valid", nsManager).InnerText);
                var companyName = xmlDoc.SelectSingleNode("//ns:name", nsManager).InnerText;
                var companyAddress = xmlDoc.SelectSingleNode("//ns:address", nsManager).InnerText;

                // return company info as string
                return $"{(valid ? "Valid VAT number" : "Invalid VAT number")}\nCompany Name: {companyName}\nCompany Address: {companyAddress}";
            }
        }

        public async Task<List<string>> GetVatInfo3Async(string vatNumber)
        {
            /*
             * https://vatlayer.com/dashboard?logged_in=1 
             */
            var accessKey1 = "384438237950f46ed363afd151757d85";
            var accessKey = "38dfee3f31277dcf10adcabddb33e249";

            var url = $"http://apilayer.net/api/validate?access_key={accessKey}&vat_number={vatNumber}";

                List<string> list = new List<string>();

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                        if (result!=null && result.success==true)
                        {
                            string name = result.company_name;
                            string adres = result.company_address;
                            string vat = result.vat_number;

                            string SplitAdres = adres;

                            string[] AdresSplit = SplitAdres.Split(",");
                            string KodPocztowyMiasto = AdresSplit[0].ToString();

                            string[]KodPocztowyiMiasto = KodPocztowyMiasto.Split(" ");
                            string KodPocztowy = KodPocztowyiMiasto[0].ToString();

                            string Miasto = "";
                            for (int i = 1; i < KodPocztowyiMiasto.Length; i++)
                            {
                                Miasto += KodPocztowyiMiasto[i].ToString() + " ";
                            }

                            string UlicaNrDomu = AdresSplit[1].ToString();
                            string krajE = AdresSplit[2].ToString();

                        list.Add("Aktywne");
                        list.Add(name);
                        list.Add(UlicaNrDomu);
                        list.Add(Miasto);
                        list.Add(KodPocztowy);
                        list.Add(krajE);

                        return list;

                        }else
                        {
                            list.Add("Usługa nie dostępna, proszę spróbować później.");
                            list.Add("Chwili obecnej nie możemy poprawnie sprawdzić VAT");
                            return list;
                        }

                    }
                    else
                    {
                        list.Add("Usługa nie dostępna, proszę spróbować później.");
                        list.Add("Przekroczono limit");
                        return list;

                    }
                }
        }


        [HttpPost]
        public async Task<List<string>> SprawdzNIPAdminAddUser(string? Vat = null)
        {
            CompanyModel _model = new CompanyModel();

            _model.Vat = Vat;
            if (_model.Vat != null)
            {
                _model = await RegonService.GetCompanyDataByNipAsync(_model.Vat);
            }
            else
            {
                _model = await RegonService.GetCompanyDataByNipAsync(Vat);
            }

            var komunikat = "Brak danych";
            var NazwaFirmy = "Nie znaleziono takiej firmy";

            var Regon = "";
            var Wojewodztwo1 = "";
            var Powiat1 = "";
            var Ulica1 = "";
            var Kraj1 = "Polska";
            var Miasto1 = "";
            var KodPocztowy1 = "";

            //var Ulica2 = "";
            //var Kraj2 = "";
            //var Miasto2 = "";
            //var KodPocztowy2 = "";

            if (_model.Errors.Count > 0)
            {
                komunikat = _model.Errors[0].ErrorMessagePl;
            }

            if (_model != null && _model.Errors.Count <= 0 && _model.Name != "")
            {
                NazwaFirmy = _model.Name;
                Regon = _model.Regon;
                Wojewodztwo1 = _model.Wojewodztwo;
                Powiat1 = _model.Powiat;
                Ulica1 = _model.Ulica;

                Miasto1 = _model.Miejscowosc;
                KodPocztowy1 = _model.KodPocztowy;
            }

            EuropeanVatInformation companyEuropa = EuropeanVatInformation.Get(Vat);

            if (companyEuropa != null)
            {
                string SplitAdres = companyEuropa.Address;

                string[] AdresSplit = SplitAdres.Split("\n");
                string ulicaS = AdresSplit[0].ToString();
                string kod_pocztowyS = AdresSplit[1].ToString();
                string krajE = AdresSplit[2].ToString();

                string KodPocztowyZagraniczny = new String(kod_pocztowyS.Where(Char.IsDigit).ToArray());
                var MiastoS = new String(kod_pocztowyS.Where(Char.IsLetter).ToArray());

                NazwaFirmy = companyEuropa.Name;
                Regon = companyEuropa.VatNumber;
                Wojewodztwo1 = companyEuropa.CountryCode;
                Powiat1 = companyEuropa.CountryCode;
                Ulica1 = ulicaS;
                Kraj1 = krajE;
                Miasto1 = MiastoS;
                KodPocztowy1 = KodPocztowyZagraniczny;
            }
                
            List<string> list = new List<string>();

            list.Add(komunikat);
            list.Add(NazwaFirmy);
            list.Add(Regon);
            list.Add(Wojewodztwo1);
            list.Add(Powiat1);
            list.Add(Ulica1);
            list.Add(Kraj1);
            list.Add(Miasto1);
            list.Add(KodPocztowy1);


            return list;
        }
    }
}
