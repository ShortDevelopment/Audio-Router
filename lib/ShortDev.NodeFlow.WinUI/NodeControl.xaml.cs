using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using System.Collections;
using Windows.Foundation;

namespace ShortDev.NodeFlow.WinUI;

[ContentProperty(Name = nameof(NodeContent))]
public partial class NodeControl : Control, IConnectorContainer
{
    public UIElement? NodeContent
    {
        get => GetValue(NodeContentProperty) as UIElement;
        set => SetValue(NodeContentProperty, value);
    }

    public static DependencyProperty NodeContentProperty { get; } =
        DependencyProperty.Register(nameof(NodeContent), typeof(UIElement), typeof(NodeControl), new PropertyMetadata(null));

    public string? Title
    {
        get => GetValue(TitleProperty) as string;
        set => SetValue(TitleProperty, value);
    }

    public static DependencyProperty TitleProperty { get; } =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(NodeControl), new PropertyMetadata(null));

    public object? InputConnectorTemplate
    {
        get => GetValue(InputConnectorTemplateProperty);
        set => SetValue(InputConnectorTemplateProperty, value);
    }

    public static DependencyProperty InputConnectorTemplateProperty { get; } =
        DependencyProperty.Register(nameof(InputConnectorTemplate), typeof(object), typeof(NodeControl), new PropertyMetadata(null));

    public object? OutputConnectorTemplate
    {
        get => GetValue(OutputConnectorTemplateProperty);
        set => SetValue(OutputConnectorTemplateProperty, value);
    }

    public static DependencyProperty OutputConnectorTemplateProperty { get; } =
        DependencyProperty.Register(nameof(OutputConnectorTemplate), typeof(object), typeof(NodeControl), new PropertyMetadata(null));

    public IEnumerable? Inputs
    {
        get => (IEnumerable?)GetValue(InputsProperty);
        set => SetValue(InputsProperty, value);
    }

    public static DependencyProperty InputsProperty { get; } =
        DependencyProperty.Register(nameof(Inputs), typeof(IEnumerable), typeof(NodeControl), new PropertyMetadata(null));

    public IEnumerable? Outputs
    {
        get => (IEnumerable?)GetValue(OutputsProperty);
        set => SetValue(OutputsProperty, value);
    }

    public static DependencyProperty OutputsProperty { get; } =
        DependencyProperty.Register(nameof(Outputs), typeof(IEnumerable), typeof(NodeControl), new PropertyMetadata(null));

    readonly CompositeTransform PositionTransform;
    public NodeControl()
    {
        DefaultStyleKey = typeof(NodeControl);
        RenderTransform = PositionTransform = new CompositeTransform();

        PointerPressed += OnPointerPressed;
    }

    private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        this.BringToFront();
        Focus(FocusState.Keyboard);
        e.Handled = true;
    }

    UIElement? NodeTitleGrid;

    protected override void OnApplyTemplate()
    {
        NodeTitleGrid = (UIElement)GetTemplateChild("NodeTitleElement");
        NodeTitleGrid.ManipulationDelta += OnManipulationDelta;
        NodeTitleGrid.ManipulationStarted += OnManipulationStarted;
    }

    private void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        => this.BringToFront();

    public event EventHandler? PositionChanged;
    private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
        if (PositionTransform is null)
            return;

        PositionTransform.TranslateX += e.Delta.Translation.X;
        PositionTransform.TranslateY += e.Delta.Translation.Y;

        PositionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Point NodePosition
    {
        get => new(PositionTransform.TranslateX, PositionTransform.TranslateY);
        set
        {
            if (PositionTransform is null)
                return;

            PositionTransform.TranslateX = value.X;
            PositionTransform.TranslateY = value.Y;
        }
    }
}
