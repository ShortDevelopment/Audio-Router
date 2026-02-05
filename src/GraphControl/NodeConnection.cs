using Microsoft.UI.Xaml.Shapes;
using ShortDev.NodeFlow.WinUI;

namespace AudioRouter.GraphControl;
public struct NodeConnection
{
    public ConnectorControl SourceConnector { get; set; }
    public ConnectorControl DestinationConnector { get; set; }
    public Shape Line { get; set; }
}

