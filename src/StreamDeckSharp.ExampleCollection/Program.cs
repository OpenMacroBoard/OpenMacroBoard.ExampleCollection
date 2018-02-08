using System;
using System.Diagnostics;
using StreamDeckSharp.Extensions;
using System.Threading;
using AForge.Video.FFMPEG;

namespace StreamDeckSharp.ExampleCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var deck = StreamDeck.OpenDevice())
            {
                PlayVideoAForgeFFMPEG(deck, @"C:\testvideo.mp4");
            }
        }

        static void PlayVideoAForgeFFMPEG(IStreamDeck deck, string videoPath)
        {
            using (VideoFileReader reader = new VideoFileReader())
            {
                reader.Open(videoPath);
                var fr = reader.FrameRate;
                int frameLength = (int)Math.Round(1000.0 / fr);

                while (true)
                {
                    var sw = Stopwatch.StartNew();
                    using (var frame = reader.ReadVideoFrame())
                    {
                        if (frame == null) return;
                        deck.DrawFullScreenBitmap(frame);
                        var wait = frameLength - (int)sw.ElapsedMilliseconds;
                        sw.Restart();
                        if (wait > 0) Thread.Sleep(wait);
                    }
                }
            }
        }
    }
}



