using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using YoutubeExplode;
using YoutubeExplode.Exceptions;
using YoutubeExplode.Videos.Streams;
using OutputParser;
using ConsoleTableExt;
using FilenameParser;

namespace VDownload
{
    class Youtube
    {
        // Informations about specified YouTube video (metadata, video streams and audio streams)
        public static void VideoInfo(string url)
        {
            try
            {
                // Metadata
                var metadata = GetMetadata(url);

                // Video streams
                string videoStreamsData;
                try
                {
                    var videoStreams = GetVideoStreams(url);
                    List<List<object>> videoStreamsTable = new();
                    foreach (KeyValuePair<int, Tuple<string[], VideoOnlyStreamInfo>> e in videoStreams)
                    {
                        videoStreamsTable.Add(new List<object> {
                            e.Key,
                            e.Value.Item1[0],
                            e.Value.Item1[1],
                            e.Value.Item1[2],
                            e.Value.Item1[3],
                            e.Value.Item1[4]
                        });
                    }
                    videoStreamsData = ConsoleTableBuilder
                        .From(videoStreamsTable)
                        .WithColumn("ID", "Bitrate", "Container", "Quality", "Codec", "Size")
                        .WithCharMapDefinition(CharMapDefinition.FramePipDefinition, Global.TABLEAPPEARANCE)
                        .Export().ToString().Trim();
                }
                catch (VideoUnplayableException)
                {
                    videoStreamsData = Output.Get(@"output\youtube\error_no_video_streams_video_unplayable.out", false, false);
                }

                // Audio streams
                string audioStreamsData;
                try
                {
                    var audioStreams = GetAudioStreams(url);
                    List<List<object>> audioStreamsTable = new();
                    foreach (KeyValuePair<int, Tuple<string[], AudioOnlyStreamInfo>> e in audioStreams)
                    {
                        audioStreamsTable.Add(new List<object> {
                            e.Key,
                            e.Value.Item1[0],
                            e.Value.Item1[1],
                            e.Value.Item1[2],
                            e.Value.Item1[3]
                        });
                    }
                    audioStreamsData = ConsoleTableBuilder
                        .From(audioStreamsTable)
                        .WithColumn("ID", "Bitrate", "Container", "Codec", "Size")
                        .WithCharMapDefinition(CharMapDefinition.FramePipDefinition, Global.TABLEAPPEARANCE)
                        .Export().ToString().Trim();
                }
                catch (VideoUnplayableException)
                {
                    audioStreamsData = Output.Get(@"output\youtube\error_no_audio_streams_video_unplayable.out", false, false);
                }

                // Print informations
                List<string> args = new()
                {
                    metadata["title"],
                    metadata["url"],
                    metadata["author"],
                    metadata["views"],
                    metadata["date"],
                    metadata["duration"],
                    metadata["rating"],
                    metadata["thumbnail"],
                    videoStreamsData,
                    audioStreamsData,
                };
                Console.WriteLine(Output.Get(
                    file: @"output\youtube\video_info.out",
                    args: args
                ));
            }
            catch (VideoUnavailableException)
            {
                Console.Write(Output.Get(@"output\youtube\error_invalid_link.out", args: new() { url }));
            }
            catch
            {
                Console.Write(Output.Get(@"output\youtube\error_undefined_getting_info.out"));
            }
        }



        // Downloading Video
        public static void VideoDownload(string url, Dictionary<string, string> options)
        {
            try
            {
                Dictionary<string, string> metadata = GetMetadata(url);

                // Options
                Dictionary<string, string> filenameCode = new Dictionary<string, string> {
                    {"%title%", metadata["title"]},
                    {"%author%", metadata["author"]},
                    {"%pub_date%", metadata["date"]},
                    {"%id%", metadata["id"]},
                    {"%act_date%", DateTime.Now.ToString(Config.Main.R_key("date_format"))},
                    {"%duration%", metadata["duration"]},
                    {"%views%", metadata["views"]},
                };
                string filename = Config.Main.R_key("filename");
                if (options.ContainsKey("filename") && !(options["filename"] == null))
                {
                    filename = options["filename"];
                }
                filename = Filename.Parse(filename, filenameCode);
                string output_path = Config.Main.R_key("output_path");
                if (options.ContainsKey("output_path") && !(options["output_path"] == null))
                {
                    output_path = options["output_path"];
                }
                string video_ext = Config.Main.R_key("video_ext");
                if (options.ContainsKey("video_ext") && !(options["video_ext"] == null))
                {
                    video_ext = options["video_ext"];
                }
                string audio_ext = Config.Main.R_key("audio_ext");
                if (options.ContainsKey("audio_ext") && !(options["audio_ext"] == null))
                {
                    audio_ext = options["audio_ext"];
                }
                bool onlyvideo = options.ContainsKey("onlyvideo");
                bool onlyaudio = options.ContainsKey("onlyaudio");

                // Downloading
                List<string> args = new()
                {
                    metadata["title"],
                    metadata["url"]
                };
                List<string> tempFiles = new();
                if (onlyvideo)
                {
                    var streams = GetVideoStreams(url);
                    var streamsData = new Dictionary<int, string[]>();
                    foreach (int i in streams.Keys) { streamsData[i] = streams[i].Item1; }
                    if (options.ContainsKey("video") && options["video"] != null && int.TryParse(options["video"], out int id)) { }
                    else { id = SelectBestStream(streamsData); }
                    args.Add(String.Format("Video stream ID: {0} ({1})", id, streamsData[id][2]));
                    Console.WriteLine(Output.Get(@"output\youtube\downloading_start.out", args: args));
                    string tempPath = DownloadVideoStream(streams, id);
                    string outPath = String.Format(@"{0}\{1}.{2}", output_path, filename, video_ext);
                    if (Path.GetExtension(tempPath).Trim('.') == video_ext)
                    {
                        File.Move(tempPath, outPath, true);
                    }
                    else
                    {
                        MediaProcessing.Convert(tempPath, outPath);
                        tempFiles.Add(tempPath);
                    }
                }
                else if (onlyaudio)
                {
                    var streams = GetAudioStreams(url);
                    var streamsData = new Dictionary<int, string[]>();
                    foreach (int i in streams.Keys) { streamsData[i] = streams[i].Item1; }
                    if (options.ContainsKey("audio") && options["audio"] != null && int.TryParse(options["audio"], out int id)) { }
                    else { id = SelectBestStream(streamsData); }
                    args.Add(String.Format("Audio stream ID: {0} ({1})", id, streamsData[id][0]));
                    Console.WriteLine(Output.Get(@"output\youtube\downloading_start.out", args: args));
                    string tempPath = DownloadAudioStream(streams, id);
                    string outPath = String.Format(@"{0}\{1}.{2}", output_path, filename, audio_ext);
                    if (Path.GetExtension(tempPath).Trim('.') == audio_ext)
                    {
                        File.Move(tempPath, outPath, true);
                    }
                    else
                    {
                        MediaProcessing.Convert(tempPath, outPath);
                        tempFiles.Add(tempPath);
                    }
                }
                else
                {
                    var streamsVideo = GetVideoStreams(url);
                    var streamsVideoData = new Dictionary<int, string[]>();
                    foreach (int i in streamsVideo.Keys) { streamsVideoData[i] = streamsVideo[i].Item1; }
                    if (options.ContainsKey("video") && options["video"] != null && int.TryParse(options["video"], out int video_id)) { }
                    else { video_id = SelectBestStream(streamsVideoData); }

                    var streamsAudio = GetAudioStreams(url);
                    var streamsAudioData = new Dictionary<int, string[]>();
                    foreach (int i in streamsAudio.Keys) { streamsAudioData[i] = streamsAudio[i].Item1; }
                    if (options.ContainsKey("audio") && options["audio"] != null && int.TryParse(options["audio"], out int audio_id)) { }
                    else { audio_id = SelectBestStream(streamsAudioData); }

                    args.Add(String.Format("Video stream ID: {0} ({1}) | Audio stream ID: {2} ({3})", video_id, streamsVideoData[video_id][2], audio_id, streamsAudioData[audio_id][0]));
                    Console.WriteLine(Output.Get(@"output\youtube\downloading_start.out", args: args));

                    string tempVideoPath = DownloadVideoStream(streamsVideo, video_id);
                    string tempAudioPath = DownloadAudioStream(streamsAudio, audio_id);

                    string outPath = String.Format(@"{0}\{1}.{2}", output_path, filename, video_ext);
                    MediaProcessing.Merge(tempVideoPath, tempAudioPath, outPath);
                    tempFiles.Add(tempVideoPath);
                    tempFiles.Add(tempAudioPath);
                }
                if (tempFiles.Count > 0)
                {
                    Console.WriteLine(Output.Get(@"output\youtube\delete_temporary.out", upSP: false, downSP: false));
                    foreach (string p in tempFiles)
                    {
                        File.Delete(p);
                    }
                }
                Console.WriteLine(Output.Get(@"output\youtube\done.out", upSP: false));
            }
            catch (VideoUnavailableException)
            {
                Console.WriteLine(Output.Get(@"output\youtube\error_invalid_link.out", args: new() { url }));
            }
            catch (VideoUnplayableException)
            {
                Console.WriteLine(Output.Get(@"output\youtube\error_unplayable_video.out", args: new() { url }));
            }
            catch
            {
                Console.WriteLine(Output.Get(@"output\youtube\error_undefined_downloading_video.out"));
            }
        }




        // Get metadata
        private static Dictionary<string, string> GetMetadata(string url)
        {
            var Client = new YoutubeClient();
            var metadata = new Dictionary<string, string>();
            var data = Client.Videos.GetAsync(url).Result;
            metadata["title"] = data.Title;
            metadata["author"] = data.Author.Title;
            metadata["date"] = data.UploadDate.Date.ToString(Config.Main.R_key("date_format"));
            metadata["duration"] = data.Duration.ToString();
            metadata["views"] = data.Engagement.ViewCount.ToString();
            metadata["rating"] = String.Format("{0}/{1}", data.Engagement.LikeCount.ToString(), data.Engagement.DislikeCount.ToString());
            metadata["thumbnail"] = String.Format(@"https://img.youtube.com/vi/{0}/maxresdefault.jpg", data.Id);
            metadata["url"] = data.Url;
            metadata["id"] = data.Id;
            return metadata;
        }

        // Get video streams
        private static Dictionary<int, Tuple<string[], VideoOnlyStreamInfo>> GetVideoStreams(string url)
        {
            var Client = new YoutubeClient();
            var streams = new Dictionary<int, Tuple<string[], VideoOnlyStreamInfo>>();
            var streamManifest = Client.Videos.Streams.GetManifestAsync(url).Result;
            var streamObjects = streamManifest.GetVideoOnlyStreams();
            int i = 0;
            foreach (var s in streamObjects)
            {
                string[] metadata = new string[] {
                    s.Bitrate.ToString(),
                    s.Container.ToString(),
                    s.VideoQuality.ToString(),
                    s.VideoCodec,
                    s.Size.ToString(),
                };
                var stream = Tuple.Create(metadata, s);
                streams[i] = stream;
                i++;
            }
            return streams;
        }

        // Get audio streams
        private static Dictionary<int, Tuple<string[], AudioOnlyStreamInfo>> GetAudioStreams(string url)
        {
            var Client = new YoutubeClient();
            var streams = new Dictionary<int, Tuple<string[], AudioOnlyStreamInfo>>();
            var streamManifest = Client.Videos.Streams.GetManifestAsync(url).Result;
            var streamObjects = streamManifest.GetAudioOnlyStreams();
            int i = 0;
            foreach (var s in streamObjects)
            {
                string[] metadata = new string[] {
                    s.Bitrate.ToString(),
                    s.Container.ToString(),
                    s.AudioCodec,
                    s.Size.ToString(),
                };
                var stream = Tuple.Create(metadata, s);
                streams[i] = stream;
                i++;
            }
            return streams;
        }



        // Select best stream
        private static int SelectBestStream(Dictionary<int, string[]> streams)
        {
            double max_bitrate_val = 0;
            int max_bitrate_id = 0;
            foreach (KeyValuePair<int, string[]> e in streams)
            {
                int id = e.Key;
                string bitrate = e.Value[0];
                double bitrate_val = Double.Parse(bitrate.Split(' ')[0]);
                string bitrate_unit = bitrate.Split(' ')[1];
                switch (bitrate_unit)
                {
                    case "Kbit/s": bitrate_val *= Math.Pow(1000, 1); break;
                    case "Mbit/s": bitrate_val *= Math.Pow(1000, 2); break;
                    case "Gbit/s": bitrate_val *= Math.Pow(1000, 3); break;
                    case "Tbit/s": bitrate_val *= Math.Pow(1000, 4); break;
                    case "Pbit/s": bitrate_val *= Math.Pow(1000, 5); break;
                    case "Ebit/s": bitrate_val *= Math.Pow(1000, 6); break;
                    case "Zbit/s": bitrate_val *= Math.Pow(1000, 7); break;
                    case "Ybit/s": bitrate_val *= Math.Pow(1000, 8); break;
                }
                if (bitrate_val > max_bitrate_val)
                {
                    max_bitrate_val = bitrate_val;
                    max_bitrate_id = id;
                }
            }
            return max_bitrate_id;
        }



        // Download video stream
        private static string DownloadVideoStream(Dictionary<int, Tuple<string[], VideoOnlyStreamInfo>> streams, int id)
        {
            var Client = new YoutubeClient();
            Directory.CreateDirectory(Global.PATH_TEMP);
            string tempPath = String.Format(@"{0}\video.{1}", Global.PATH_TEMP, streams[id].Item1[1]);

            Console.Write(Output.Get(@"output\youtube\downloading_video.out", upSP: false, downSP: false));
            Stopwatch downloadTime = new Stopwatch();
            downloadTime.Start();
            Client.Videos.Streams.DownloadAsync(streams[id].Item2, tempPath).AsTask().Wait();
            downloadTime.Stop();
            Console.WriteLine(String.Format(" (Done in {0} seconds)", downloadTime.Elapsed.TotalSeconds));

            return tempPath;
        }

        // Download audio stream
        private static string DownloadAudioStream(Dictionary<int, Tuple<string[], AudioOnlyStreamInfo>> streams, int id)
        {
            var Client = new YoutubeClient();
            Directory.CreateDirectory(Global.PATH_TEMP);
            string tempPath = String.Format(@"{0}\audio.{1}", Global.PATH_TEMP, streams[id].Item1[1]);

            Console.Write(Output.Get(@"output\youtube\downloading_audio.out", upSP: false, downSP: false));
            Stopwatch downloadTime = new Stopwatch();
            downloadTime.Start();
            Client.Videos.Streams.DownloadAsync(streams[id].Item2, tempPath).AsTask().Wait();
            downloadTime.Stop();
            Console.WriteLine(String.Format(" (Done in {0} seconds)", downloadTime.Elapsed.TotalSeconds));

            return tempPath;
        }
    }
}