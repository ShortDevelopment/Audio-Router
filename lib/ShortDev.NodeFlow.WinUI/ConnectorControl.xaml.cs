using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

namespace ShortDev.NodeFlow.WinUI;

public sealed partial class ConnectorControl : UserControl
{
    public ConnectorControl()
    {
        InitializeComponent();

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    public bool IsOutgoing { get; set; } = false;

    public bool AllowMultipleConnections { get; set; } = false;

    public Point ConnectorPosition
    {
        get => (Point)GetValue(ConnectorPositionProperty);
        set => SetValue(ConnectorPositionProperty, value);
    }

    public static DependencyProperty ConnectorPositionProperty { get; } = DependencyProperty.Register(
        nameof(ConnectorPosition),
        typeof(Point),
        typeof(ConnectorControl),
        new PropertyMetadata(new Point(0, 0))
    );

    public event EventHandler<(object? source, object? target)>? Connected;

    private void OnDragStarting(UIElement sender, DragStartingEventArgs args)
    {
        if (!IsOutgoing || _parent is null)
        {
            args.Cancel = true;
            return;
        }

        args.AllowedOperations = DataPackageOperation.Link;
        args.Data.Properties.Add("AttachedNode", _parent);
        args.Data.Properties.Add("Connector", this);
    }

    private void OnDragOver(object sender, DragEventArgs e)
    {
        e.AcceptedOperation = IsOutgoing switch
        {
            true => DataPackageOperation.None,
            false => DataPackageOperation.Link
        };
    }

    private void OnDrop(object sender, DragEventArgs e)
    {
        if (!IsOutgoing && e.DataView != null && e.DataView.Properties != null)
        {
            // ToDo: Do we allow multiple connections?

            var remoteConnector = (ConnectorControl)e.DataView.Properties["Connector"];

            Connected?.Invoke(this, (remoteConnector.DataContext, DataContext));
        }
        e.AcceptedOperation = DataPackageOperation.None;
    }

    NodeControl? _parent;
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _parent = this.FindAscendant<NodeControl>();
        if (_parent is null)
            return;

        _parent.PositionChanged += OnPositionChanged;
    }

    private void OnPositionChanged(object? sender, EventArgs e)
    {
        if (_parent is null)
            return;

        if (VisualTreeHelper.GetParent(_parent) is not UIElement parent)
            return;

        ConnectorPosition = TransformToVisual(parent)
            .TransformPoint(new Point(ActualWidth / 2, ActualHeight / 2));
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (_parent is null)
            return;

        _parent.PositionChanged -= OnPositionChanged;
        _parent = null;
    }
}
