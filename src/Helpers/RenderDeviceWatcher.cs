using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;
using Windows.Devices.Enumeration;

namespace AudioRouter.Helpers;

public sealed class RenderDeviceWatcher
{
    readonly DispatcherQueue _dispatcher = DispatcherQueue.GetForCurrentThread();

    public ObservableCollection<DeviceInformation> RenderDevices { get; } = [];

    DeviceWatcher? _watcher;
    public void Start()
    {
        if (_watcher is not null)
            return;

        _watcher = DeviceInformation.CreateWatcher(DeviceClass.AudioRender);
        _watcher.Added += OnAdded;
        _watcher.Updated += OnUpdated;
        _watcher.Removed += OnRemoved;
        _watcher.Start();
    }

    private void OnAdded(DeviceWatcher sender, DeviceInformation args)
        => _dispatcher.TryEnqueue(() => RenderDevices.Add(args));

    private void OnUpdated(DeviceWatcher sender, DeviceInformationUpdate args)
    {
        // ToDo: Handle updates to device information
    }

    private void OnRemoved(DeviceWatcher sender, DeviceInformationUpdate args)
    {
        _dispatcher.TryEnqueue(() =>
        {
            for (int i = RenderDevices.Count - 1; i >= 0; i++)
            {
                if (RenderDevices[i].Id != args.Id)
                    continue;

                RenderDevices.RemoveAt(i);
                break;
            }
        });
    }

    public void Stop()
    {
        if (_watcher is null)
            return;

        _watcher.Added -= OnAdded;
        _watcher.Updated -= OnUpdated;
        _watcher.Removed -= OnRemoved;

        _watcher.Stop();
        _watcher = null;
    }
}
