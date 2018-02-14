using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Чтение.Managers
{
    public static class AnimationManager
    {
        public static void ApplyOpacityAnimation(Action completed, bool needBack, double from, double to, params UIElement[] elements)
        {
            object countobj = new object();
            int count = 0;
            var da = new DoubleAnimation(to, new Duration(TimeSpan.FromMilliseconds(300)), FillBehavior.HoldEnd);
            da.Completed += (sender, e) =>
            {
                lock (countobj)
                    if (++count == elements.Length)
                    {
                        completed?.Invoke();
                        if (needBack)
                            ApplyAnimation(elements, new DoubleAnimation(from, new Duration(TimeSpan.FromMilliseconds(300)), FillBehavior.HoldEnd));
                    }
            };
            ApplyAnimation(elements, da);
        }

        public static void ApplyOpacityAnimationWithSavingValues(Action completed, bool needBack, bool reverse, double to, params UIElement[] elements)
        {
            object countobj = new object();
            double[] opacities = new double[elements.Length];
            for (int vl = 0; vl < elements.Length; vl++)
                opacities[vl] = elements[vl]?.Opacity ?? -1;
            int count = 0;
            var da = new DoubleAnimation(!reverse ? to : -1, new Duration(TimeSpan.FromMilliseconds(300)), FillBehavior.HoldEnd);
            da.Completed += (sender, e) =>
            {
                lock (countobj)
                    if (++count == opacities.Where(op => op != -1).Count())
                    {
                        completed?.Invoke();
                        if (needBack)
                            ApplyAnimation(elements, new DoubleAnimation(!reverse ? -1 : to, new Duration(TimeSpan.FromMilliseconds(300)), FillBehavior.HoldEnd), !reverse ? opacities : null);
                    }
            };
            ApplyAnimation(elements, da, !reverse ? null : opacities);
        }

        private static void ApplyAnimation(UIElement[] elements, DoubleAnimation animation, double[] ops = null)
        {
            Parallel.For(0, elements.Length, async (int a) =>
            {
                if (elements[a] != null)
                    await elements[a].Dispatcher.InvokeAsync(() =>
                    {
                        if (ops != null)
                        {
                            animation.To = ops[a];
                            elements[a].BeginAnimation(UIElement.OpacityProperty, animation, HandoffBehavior.Compose);
                        }
                        else elements[a].BeginAnimation(UIElement.OpacityProperty, animation, HandoffBehavior.Compose);
                    });
            });
        }

        /*public static void ApplyNavigateAnimation(Page page, Action completed, bool back = false)
        {
            if (page != null)
            {
                var da = new DoubleAnimation(!back ? 0 : 1, new Duration(TimeSpan.FromMilliseconds(300)));
                da.Completed += (sender, e) =>
                    completed?.Invoke();
                page.BeginAnimation(UIElement.OpacityProperty, da);
            }
        }*/

        public static void ApplyDefaultLanguageAnimation(Button hide, Button show, Action completed)
        {
            object countobj = new object();
            int count = 0;
            if (hide.Opacity != 0.4)
            {
                hide.IsEnabled = false;
                var da = new DoubleAnimation(0.4, new Duration(TimeSpan.FromMilliseconds(300)));
                da.Completed += (sender, e) =>
                {
                    lock (countobj)
                        if (++count == 2)
                            completed();
                };
                hide.BeginAnimation(UIElement.OpacityProperty, da);
            }
            if (show.Opacity != 1)
            {
                var da = new DoubleAnimation(1, new Duration(TimeSpan.FromMilliseconds(300)));
                da.Completed += (sender, e) =>
                {
                    show.IsEnabled = true;
                    lock (countobj)
                        if (++count == 2)
                            completed();
                };
                show.BeginAnimation(UIElement.OpacityProperty, da);
            }
        }

        public static void ApplyMarginAnimation(UIElement element, Thickness values, Action completed, bool hiding = false)
        {
            int count = 0;
            object locker = new object();
            void CompletedAnim(object sender, EventArgs e)
            {
                lock(locker)
                    if (++count == 2)
                        completed?.Invoke();
            }
            var ta = new ThicknessAnimation(values, new Duration(TimeSpan.FromMilliseconds(100)));
            ta.Completed += CompletedAnim;
            var da = new DoubleAnimation(hiding ? 0.3 : 1, new Duration(TimeSpan.FromMilliseconds(100)));
            da.Completed += CompletedAnim;
            element.BeginAnimation(FrameworkElement.MarginProperty, ta, HandoffBehavior.Compose);
            element.BeginAnimation(UIElement.OpacityProperty, da, HandoffBehavior.Compose);
        }
    }
}
