using System.Collections.Generic;
using System.Globalization;

namespace Чтение.Localization
{
    /// <summary>
    /// Интерфейс, используемый для основы локализации в приложении.
    /// </summary>
    public interface ILocalizationProvider
    {
        object Localize(string key);
        IEnumerable<CultureInfo> Cultures { get; }
    }
}
