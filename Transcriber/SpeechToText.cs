using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Vorbis;
using NAudio.Wave;
using FFmpeg;
using System.Diagnostics;
using Concentus.Oggfile;
using Concentus.Structs;
using Concentus;

namespace Transcriber
{
    public static class SpeechToText
    {
        private static readonly string _subscriptionKey = "5271dda9e8fe4b16b1f7f4564e3829bc";
        private static readonly string _region = "brazilsouth";
        public static Stream OpusStream { get; internal set; }

        public async static Task<string> FromStream()
        {
            var speechConfig = SpeechConfig.FromSubscription("YourSpeechKey", "YourSpeechRegion");

            var wavStream = await ConvertOggToWavAsync();

                using (var binaryReader = new BinaryReader(wavStream))
                {
                    // Process binary data using BinaryReader methods

                    using var audioConfigStream = AudioInputStream.CreatePushStream();
                    using var audioConfig = AudioConfig.FromStreamInput(audioConfigStream);
                    using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

                    byte[] readBytes = wavStream.ToArray();
                    //do
                    //{
                      //  readBytes = binaryReader.ReadBytes(1024);
                        audioConfigStream.Write(readBytes, readBytes.Length);
                    //} while (readBytes.Length > 0);

                    var result = await speechRecognizer.RecognizeOnceAsync();

                    Console.WriteLine($"RECOGNIZED: Text={result.Text}");
                    return result.Text;
                }
        }

        public static async Task<MemoryStream> ConvertOggToWavAsync()
        {
            try
            {
                OpusStream.Position = 0;

                //var filePath = $@"C:\Users\blabla\foo\bar\";
                var fileOgg = "testAudio.ogg";
                //var fileWav = "testAudio.wav";

                //using (FileStream fileIn = new FileStream($"{filePath}{fileOgg}", FileMode.Open))
                MemoryStream pcmStream = new MemoryStream();
                {
                    var decoder = OpusCodecFactory.CreateDecoder(48000, 1);//.Create(48000, 1);
                    OpusOggReadStream oggIn = new OpusOggReadStream(decoder, OpusStream);
                    while (oggIn.HasNextPacket)
                    {
                        short[] packet = oggIn.DecodeNextPacket();
                        if (packet != null)
                        {
                            for (int i = 0; i < packet.Length; i++)
                            {
                                var bytes = BitConverter.GetBytes(packet[i]);
                                pcmStream.Write(bytes, 0, bytes.Length);
                            }
                        }
                    }
                    pcmStream.Position = 0;
                    return pcmStream;
                    //var sampleProvider = wavStream.ToSampleProvider();
                    //WaveFileWriter.CreateWaveFile16($"{filePath}{fileWav}", sampleProvider);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
    }
}
