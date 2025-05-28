using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using VBAudioRouter.Helpers;
using Windows.Devices.Enumeration;

namespace VBAudioRouter.UI;

public sealed partial class MainPage : Page
{
    RenderDeviceWatcher RenderDeviceWatcher { get; } = new();
    public MainPage()
    {
        InitializeComponent();
    }

    Type? _pageType;
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        _pageType = (Type)e.Parameter;

        RenderDeviceWatcher.Start();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        RenderDeviceWatcher.Stop();
    }

    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (_pageType is null)
            return;

        var deviceInfo = (DeviceInformation)args.SelectedItem;
        ContentFrame.Navigate(_pageType, deviceInfo, args.RecommendedNavigationTransitionInfo);
    }
}
