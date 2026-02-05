using MorseCode.ITask;
using ShortDev.NodeFlow.WinUI;
using Windows.Media.Audio;

namespace VBAudioRouter.LocalAudioGraph.Nodes;

internal sealed partial class OutputNodeControl : NodeControl, IAudioNodeFactory<AudioDeviceOutputNode>
{
    public OutputNodeControl()
    {
        InitializeComponent();
    }

    public event EventHandler? NodeInvalidated;

    public async ITask<AudioDeviceOutputNode> CreateAudioNodeAsync(AudioGraph graph)
    {
        var result = await graph.CreateDeviceOutputNodeAsync();
        if (result.Status != AudioDeviceNodeCreationStatus.Success)
            throw result.ExtendedError;

        return result.DeviceOutputNode;
    }
}

public sealed partial class OutputNodeModel : NodeViewModel;
