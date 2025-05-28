using Microsoft.UI.Xaml.Controls;
using VBAudioRouter.Helpers;
using VBAudioRouter.LocalAudioGraph;
using VBAudioRouter.LocalAudioMix;
using VBAudioRouter.UI;

namespace VBAudioRouter;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Title = "AppDisplayName".GetLocalized();

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(DefaultTitleBar);
    }

    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        var item = args.SelectedItemContainer;
        if (item is null)
            return;

        var (page, parameter) = item.Tag switch
        {
            "LocalAudioGraph" => (typeof(AudioGraphPage), null),
            "LocalAudioMix" => (typeof(MainPage), typeof(SpeakerControlPage)),
            _ => (null, null)
        };
        if (page is null)
            return;

        MainFrame.Navigate(page, parameter, args.RecommendedNavigationTransitionInfo);
    }
}
