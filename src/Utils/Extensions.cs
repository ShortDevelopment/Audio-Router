using AudioRouter.Utils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Diagnostics;

namespace AudioRouter.Utils;

internal static class Extensions
{
    /// <summary>
    /// https://stackoverflow.com/a/14353572/15213858
    /// </summary>
    /// <returns></returns>
    public static double Map(this double value, double fromSource, double toSource, double fromTarget, double toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }

    public static T? FindNameRecursive<T>(this FrameworkElement ele, string name) where T : FrameworkElement
    {
        for (var child_index = 0; child_index <= VisualTreeHelper.GetChildrenCount(ele) - 1; child_index++)
        {
            FrameworkElement child = (FrameworkElement)VisualTreeHelper.GetChild(ele, child_index);

            var search = (T?)child.FindName(name);
            if (search != null)
                return search;

            var recursion = child.FindNameRecursive<T>(name);
            if (recursion != null)
                return recursion;
        }
        return null;
    }

    public static FrameworkElement FindNameRecursive(this FrameworkElement ele, string name)
        => ele.FindNameRecursive(name);

    extension(Process)
    {
        public static Process? TryGetById(int id)
        {
            try
            {
                return Process.GetProcessById(id);
            }
            catch
            {
                return null;
            }
        }
    }

    extension(UIElement element)
    {
        public async Task ShowErrorDialogAsync(Exception ex)
        {
            ContentDialog dialog = new()
            {
                Title = ex.GetType().Name,
                Content = new TextBlock() { Text = ex.Message },
                DefaultButton = ContentDialogButton.Close,
                CloseButtonText = "Ok",
                XamlRoot = element.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}
