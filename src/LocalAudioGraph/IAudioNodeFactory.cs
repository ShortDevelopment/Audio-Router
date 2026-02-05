using MorseCode.ITask;
using Windows.Media.Audio;

namespace AudioRouter.LocalAudioGraph;

internal interface IAudioNodeFactory<out TNode> where TNode : IAudioNode
{
    ITask<TNode> CreateAudioNodeAsync(AudioGraph graph);
    event EventHandler? NodeInvalidated;
}
