using Microsoft.UI.Xaml;
using Windows.Foundation;
using PathShape = Microsoft.UI.Xaml.Shapes.Path;

namespace ShortDev.NodeFlow.WinUI;

public sealed partial class BezierConnection : PathShape
{
    public BezierConnection()
    {
        InitializeComponent();
    }

    public Point StartPoint
    {
        get => (Point)GetValue(StartPointProperty);
        set => SetValue(StartPointProperty, value);
    }

    public static DependencyProperty StartPointProperty { get; }
        = DependencyProperty.Register(nameof(StartPoint), typeof(Point), typeof(BezierConnection), new PropertyMetadata(new Point()));

    public Point EndPoint
    {
        get => (Point)GetValue(EndPointProperty);
        set => SetValue(EndPointProperty, value);
    }

    public static DependencyProperty EndPointProperty { get; }
        = DependencyProperty.Register(nameof(EndPoint), typeof(Point), typeof(BezierConnection), new PropertyMetadata(new Point()));

    static double Offset(Point start, Point end)
       => Math.Max(10, Math.Abs(end.X - start.X) * 0.3);

    Point ControlPoint1(Point start, Point end)
    {
        return new(
            start.X + Offset(start, end),
            start.Y
        );
    }

    Point ControlPoint2(Point start, Point end)
    {
        return new(
            end.X - Offset(start, end),
            end.Y
        );
    }

    private void Path_GotFocus(object sender, RoutedEventArgs e)
    {
        StrokeDashArray = [10];
        FocusAnimation.Begin();
    }

    private void Path_LostFocus(object sender, RoutedEventArgs e)
    {
        StrokeDashArray = [];
        FocusAnimation.Stop();
    }

    private void Path_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        e.Handled = true;
        Focus(FocusState.Pointer);
    }
}
