using System.Windows.Data;

namespace Чтение.Localization
{
    /// <summary>
    /// Слушатель изменения культуры при локализации по привязке
    /// </summary>
    public class BindingLocalizationListener : BaseLocalizationListener
    {
        private BindingExpressionBase BindingExpression { get; set; }

        public void SetBinding(BindingExpressionBase bindingExpression)
        {
            BindingExpression = bindingExpression;
        }

        protected override void OnCultureChanged()
        {
            try
            {
                BindingExpression?.UpdateTarget();
            }
            catch { }
        }
    }
}
