using NAudio.CoreAudioApi.Interfaces;
using AudioRouter.Capture;
using Windows.Win32;
using Windows.Win32.System.Com.StructuredStorage;

namespace AudioRouter;
internal static class AudioInterfaceActivator
{
    public static ValueTask<T> ActivateAudioInterfaceAsync<T>(string deviceId) where T : class
    {
        ActivateAudioInterfaceCompletionHandler<T> handler = new();
        PInvoke.ActivateAudioInterfaceAsync(deviceId, typeof(T).GUID, activationParams: default(PROPVARIANT), handler, out var resultHandler);
        handler.WaitForCompletion();

        resultHandler.GetActivateResult(out var hres, out var result);
        hres.ThrowOnFailure();

        return ValueTask.FromResult((T)result);
    }
}
