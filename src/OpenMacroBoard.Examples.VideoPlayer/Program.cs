using AForge.Video.FFMPEG;
using OpenMacroBoard.SDK;
using StreamDeckSharp.Examples.CommonStuff;
using System;
using System.Diagnostics;
using System.Threading;

namespace StreamDeckSharp.Examples.VideoPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var deck = ExampleHelper.OpenBoard())
            {
                PlayVideoAForgeFFMPEG(deck, @"C:\testvideo.mp4");
            }
        }

        static void PlayVideoAForgeFFMPEG(IMacroBoard deck, string videoPath)
        {
            using (VideoFileReader reader = new VideoFileReader())
            {
                reader.Open(videoPath);
                var fr = reader.FrameRate;
                int frameLength = (int)Math.Round(1000.0 / fr);
                long frameNum = 0;

                while (true)
                {
                    var sw = Stopwatch.StartNew();
                    using (var frame = reader.ReadVideoFrame())
                    {
                        if (frame == null)
                            return;

                        deck.DrawFullScreenBitmap(frame);

                        var wait = frameLength - (int)sw.ElapsedMilliseconds;
                        sw.Restart();

                        if (wait > 0)
                            Thread.Sleep(wait);

                        frameNum++;

                        if (frameNum % 10 == 0)
                        {
                            if (Console.KeyAvailable)
                                return;
                        }
                    }
                }
            }
        }
    }
}



