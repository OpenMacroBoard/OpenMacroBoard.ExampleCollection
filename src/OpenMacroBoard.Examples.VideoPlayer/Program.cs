using Accord.Video.FFMPEG;
using OpenMacroBoard.Examples.CommonStuff;
using OpenMacroBoard.SDK;
using System;
using System.Diagnostics;
using System.Threading;

namespace OpenMacroBoard.Examples.VideoPlayer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args == null || args.Length < 0)
            {
                Console.WriteLine("Provide a path to a video as the first commandline parameter");
                return;
            }

            // Set your video path here or as commandline argument
            var videoPath = args[0];

            using (var deck = ExampleHelper.OpenBoard())
            {
                PlayVideoAForgeFFMPEG(deck, videoPath);
            }
        }

        private static void PlayVideoAForgeFFMPEG(IMacroBoard deck, string videoPath)
        {
            using (var reader = new VideoFileReader())
            {
                reader.Open(videoPath);
                var fr = reader.FrameRate;
                var frameLength = (int)Math.Round(1000.0 / fr.ToDouble());
                long frameNum = 0;

                while (true)
                {
                    var sw = Stopwatch.StartNew();
                    using (var frame = reader.ReadVideoFrame())
                    {
                        if (frame == null)
                        {
                            return;
                        }

                        deck.DrawFullScreenBitmap(frame);

                        var wait = frameLength - (int)sw.ElapsedMilliseconds;
                        sw.Restart();

                        if (wait > 0)
                        {
                            Thread.Sleep(wait);
                        }

                        frameNum++;

                        if (frameNum % 10 == 0)
                        {
                            if (Console.KeyAvailable)
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}



