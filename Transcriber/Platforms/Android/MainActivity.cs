using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.ApplicationModel;

namespace Transcriber
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //PlatformConfiguration.SetPlatformSpecifics(this);

            if (Intent?.Action == Android.Content.Intent.ActionSend)
            {
                if (Intent.Type?.StartsWith("audio/") == true)
                {
                    // Extract the audio file path
                    var sharedAudioPath = Intent.ClipData?.GetItemAt(0)?.Uri?.Path;
                    if (sharedAudioPath != null)
                    {
                        // Handle the audio file path (e.g., display a message, access the audio)
                        Console.WriteLine($"Received audio file: {sharedAudioPath}");
                    }
                }
            }
        }
    }
}
