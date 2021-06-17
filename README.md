# VDownload

VDownload is a universal CLI (command line interface) video downloader. It allows you to download video from the following websites:
- YouTube (single video)
- YouTube (playlist)
- Twitch *[planned for version 0.3]*
- ... *[more pages will be added later]*

## Setup

**Requirements:**

- Architecture: x86/x64/arm64/arm32 (ARM builds have not been tested. Feedback is welcome.)
- Operating system: min. Windows 7
- NET Runtime (min. version 5.0) [[LINK]](https://dotnet.microsoft.com/download)
- Microsoft Visual C++ 2015 Redistributable [[LINK]](https://www.microsoft.com/en-US/download/details.aspx?id=48145)
 
VDownload also uses FFmpeg to convert and mux media files, but it is included. If you have FFmpeg installed on your computer, you can change used installation of FFmpeg in settings and delete included FFmpeg folder (for example when you are low on disk space).

Executable is available [HERE](https://github.com/mateuszskoczek/VDownload/releases).


**Installation:**

1. Download executable suitable for your architecture.
2. Put files in any folder
3. Add folder path to PATH (optional, but if you don't you will have to open terminal in app folder to use the app)



## Commands, settings & filename template

| Command                      | Description                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| ---------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `about`                      | Informations about this program                                                                                                                                                                                                                                                                                                                                                                                                                                         |
| `info <link>`                | Informations about video (metadata and available streams)                                                                                                                                                                                                                                                                                                                                                                                                               |
| `download <link> [options]`  | Download video<br><br>Options:<br>`--onlyvideo` (download only video)<br>`--onlyaudio` (download only audio)<br>`--video=<id>` (download video stream with specified id; best if not specified)<br>`--audio=<id>` (download audio stream with specified id; best if not specified)<br>`--output_path=<path>` (output file path)<br>`--filename=<filename>` (filename; template)<br>`--video_ext=<extension>` (video file extension)<br>`--audio_ext=<extension>` (audio file extension) |
| `settings-get <key>`         | Return value of the specified key                                                                                                                                                                                                                                                                                                                                                                                                                                       |
| `settings-set <key> <value>` | Set value of the specified key                                                                                                                                                                                                                                                                                                                                                                                                                                          |
| `settings-reset`             | Restore default settings                                                                                                                                                                                                                                                                                                                                                                                                                                                |

Command example: `vdownload download https://www.youtube.com/watch?v=dQw4w9WgXcQ --onlyvideo --video=5 --filename="absolutely nothing suspicious here"`


**Settings keys:**

| Key           | Description                                                    |
| ------------- | -------------------------------------------------------------- |
| `filename`    | Filename template                                              |
| `output_path` | Output file path                                               |
| `video_ext`   | Default video file extension                                   |
| `audio_ext`   | Default audio file extension                                   |
| `date_format` | Format of publication date in filenames and video informations |
| `ffmpeg_path` | FFmpeg executables path                                        |


**Filename template:**

| Wildcard     | Description            |
| ------------ | ---------------------- |
| `%title%`    | Video title            |
| `%author%`   | Video author           |
| `%pub_date%` | Video publication date |
| `%id%`       | Video ID               |
| `%act_date%` | Actual date            |
| `%duration%` | Video duration         |
| `%views%`    | Video views            |



## Issues & Support

VDownload is completely free and open source. I developing it in my free time for no money. You can contribute to the development of the application by reporting issues and your ideas [HERE](https://github.com/mateuszskoczek/VDownload/issues). Please attach informations about VDownload version which you use, your operating system and video which you want to download, when you create a new issue. Also make sure, that your problem is not already described and resolved.



## For developers

VDownload uses the following libraries:
- ConsoleTableExt [[LINK]](https://github.com/minhhungit/ConsoleTableExt)
- FFMpegCore [[LINK]](https://github.com/rosenbjerg/FFMpegCore)
- YoutubeExplode [[LINK]](https://github.com/Tyrrrz/YoutubeExplode)
- LightConfig [[LINK]](https://github.com/mateuszskoczek/LightConfig)
FFmpeg is not included in repository, because of file size limitations. You have to download FFmpeg by yourself and put executables in "ffmpeg" directory.