using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Windows.Foundation;

namespace AudioRouter.LocalAudioGraph;

public sealed partial class GraphViewModel : ObservableObject
{
    public ObservableCollection<NodeViewModel> Nodes { get; } = [];
    public ObservableCollection<ConnectionViewModel> Connections { get; } = [];

    public void Connect(ConnectorViewModel source, ConnectorViewModel target)
    {
        Connections.Add(new()
        {
            Source = source,
            Target = target
        });
    }
}

public partial class NodeViewModel : ObservableObject
{
    public string Title { get; set; } = "Some Node";

    public ObservableCollection<ConnectorViewModel> Inputs { get; } = [];
    public ObservableCollection<ConnectorViewModel> Outputs { get; } = [];
}

public sealed partial class ConnectorViewModel : ObservableObject
{
    [ObservableProperty]
    public partial Point Anchor { get; set; }
}

public sealed partial class ConnectionViewModel : ObservableObject
{
    [ObservableProperty]
    public partial ConnectorViewModel Source { get; set; }

    [ObservableProperty]
    public partial ConnectorViewModel Target { get; set; }
}
