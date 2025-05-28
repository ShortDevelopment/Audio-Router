using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace ShortDev.NodeFlow.WinUI;

internal static class Extensions
{
    extension(DependencyObject @this)
    {
        public T FindAscendant<T>()
        {
            return VisualTreeHelper.GetParent(@this) switch
            {
                T t => t,
                DependencyObject parent => parent.FindAscendant<T>(),
                _ => throw new InvalidOperationException($"No ascendant of type {typeof(T).Name} found.")
            };
        }
    }

    extension(FrameworkElement @this)
    {
        /// <summary>
        /// https://stackoverflow.com/a/24120993/15213858
        /// </summary>
        /// <param name="this"></param>
        public void BringToFront()
        {
            if (@this.Parent is not Panel parent)
                return;

            int currentIndex = Canvas.GetZIndex(@this);
            int maxZ = 0;
            foreach (var child in parent.Children)
            {
                if (child == @this)
                    continue;

                var zIndex = Canvas.GetZIndex(child);
                maxZ = Math.Max(maxZ, zIndex);

                if (zIndex >= currentIndex)
                    Canvas.SetZIndex(child, zIndex - 1);
            }

            Canvas.SetZIndex(@this, maxZ);
        }
    }
}
