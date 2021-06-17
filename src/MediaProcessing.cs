using System;
using System.Diagnostics;
using FFMpegCore;
using VDownload.Parsers;

namespace VDownload
{
    class MediaProcessing
    {
        private static FFOptions  ffmpegOptions = new FFOptions {
            BinaryFolder = Global.Paths.FFMPEG,
            TemporaryFilesFolder = Global.Paths.TEMP,
        };

        public static void Convert(string input, string output)
        {
            GlobalFFOptions.Configure(ffmpegOptions);
            Console.Write(TerminalOutput.Get(@"output\media_processing\converting_file.out", upSP: false, downSP: false));
            Stopwatch convertingTime = new Stopwatch();
            convertingTime.Start();
            try
            {
                FFMpegArguments.FromFileInput(input).OutputToFile(output).ProcessSynchronously();
            }
            catch
            {
                Console.Write(TerminalOutput.Get(@"output\media_processing\error_file_cannot_be_converted.out"));
                Environment.Exit(0);
            }
            convertingTime.Stop();
            Console.WriteLine(String.Format(" (Done in {0} seconds)", convertingTime.Elapsed.TotalSeconds));
        }

        public static void Merge(string inputVideo, string inputAudio, string output)
        {
            GlobalFFOptions.Configure(ffmpegOptions);
            Console.Write(TerminalOutput.Get(@"output\media_processing\merging_streams.out", upSP: false, downSP: false));
            Stopwatch mergingTime = new Stopwatch();
            mergingTime.Start();
            try
            {
                FFMpeg.ReplaceAudio(inputVideo, inputAudio, output);
            }
            catch
            {
                Console.Write(TerminalOutput.Get(@"output\media_processing\error_streams_cannot_be_merged.out"));
                Environment.Exit(0);
            }
            mergingTime.Stop();
            Console.WriteLine(String.Format(" (Done in {0} seconds)", mergingTime.Elapsed.TotalSeconds));
        }
    }
}
