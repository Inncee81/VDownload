using System;
using System.Diagnostics;
using FFMpegCore;
using OutputParser;

namespace VDownload
{
    class MediaProcessing
    {
        private static FFOptions  ffmpegOptions = new FFOptions {
            BinaryFolder = @".\ffmpeg",
        };
        public static void Convert(string input, string output)
        {
            GlobalFFOptions.Configure(ffmpegOptions);
            Console.Write(Output.Get(@"output\ffmpeg\converting_file.out", upSP: false, downSP: false));
            Stopwatch convertingTime = new Stopwatch();
            convertingTime.Start();
            FFMpegArguments.FromFileInput(input).OutputToFile(output).ProcessSynchronously();
            convertingTime.Stop();
            Console.WriteLine(String.Format(" (Done in {0} seconds)", convertingTime.Elapsed.TotalSeconds));
        }

        public static void Merge(string inputVideo, string inputAudio, string output)
        {
            GlobalFFOptions.Configure(ffmpegOptions);
            Console.Write(Output.Get(@"output\ffmpeg\merging_streams.out", upSP: false, downSP: false));
            Stopwatch mergingTime = new Stopwatch();
            mergingTime.Start();
            FFMpeg.ReplaceAudio(inputVideo, inputAudio, output);
            mergingTime.Stop();
            Console.WriteLine(String.Format(" (Done in {0} seconds)", mergingTime.Elapsed.TotalSeconds));
        }
    }
}
