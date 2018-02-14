using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Чтение.Localization
{
    /// <summary>
    /// Базовый класс для слушателей изменения культуры
    /// </summary>
    public abstract class BaseLocalizationListener : IWeakEventListener, IDisposable
    {
        protected BaseLocalizationListener()
        {
            CultureChangedEventManager.AddListener(LocalizationManager.Instance, this);
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(CultureChangedEventManager))
            {
                OnCultureChanged();
                return true;
            }
            return false;
        }

        protected abstract void OnCultureChanged();

        public void Dispose()
        {
            CultureChangedEventManager.RemoveListener(LocalizationManager.Instance, this);
            Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
