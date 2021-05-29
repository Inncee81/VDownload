"""
VDownload 0.0.1-dev3 (Build 21149)
by Mateusz Skoczek
© February 2021 - June 2021

https://github.com/mateuszskoczek/VDownload
"""





# ------------------------------------ # Import of modules # ------------------------------------ #

# Main modules
import sys as SYS
import os as OSC
import datetime as DTM
import terminaltables as TTB
import ffmpeg as FFM

# Custom modules
import src.pyconfig as CFG
import src.console_output_files_parser as COF
import src.custom_string_methods as CSM
import src.filename_parser as FNP
from src.option_parser import OPP

# Downloading modules
from pytube import YouTube as DWNyoutube





# ------------------------------------ # Global constants # ------------------------------------- #

class GLOBAL:
    # Infomations about program
    class programInfo:
        name = 'VDownload'
        version = '0.0.1-dev3'
        build = '21149'
        repository = 'https://github.com/mateuszskoczek/VDownload'
        authors = {
            'Mateusz Skoczek': 'https://github.com/mateuszskoczek',
        }
        date = (
            'February 2021',
            'June 2021'
        )
    
    # Important paths
    class paths:        
        config = '%s\%s\config.cfg' % (
            OSC.getenv('APPDATA'),
            'VDownload',
        )
        userprof = OSC.getenv('USERPROFILE')
    
    # Fragments of links from individual websites
    class links:
        youtube = (
            'youtube.com/watch',
            'youtu.be',
        )
    
    # Console input
    class input:
        __args = SYS.argv[1:]
        command = __args[0] if bool(__args) else ''
        parameters = __args[1:] if bool(__args[1:]) else []
    
    class youtube:
        video_stream_header = ['ID', 'Codec', 'Resolution', 'FPS', 'Type']
        audio_stream_header = ['ID', 'Codec', 'ABR', 'Type']
COF.GLOBAL = GLOBAL





# ------------------------------------- # Get 'config.cfg' # ------------------------------------ #

CFG = CFG
CFG.filePath = GLOBAL.paths.config
CFG.defaultContent = {
    'filename': r'%title%',
    'output_path': GLOBAL.paths.userprof,
    'video_ext': 'mp4',
    'audio_ext': 'mp3',
}





# -------------------------------- # YouTube downloading module # ------------------------------- #

class YTBE:
    def info(self, url):
        video = self.__getData(url)
        print(COF.get('youtube/info', (
            video['title'],
            video['url'],
            video['author'],
            video['views'],
            video['pub_date'],
            video['length'],
            video['rating'],
            video['thumbnail'],
            TTB.AsciiTable([GLOBAL.youtube.video_stream_header] + [[i] + list(s[:-1]) for i, s in video['video_stream'].items()]).table,
            TTB.AsciiTable([GLOBAL.youtube.audio_stream_header] + [[i] + list(s[:-1]) for i, s in video['audio_stream'].items()]).table,
        )))
    
    def download(self, url, options):
        video = self.__getData(url)
        FNP.data = (
            video['id'],
            video['title'],
            video['pub_date'],
            video['author']
        )
        filename = FNP.get(options['filename'] if 'filename' in options else CFG.R('filename'))
        output_path = options['output_path'] if 'output_path' in options else CFG.R('output_path')
        video_ext = options['video_ext'] if 'video_ext' in options else CFG.R('video_ext')
        audio_ext = options['audio_ext'] if 'audio_ext' in options else CFG.R('audio_ext')
        if 'onlyvideo' in options:
            stream = self.__getVideoStream(video, options)
            print(COF.get('youtube/downloading_start', (
                video['title'],
                video['url']
            ), down = False))
            print(COF.get('youtube/downloading_video', up = False, down = False))
            if stream.subtype == video_ext:
                stream.download(output_path = output_path, filename = filename)
            else:
                stream.download(output_path = output_path, filename = 'temp')
                tempPath = '%s/temp.%s' % (output_path, stream.subtype)
                outPath = '%s/%s.%s' % (output_path, filename, video_ext)
                inFile = FFM.input(tempPath)
                out = FFM.output(inFile, outPath)
                print(COF.get('youtube/converting_file', up = False, down = False))
                try:
                    out.run(quiet = True)
                except FileNotFoundError:
                    print(COF.get('youtube/error_ffmpeg_not_found'))
                    print(COF.get('youtube/removing_temp'), down = False)
                else:
                    print(COF.get('youtube/removing_temp', up = False, down = False))
                OSC.remove(tempPath)
            print(COF.get('youtube/done', up = False))
        elif 'onlyaudio' in options:
            stream = self.__getAudioStream(video, options)
            print(COF.get('youtube/downloading_start', (
                video['title'],
                video['url']
            ), down = False))
            print(COF.get('youtube/downloading_audio', up = False, down = False))
            stream.download(output_path = output_path, filename = 'temp')
            tempPath = '%s/temp.%s' % (output_path, stream.subtype)
            outPath = '%s/%s.%s' % (output_path, filename, audio_ext)
            inFile = FFM.input(tempPath)
            out = FFM.output(inFile, outPath)
            print(COF.get('youtube/converting_file', up = False, down = False))
            try:
                out.run(quiet = True)
            except FileNotFoundError:
                print(COF.get('youtube/error_ffmpeg_not_found'))
                print(COF.get('youtube/removing_temp'), down = False)
            else:
                print(COF.get('youtube/removing_temp', up = False, down = False))
            OSC.remove(tempPath)
            print(COF.get('youtube/done', up = False))
        else:
            streamVideo = self.__getVideoStream(video, options)
            streamAudio = self.__getAudioStream(video, options)
            print(COF.get('youtube/downloading_start', (
                video['title'],
                video['url']
            ), down = False))
            print(COF.get('youtube/downloading_video', up = False, down = False))
            streamVideo.download(output_path = output_path, filename = 'video_temp')
            print(COF.get('youtube/downloading_audio', up = False, down = False))
            streamAudio.download(output_path = output_path, filename = 'audio_temp')
            videoTempPath = '%s/video_temp.%s' % (output_path, streamVideo.subtype)
            audioTempPath = '%s/audio_temp.%s' % (output_path, streamAudio.subtype)

            videoFile = FFM.input(videoTempPath)
            audioFile = FFM.input(audioTempPath)
            outPath = '%s/%s.%s' % (output_path, filename, video_ext)
            out = FFM.output(videoFile, audioFile, outPath)

            print(COF.get('youtube/merging_streams', up = False, down = False))
            try:
                out.run(quiet = True)
            except FileNotFoundError:
                print(COF.get('youtube/error_ffmpeg_not_found'))
                print(COF.get('youtube/removing_temp'), down = False)
            else:
                print(COF.get('youtube/removing_temp', up = False, down = False))
            OSC.remove(videoTempPath)
            OSC.remove(audioTempPath)
            print(COF.get('youtube/done', up = False))
    
    def __getData(self, url):
        video_obj = DWNyoutube(url)
        video = {}
        video['url'] = url
        video['id'] = video_obj.video_id
        video['title'] = video_obj.title
        video['author'] = video_obj.author
        video['views'] = video_obj.views
        video['pub_date'] = str((video_obj.publish_date).date()).replace('-', '.')
        video['length'] = str(DTM.timedelta(seconds = video_obj.length))
        video['rating'] = video_obj.rating
        video['thumbnail'] = video_obj.thumbnail_url
        video['video_stream'] = {}
        video['audio_stream'] = {}
        video_index = 0
        audio_index = 0
        for s in video_obj.streams:
            if s.video_codec and s.audio_codec: continue
            elif s.video_codec:
                video['video_stream'][video_index] = (
                    s.video_codec,
                    s.resolution,
                    s.fps,
                    s.subtype,
                    s
                )
                video_index += 1
            elif s.audio_codec:
                video['audio_stream'][audio_index] = (
                    s.audio_codec,
                    s.abr,
                    s.subtype,
                    s
                )
                audio_index += 1
        return video
    
    def __bestVideoQuality(self, streams):
        streams = sorted(streams, key = lambda x: (int(streams[x][1][:-1]), streams[x][2]))
        return streams[-1]
    
    def __bestAudioQuality(self, streams):
        streams = sorted(streams, key = lambda x: streams[x][1])
        return streams[-1]
    
    def __getVideoStream(self, video, options):
        if 'video' in options:
            try:
                id = int(options['video'])
            except ValueError: 
                print(COF('youtube/error_id_not_int'))
                OSC._exit(0)
        else:
            id = self.__bestVideoQuality(video['video_stream'])
            if id not in video['video_stream']:
                print(COF('youtube/error_id_does_not_exist'))
                OSC._exit(0)
        return video['video_stream'][id][-1]
    
    def __getAudioStream(self, video, options):
        if 'audio' in options:
            try:
                id = int(options['audio'])
            except ValueError: 
                print(COF('youtube/error_id_not_int'))
                OSC._exit(0)
        else:
            id = self.__bestAudioQuality(video['audio_stream'])
            if id not in video['audio_stream']:
                print(COF('youtube/error_id_does_not_exist'))
                OSC._exit(0)
        return video['audio_stream'][id][-1]

YTBE = YTBE()





# ----------------------------------------- # Options # ----------------------------------------- #

class OPTS:
    def option_get(option):
        if option not in CFG.defaultContent: print(COF.get('error/option_doesnt_exist'))
        else: print(COF.get('options/get', (
            option,
            CFG.R(option)
        )))

    def option_set(option, value):
        if option not in CFG.defaultContent: print(COF.get('error/option_doesnt_exist'))
        else:
            CFG.W(option, str(value))
            print(COF.get('options/set', (
                option,
                value
            )))
        




# -------------------------------------- # Main program # --------------------------------------- #

class MAIN:
    def __init__(self):
        if GLOBAL.input.command.lower() == 'about': self.about()
        elif GLOBAL.input.command.lower() == 'info': self.info()
        elif GLOBAL.input.command.lower() == 'download': self.download()
        elif GLOBAL.input.command.lower() == 'option-get': self.option_get()
        elif GLOBAL.input.command.lower() == 'option-set': self.option_set()
        else: self.help()
    
    def help(self):
        print(COF.get('main/help'))
        OSC._exit(0)
    
    def about(self):
        print(COF.get('main/about'))
        OSC._exit(0)
    
    def info(self):
        if not GLOBAL.input.parameters: self.help()
        url = GLOBAL.input.parameters[0]
        if CSM.ifContainsStrFromArrayOR(url, GLOBAL.links.youtube): YTBE.info(url)
        else: print(COF.get('error/wrong_page'))
    
    def download(self):
        if not GLOBAL.input.parameters: self.help()
        url = GLOBAL.input.parameters[0]
        options = OPP(GLOBAL.input.parameters[1:])
        if CSM.ifContainsStrFromArrayOR(url, GLOBAL.links.youtube): YTBE.download(url, options)
        else: print(COF.get('error/wrong_page'))

    def option_get(self):
        if not GLOBAL.input.parameters: self.help()
        option = GLOBAL.input.parameters[0]
        OPTS.option_get(option)

    def option_set(self):
        if len(GLOBAL.input.parameters) < 2: self.help()
        option = GLOBAL.input.parameters[0]
        value = GLOBAL.input.parameters[1]
        OPTS.option_set(option, value)

MAIN()