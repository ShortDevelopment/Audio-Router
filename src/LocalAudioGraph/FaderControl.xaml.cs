using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using AudioRouter.GraphControl;

namespace AudioRouter.LocalAudioGraph;

[ObservableObject]
internal sealed partial class FaderControl : UserControl
{
    public FaderControl()
    {
        InitializeComponent();
    }

    [ObservableProperty]
    public partial FaderData? FaderData { get; set; } = null;

    private async void OpenGraphButton_Click(object sender, RoutedEventArgs e)
    {
    }
}
