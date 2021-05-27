"""
VDownload 0.0.1-dev2 (Build 21148)
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

# Custom modules
import src.pyconfig as CFG
import src.console_output_files_parser as COF
import src.custom_string_methods as CSM
from src.filename_parser import get as FNP
from src.option_parser import OPP

# Downloading modules
from pytube import YouTube as DWNyoutube





# ------------------------------------ # Global constants # ------------------------------------- #

class GLOBAL:
    # Infomations about program
    class programInfo:
        name = 'VDownload'
        version = '0.0.1-dev2'
        build = '21148'
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
    
    defaultConfig = {
        'filename': r'{title}',
        'output_path': r'C:/'
    }

COF.GLOBAL = GLOBAL





# ------------------------------------- # Get 'config.cfg' # ------------------------------------ #

CFG = CFG
CFG.filePath = GLOBAL.paths.config
CFG.defaultContent = GLOBAL.defaultConfig





# -------------------------------- # YouTube downloading module # ------------------------------- #

class YTBE:
    def __init__(self, url, options):
        video_obj = DWNyoutube(url)
        video = {
            'Object': video_obj,
            'Url': url,
            'Title': video_obj.title,
            'Author': video_obj.author,
            'Views': video_obj.views,
            'Publish date': str((video_obj.publish_date).date()).replace('-', '.'),
            'Length': str(DTM.timedelta(seconds = video_obj.length)),
            'Rating': video_obj.rating,
            'Thumbnail': video_obj.thumbnail_url,
        }
        streams = {}
        for i, s in enumerate(video_obj.streams):
            data = {}
            data['Type'] = s.type
            data['Codec'] = '/'.join(s.codecs)
            data['Quality'] = '%s/%s' % (s.resolution, str(s.fps)) if data['Type'] == 'video' else s.abr
            data['MIME'] = s.mime_type
            data['Object'] = s
            streams[i] = data
        video['Streams'] = streams

        if 'info' in options: self.info(video)

        if 'stream' in options:
            try: stream = int(options['stream'])
            except ValueError: print(COF.get('error/invalid_stream_id_not_int'))
            else:
                if stream not in video['Streams']: print(COF.get('error/invalid_stream_id_doesnt_exist'))
                else: self.download(stream, video, options)
        elif 'onlyaudio' in options:
            streams = {i:x for i, x in video['Streams'].items() if x['Type'] == 'audio'}
            streams = sorted(streams, key = lambda i: int(streams[i]['Quality'][:-4]))
            self.download(streams[-1], video, options)
        else:
            streams = {i:x for i, x in video['Streams'].items() if x['Type'] == 'video'}
            max_res = str(max([int(x['Quality'].split('/')[0][:-1]) for x in streams.values()])) + 'p'
            streams = {i:s for i, s in streams.items() if s['Quality'].split('/')[0] == max_res}
            max_fps = str(max([int(x['Quality'].split('/')[1]) for x in streams.values()]))
            streams = {i:s for i, s in streams.items() if s['Quality'].split('/')[1] == max_fps}
            self.download(list(streams.keys())[-1], video, options)

    
    def download(self, stream_id, video, options):
        stream = video['Streams'][stream_id]['Object']
        output_path = options['output_path'] if 'output_path' in options else CFG.R('output_path')
        filename = FNP(options['filename'] if 'filename' in options else CFG.R('filename'), video)
        quality = '%s %s' % (
            video['Streams'][stream_id]['Type'],
            video['Streams'][stream_id]['Quality']
        )
        print(COF.get('ytbe/downloading', (
            video['Title'],
            video['Url'],
            quality
        )))
        stream.download(output_path = output_path, filename = filename)
        
            
    def info(self, video):
        print(COF.get(
            'ytbe/info',
            (
                video['Title'],
                video['Url'],
                video['Author'],
                video['Views'],
                video['Publish date'],
                video['Length'],
                video['Rating'],
                video['Thumbnail'],
                TTB.AsciiTable(
                    [['ID'] + list(video['Streams'][0].keys())[:-1]] + [[i] + list(s.values())[:-1] for i, s in video['Streams'].items()]
                ).table
            )
        ))
        OSC._exit(0)

        




# -------------------------------------- # Main program # --------------------------------------- #

class MAIN:
    def __init__(self):
        if GLOBAL.input.command.lower() == 'about': self.about()
        elif GLOBAL.input.command.lower() == 'download': self.download()
        else: self.help()
    
    def help(self):
        print(COF.get('main/help'))
        OSC._exit(0)
    
    def about(self):
        print(COF.get('main/about'))
        OSC._exit(0)
    
    def download(self):
        if not GLOBAL.input.parameters: self.help()

        url = GLOBAL.input.parameters[0]
        options = OPP(GLOBAL.input.parameters[1:])

        if CSM.ifContainsStrFromArrayOR(url, GLOBAL.links.youtube): YTBE(url, options)
        else: print(COF.get('error/wrong_page'))

MAIN()