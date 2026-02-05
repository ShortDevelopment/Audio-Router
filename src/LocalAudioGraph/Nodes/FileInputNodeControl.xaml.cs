using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MorseCode.ITask;
using ShortDev.NodeFlow.WinUI;
using System.Diagnostics;
using Windows.Media.Audio;
using Windows.Media.Core;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace AudioRouter.LocalAudioGraph.Nodes;

[ObservableObject]
internal sealed partial class FileInputNodeControl : NodeControl, IAudioNodeFactory<MediaSourceAudioInputNode>
{
    private FileInputNodeControl()
    {
        InitializeComponent();
    }

    [ObservableProperty]
    public partial MediaSource? MediaSource { get; set; }

    public Canvas? Canvas { get; set; }

    public ConnectorControl OutgoingConnector => OutgoingConnectorControl;

    public event EventHandler? NodeInvalidated;
    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        var file = await CreatePicker().PickSingleFileAsync();
        if (file == null)
            return;

        PathDisplay.Text = file.Path;

        NodeInvalidated?.Invoke(this, EventArgs.Empty);
    }

    static FileOpenPicker CreatePicker()
    {
        FileOpenPicker picker = new()
        {
            SuggestedStartLocation = PickerLocationId.MusicLibrary,
            ViewMode = PickerViewMode.Thumbnail,
            FileTypeFilter =
            {
                ".mp3",
                ".wav",
                ".wma",
                ".m4a",
            }
        };
        InitializeWithWindow.Initialize(picker, Process.GetCurrentProcess().MainWindowHandle);
        return picker;
    }

    public async ITask<MediaSourceAudioInputNode> CreateAudioNodeAsync(AudioGraph graph)
    {
        var result = await graph.CreateMediaSourceAudioInputNodeAsync(MediaSource);
        if (result.Status != MediaSourceAudioInputNodeCreationStatus.Success)
            throw result.ExtendedError;

        return result.Node;
    }
}

