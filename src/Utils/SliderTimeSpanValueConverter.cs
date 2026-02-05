using Microsoft.UI.Xaml.Data;

namespace AudioRouter.Utils;

internal sealed partial class SliderTimeSpanValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (targetType == typeof(string))
            return TimeSpan.FromMilliseconds(System.Convert.ToDouble(value)).ToString(@"mm\:ss");

        if (value is TimeSpan timespan)
            return timespan.TotalMilliseconds;

        throw new NotImplementedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => Convert(value, targetType, parameter, language);
}
