using System.Collections.Generic;
using System.Globalization;
using Чтение.Localization;
using Чтение.Resources;

namespace Чтение.Localization
{
    public class ResxLocalizationProvider : ILocalizationProvider
    {
        private IEnumerable<CultureInfo> _cultures;

        public object Localize(string key)
        {
            return Strings.ResourceManager.GetObject(key);
        }

        public IEnumerable<CultureInfo> Cultures => _cultures ?? (_cultures = new List<CultureInfo>
        {
            new CultureInfo("ru-RU"),
            new CultureInfo("uk-UA")
        });
    }
}
