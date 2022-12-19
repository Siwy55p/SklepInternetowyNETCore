using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Reflection;

namespace partner_aluro.wwwroot.Resources
{
    public class LanguageService
    {
        private readonly IStringLocalizer _localizer;
        public LanguageService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);

            _localizer = factory.Create("SharedResource", assemblyName.Name);
        }

        public LocalizedString Getkey(string key)
        {
            return _localizer[key];
        }

        public LocalizedString GetLocalizedHtmlString(string key, string parameter)
        {
            return _localizer[key, parameter];
        }

        public decimal GetCurrently(decimal value, string CultureName)
        {
            if (CultureName == "pl-PL")
            {
                //var val = _currentCulture.IsNeutralCulture;
                //value = value;

            }
            else
            {
                value = value / Core.Constants.Eur;
            }

            return value;
        }
        public decimal GetCurrently(decimal value, NumberFormatInfo myNumberFormatInfo)
        {

            if (myNumberFormatInfo.CurrencySymbol == "zł")
            {
                //var val = _currentCulture.IsNeutralCulture;
                //value = value;

            }
            else if (myNumberFormatInfo.CurrencySymbol == "€")
            {
                value = value / Core.Constants.Eur;
            }

            return value;
        }
        public decimal GetCurrently(decimal value, CultureInfo myNumberFormatInfo)
        {

            if (myNumberFormatInfo.NumberFormat.CurrencySymbol == "zł")
            {
                //var val = _currentCulture.IsNeutralCulture;
                //value = value;

            }
            else if (myNumberFormatInfo.NumberFormat.CurrencySymbol == "€")
            {
                value = value / Core.Constants.Eur;
            }

            return value;
        }
    }
}
