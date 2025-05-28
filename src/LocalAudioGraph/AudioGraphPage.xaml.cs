using Microsoft.UI.Xaml.Controls;

namespace VBAudioRouter.LocalAudioGraph;

public sealed partial class AudioGraphPage : Page
{
    public GraphViewModel ViewModel { get; } = new()
    {
        Nodes = {
            new() { Title = "Test", Inputs = { new() } },
            new() { Title = "Test2", Outputs = { new() } }
        }
    };

    public AudioGraphPage()
    {
        InitializeComponent();
    }

    private void ConnectorControl_Connected(object sender, (object? source, object? target) e)
    {
        if (e is not (ConnectorViewModel source, ConnectorViewModel target))
            return;

        ViewModel.Connect(source, target);
    }
}
