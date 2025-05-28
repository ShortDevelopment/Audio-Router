using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using VBAudioRouter.GraphControl;
using VBAudioRouter.Utils;
using Windows.Devices.Enumeration;
using Windows.Media.Audio;

namespace VBAudioRouter.LocalAudioGraph;

internal sealed partial class MixViewPage : Page
{
    public MixViewPage()
    {
        InitializeComponent();
    }

    AudioGraph _graph = null!;
    AudioDeviceOutputNode _outputNode = null!;
    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        try
        {
            _graph = await AudioGraphHelper.CreateGraphAsync((DeviceInformation)e.Parameter);
            var result = await _graph.CreateDeviceOutputNodeAsync();
            if (result.Status != AudioDeviceNodeCreationStatus.Success)
                throw result.ExtendedError;

            _outputNode = result.DeviceOutputNode;
        }
        catch (Exception ex)
        {
            await this.ShowErrorDialogAsync(ex);
        }
    }

    public ObservableCollection<FaderData> Faders { get; } = [];
    private void AddFaderButton_Click(object sender, RoutedEventArgs e)
    {
        FaderData faderData = new(_graph);
        faderData.ConnectionNode.AddOutgoingConnection(_outputNode);
        Faders.Add(faderData);
    }
}
