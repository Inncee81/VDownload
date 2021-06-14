<h1>VDownload</h1>
VDownload is a universal CLI (command line interface) video downloader. It allows you to download video from the following websites:
<ul>
    <li>YouTube (single video)</li>
    <li>YouTube (playlist) <i>[planned for version 0.2]</i></li>
    <li>Twitch <i>[planned for version 0.3]</i></li>
    <li>... <i>[more pages will be added later]</i></li>
</ul>

<h2>Setup</h2>
This app requires:
<ul>
    <li>Windows 10</li>
    <li>.NET Runtime (min. version 5.0) [<a href="https://dotnet.microsoft.com/download">LINK</a>]</li>
</ul>
VDownload also uses FFmpeg to convert and mux media files, but it is included. If you have FFmpeg installed on your computer, you can change used installation of FFmpeg in settings and delete included FFmpeg folder (for example when you are low on disk space).<br>
Executable is available <a href="https://github.com/mateuszskoczek/VDownload/releases">HERE</a>.

<h2>Commands, settings & filename template</h2>
<table>
    <tr>
        <th>Command</th>
        <th>Description</th>
    </tr>
    <tr>
        <td><code>about</code></td>
        <td>Informations about this program</td>
    </tr>
    <tr>
        <td><code>info &lt;link&gt;</code></td>
        <td>Informations about video (metadata and available streams)</td>
    </tr>
    <tr>
        <td><code>download &lt;link&gt; [options]</code></td>
        <td>Download video</td>
    </tr>
    <tr>
        <td colspan="2">
            Options:<br>
            <code>--onlyvideo</code> (download only video)<br>
            <code>--onlyaudio</code> (download only audio)<br>
            <code>--video=&lt;id&gt;</code> (download video stream with specified id, best if not specified)<br>
            <code>--audio=&lt;id&gt;</code> (download audio stream with specified id, best if not specified)<br>
            <code>--output_path=&lt;path&gt;</code> (output file path)<br>
            <code>--filename=&lt;filename&gt;</code> (filename; template)<br>
            <code>--video_ext=&lt;extension&gt;</code> (video file extension)<br>
            <code>--audio_ext=&lt;extension&gt;</code> (audio file extension)
        </td>
    </tr>
    <tr>
        <td><code>settings-get &lt;key&gt;</code></td>
        <td>Return value of the specified key</td>
    </tr>
    <tr>
        <td><code>settings-set &lt;key&gt; &lt;value&gt;</code></td>
        <td>Set value of the specified key</td>
    </tr>
    <tr>
        <td><code>settings-reset</code></td>
        <td>Restore default settings</td>
    </tr>
</table>

<b>Settings keys:</b>
<table>
    <tr>
        <th>Key</th>
        <th>Description</th>
    </tr>
    <tr>
        <td><code>filename</code></td>
        <td>Filename template</td>
    </tr>
    <tr>
        <td><code>output_path</code></td>
        <td>Output file path</td>
    </tr>
    <tr>
        <td><code>video_ext</code></td>
        <td>Default video file extension</td>
    </tr>
    <tr>
        <td><code>audio_ext</code></td>
        <td>Default audio file extension</td>
    </tr>
    <tr>
        <td><code>date_format</code></td>
        <td>Format of publication date in filenames and video informations</td>
    </tr>
    <tr>
        <td><code>ffmpeg_path</code></td>
        <td>FFmpeg executables path</td>
    </tr>
</table>

<b>Filename template:</b>
<table>
    <tr>
        <th>Wildcard</th>
        <th>Description</th>
    </tr>
    <tr>
        <td><code>%title%</code></td>
        <td>Video title</td>
    </tr>
    <tr>
        <td><code>%author%</code></td>
        <td>Video author</td>
    </tr>
    <tr>
        <td><code>%pub_date%</code></td>
        <td>Video publication date</td>
    </tr>
    <tr>
        <td><code>%id%</code></td>
        <td>Video ID</td>
    </tr>
    <tr>
        <td><code>%act_date%</code></td>
        <td>Actual date</td>
    </tr>
    <tr>
        <td><code>%duration%</code></td>
        <td>Video duration</td>
    </tr>
    <tr>
        <td><code>%views%</code></td>
        <td>Video views</td>
    </tr>
</table>

<h2>Issues & Support</h2>
VDownload is completely free and open source. I developing it in my free time for no money. You can contribute to the development of the application by reporting issues and your ideas <a href="https://github.com/mateuszskoczek/VDownload/issues">HERE</a>. Please attach informations about VDownload version which you use and your operating system, when you create a new issue. Also make sure, that your problem is not already described and resolved.

<h2>For developers</h2>
VDownload uses the following libraries:
<ul>
    <li>AngleSharp [<a href="https://anglesharp.github.io/">LINK</a>]</li>
    <li>ConsoleTableExt [<a href="https://github.com/minhhungit/ConsoleTableExt">LINK</a>]</li>
    <li>FFMpegCore [<a href="https://github.com/rosenbjerg/FFMpegCore">LINK</a>]</li>
    <li>Instances [<a href="https://github.com/rosenbjerg/Instances">LINK</a>]</li>
    <li>YoutubeExplode [<a href="https://github.com/Tyrrrz/YoutubeExplode">LINK</a>]</li>
    <li>...and my 5 libraries, which are included in repository with the libraries above.</li>
</ul>
FFmpeg is not included in repository, because of file size limitations. You have to download FFmpeg by yourself and put executables in "ffmpeg" directory.