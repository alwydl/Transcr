using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.ApplicationModel;

namespace Transcriber
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTask, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    [IntentFilter(
        new[] { Android.Content.Intent.ActionView, Android.Content.Intent.ActionSend, Android.Content.Intent.ActionPick },
        Categories = new[]
        {
             Android.Content.Intent.CategoryDefault
        },
        DataMimeType = "audio/*"
    )]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            if (intent?.Action == Intent.ActionSend && intent.Type?.StartsWith("audio/") == true)
            {
                //// Extract the audio file path
                //var sharedAudioPath = intent.ClipData?.GetItemAt(0)?.Uri?.Path;
                //if (sharedAudioPath != null)
                //{
                //    SpeechToText.OggPath = sharedAudioPath;
                //}

                Stream? inputStream = null;
                var filePath = intent?.ClipData?.GetItemAt(0);
                if (filePath?.Uri != null)
                {
                    SpeechToText.OpusStream = ContentResolver!.OpenInputStream(filePath.Uri)!;
                }
            }
        }
    }
}
