using AudioRouter.Capture;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using MorseCode.ITask;
using ShortDev.NodeFlow.WinUI;
using Windows.Media.Audio;

namespace AudioRouter.LocalAudioGraph.Nodes;

public sealed partial class ProcessInputNodeControl : NodeControl, IAudioNodeFactory<AudioFrameInputNode>
{
    public IList<ProcessTreeNode> ProcessTreeNodes { get; } = ProcessSnapshot();
    public ProcessInputNodeControl()
    {
        InitializeComponent();
    }

    public Canvas? Canvas { get; set; }

    private ProcessAudioCapture? _capture;

    public event EventHandler? NodeInvalidated;
    private void InputDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        => NodeInvalidated?.Invoke(this, EventArgs.Empty);

    private void MuteToggleButton_Click(object sender, RoutedEventArgs e)
    {
        // if (GraphNode.ConsumeInput)
        //     MuteButton.Icon = new SymbolIcon(Symbol.Mute);
        // else
        //     MuteButton.Icon = new SymbolIcon(Symbol.Volume);
    }

    private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        // GainSlider.Value.Map(0, 100, 0, GainControl.fxeq_max_gain);
    }

    public async ITask<AudioFrameInputNode> CreateAudioNodeAsync(AudioGraph graph)
        => graph.CreateFrameInputNode();
}

public sealed partial class ProcessInputNodeModel : NodeViewModel;
