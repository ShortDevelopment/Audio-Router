using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Windows.Foundation;

namespace VBAudioRouter.Utils;

public class MediaTransportControlsWrapper(MediaTransportControls control)
{
    public bool IsMuted { get; private set; }

    public event TypedEventHandler<MediaTransportControlsWrapper, bool>? MutedChanged;

    private void AudioMuteButton_Click(object sender, RoutedEventArgs e)
    {
        IsMuted = !IsMuted;
    }

    public double Volume { get; private set; }

    public event TypedEventHandler<MediaTransportControlsWrapper, double>? VolumeChanged;

    private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {

    }

    public bool IsPlaying { get; private set; }

    public event TypedEventHandler<MediaTransportControlsWrapper, bool>? PlayStateChanged;

    private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
    {
        IsPlaying = !IsPlaying;
    }

    public TimeSpan Position { get; private set; }

    public event TypedEventHandler<MediaTransportControlsWrapper, TimeSpan>? PositionChanged;

    private void ProgressSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
    }

    public TimeSpan Duration { get; private set; }
}
