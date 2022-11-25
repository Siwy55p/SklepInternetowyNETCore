using partner_aluro.Models;
using Quartz;
using System.Globalization;
using System.Text.Encodings.Web;

namespace partner_aluro.Core
{
    public static class Constants
    {

        public static class Roles
        {
            public const string Administrator = "Administrator";
            public const string Manager = "Manager";
            public const string User = "User";
        }

        public static class Policies
        {
            public const string RequireAdmin = "RequireAdmin";
            public const string RequireManager = "RequireManager";
        }

        public static async Task<byte[]> DownloadFile(string url)
        {
            using (var client = new HttpClient())
            {

                using (var result = await client.GetAsync(url))
                {
                    if (result.IsSuccessStatusCode)
                    {

                        return await result.Content.ReadAsByteArrayAsync();
                    }

                }
            }
            return null;
        }

        public static NumberFormatInfo myNumberFormatInfo = new CultureInfo("pl-PL", false).NumberFormat;

        public static Kurs Euro { get; set; } = new Kurs();

        public static decimal Eur = (decimal)4.69;
        public static decimal Zloty = (decimal)1;

        public static decimal Vat = (decimal)1.23;

        public static string UserId;

        public static decimal Rabat;
        public static int SliderHome1;
        public static int SliderHome2;
        public static int SliderHome3;

        public static string ResetMessageEmail = $"Wiadomosc.";

        public static string RegisterNewAccoutMessageEmailSubject = $"Dziękujemy za rejestrację.";
        public static string RegisterNewAccoutMessageEmail = $"" +
            $"<p style='text-align: center;'>Dziękujemy za rejestrację nowego konta w systemie platformy hurtowej B2B marki ALURO.<br><br>Po weryfikacji danych, otrzymają Państwo dostęp do platformy hurtowej<br>z możliwością zakup&oacute; w w cenach hurtowych.<br><br>Zazwyczaj proces weryfikacji trwa od 1 do 12 godzin, <br>dziękujemy za cierpliwość.</p>";

        public static string ActiveNewAccoutMessageEmail = $"Dziękujemy za cierpliwość. <br>Twoje konto zostało aktywowane, możesz korzystać z naszych usług.<b>Możesz zalogować się do platwormy, lub <a href='#'>resetuj hasło</a> jeśli zapomniałeś hasła.<br>";

    }
}
