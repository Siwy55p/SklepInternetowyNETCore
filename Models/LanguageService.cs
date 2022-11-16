using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

using partner_aluro.Services;
using partner_aluro.Services.Interfaces;
using System.Globalization;
using System.Reflection;

namespace partner_aluro.Models
{
    public class LanguageService
    {
        private readonly IStringLocalizer _localizer;

        private readonly CultureInfo _currentCulture;
        public LanguageService(IStringLocalizerFactory factory, CultureInfo culture = null)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);

            _currentCulture = culture ?? CultureInfo.DefaultThreadCurrentUICulture;

            _localizer = factory.Create("SharedResource",assemblyName.Name);
        }

        public LocalizedString Getkey(string key)
        {
            return _localizer[key];
        }
        public decimal GetCurrently (decimal value, string CultureName)
        {
            if(CultureName == "pl-PL")
            {
                //var val = _currentCulture.IsNeutralCulture;
                value = (decimal)value;

            }
            else
            {
                value = (decimal)value / Core.Constants.Eur;
            }

            return value;
        }
    }
}
