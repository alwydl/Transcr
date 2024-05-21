using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcriber
{
    public static class SpeechToText
    {
        private static readonly string _subscriptionKey = "5271dda9e8fe4b16b1f7f4564e3829bc";
        private static readonly string _region = "brazilsouth";

        public async static Task<string> FromStream()
        {
            var speechConfig = SpeechConfig.FromSubscription("YourSpeechKey", "YourSpeechRegion");

            //var reader = new BinaryReader(File.OpenRead("PathToFile.wav"));
            
            using var stream = await FileSystem.OpenAppPackageFileAsync("AboutAssets.txt");
            using var reader = new StreamReader(stream);

            var data = reader.ReadToEnd();  // Read as string
            var bytes = Encoding.UTF8.GetBytes(data); // Convert to bytes (assuming UTF-8 encoding)

            using (var binaryReader = new BinaryReader(new MemoryStream(bytes)))
            {
                // Process binary data using BinaryReader methods


                using var audioConfigStream = AudioInputStream.CreatePushStream();
                using var audioConfig = AudioConfig.FromStreamInput(audioConfigStream);
                using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

                byte[] readBytes;
                do
                {
                    readBytes = binaryReader.ReadBytes(1024);
                    audioConfigStream.Write(readBytes, readBytes.Length);
                } while (readBytes.Length > 0);

                var result = await speechRecognizer.RecognizeOnceAsync();
                Console.WriteLine($"RECOGNIZED: Text={result.Text}");
                return result.Text;
            }
        }
    }
}
