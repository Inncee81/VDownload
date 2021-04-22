"""
# VDownload
"""





# ------------------------------------ # Import of modules # ------------------------------------ #

# Main modules
import sys as SS
import os as OS
import codecs as CD
import terminaltables as TT


# Downloading modules
from pytube import YouTube as DWytb





# ---------------------------------- # Main global constants # ---------------------------------- #

class VAR:
    # Program info
    programName = 'VDownload'
    programVersion = '0.0.1'
    programVersionStage = 'Alpha'
    programVersionBuild = '21112'
    programRepository = 'https://github.com/mateuszskoczek/VDownload'
    programAuthor1 = ['Mateusz Skoczek', 'https://github.com/mateuszskoczek']
    programToW = ['February', '2021', 'April', '2021']

    # Paths
    pathAppdata = OS.getenv('APPDATA')







# ------------------------------------ # Terminal outputs # ------------------------------------- #

OUT = {
    'O0001' : ("""
        List of commands:

        • help                                  - returns list of commands (excalty the one you see now)

        • about                                 - returns informations about this program

        • option-help [specified option]       - returns list of options (or help for specified function if available)

        • option-value <specified option>       - returns value of specified option

        • option-set <option> <value>           - set value for specified options

        • download <link> [options]     - downloads video from specified link
            --info                      - shows information about video (doesn't download video and ignores other options)
            --streams                   - shows a list of available streams (doesn't download video and ignores other options)
            --path=<path>               - path to folder, where video is to be saved (default from settings if not selected)
            --name=<name>               - set filename (default from settings if not selected)
            --s=<id>                    - stream selection (default from settings if not selected)
    """),

    'O0002' : ("""
        %s
        Version: %s (Build %s) (%s Release)
        by %s (%s)
        %s %s - %s %s
        Repository: %s
    """ % (
            VAR.programName,
            VAR.programVersion,
            VAR.programVersionBuild,
            VAR.programVersionStage,
            VAR.programAuthor1[0],
            VAR.programAuthor1[1],
            VAR.programToW[0],
            VAR.programToW[1],
            VAR.programToW[2],
            VAR.programToW[3],
            VAR.programRepository
        )
    ),

    'O0003_0' : ("""
        List of options:

        • advancedErrors                        - (0/1) hide/show advanced info about errors
        • defaultPath                           - default downloading folder path (you can use tags for naming folders - see help for this option)*
        • defaultFilename                       - default file name (you can use tags for naming files - see help for this option)*

        * means that help is available for this option 
    """),

    'O0003_1' : ("""
        Help for "defaultPath" option:

        • Only absolute paths
        • You can use tags for naming folders

        Tags:
        • $title$       - title of the video
        • $author$      - author of the video
    """),

    'O0004_1' : ("""
        "%s" from YouTube [%s]

        Author: %s
        Views: %s
        Publish date: %s
        Length: %s
        Rating: %s
        Thumbnail: %s
    """),


    'E0001' : "Video cannot be downloaded from this page (%s)",
    'E0002' : "Config file cannot be created",
    'E0003' : "Config file cannot be created",
    'E0004' : "Config file cannot be opened",
    'E0005' : 'Option "%s" not found',
    'E0006' : 'Wrong value for option "%s"',

    'I0001' : "'%s' option's value has been changed to '%s'"
}





# ------------------------------------- # Get 'config.cfg' # ------------------------------------ #

class CFG:
    # Default config
    defaultCFG = {
        "advancedErrors": '0',
        "defaultPath" : 'C:/',
        "defaultFilename" : '$title$',
    }


    # Read
    def R(self, key):
        self.__checkIfFileExist()
        try: file = open(('%s\VDownload\config.cfg' % VAR.pathAppdata))
        except Exception as exceptInfo:
            print(
                '\n\tERROR! %s\n\t%s\n'
                % (
                    OUT['E0004'],
                    exceptInfo
                )
            )
            OS._exit(0)
        
        config = {}
        for x in file.read().strip().split('\n'):
            try: 
                key_n, value_n = x.split(' = ')
                config[key_n] = value_n
            except: pass
        
        try: return config[key]
        except: return self.defaultCFG[key]

    # Write
    def W(self, key, value):
        self.__checkIfFileExist()
        try: data, file = (
                open(('%s\VDownload\config.cfg' % VAR.pathAppdata)).read().strip().split('\n'),
                open(('%s\VDownload\config.cfg' % VAR.pathAppdata), 'w')
            )
        except Exception as exceptInfo:
            print(
                '\n\tERROR! %s\n\t%s\n'
                % (
                    OUT['E0004'],
                    exceptInfo
                )
            )
            OS._exit(0)
        
        config = {}
        for x in data:
            try: 
                key_n, value_n = x.split(' = ')
                config[key_n] = value_n
            except: pass
        
        config[key] = value

        for x in config:
            file.write('%s = %s' % (x, config[x]))
        file.close()
        


    # Checking
    def __checkIfFileExist(self):
        if "VDownload" not in list(OS.listdir(VAR.pathAppdata)):
            try: OS.mkdir('%s\VDownload' % VAR.pathAppdata)
            except Exception as exceptInfo: 
                print(
                    '\n\tERROR! %s\n\t%s\n'
                    % (
                        OUT['E0002'],
                        exceptInfo
                    )
                )
                OS._exit(0)
        
        if 'config.cfg' not in list(OS.listdir('%s\VDownload' % VAR.pathAppdata)):
            try:
                file = CD.open('%s\VDownload\config.cfg' % VAR.pathAppdata, 'w', 'utf-8')
                file.write('\n'.join([('%s = %s' % (x, self.__defaultCFG[x])) for x in self.__defaultCFG]))
                file.close()
            except Exception as exceptInfo:
                print(
                    '\n\tERROR! %s\n\t%s\n'
                    % (
                        OUT['E0003'],
                        exceptInfo
                    )
                )
                OS._exit(0)

CFG = CFG()





# -------------------------------- # Youtube downloading module # ------------------------------- #

class YTB:
    def __init__(self, link, options):
        video_obj = DWytb(link)
        video = {
            'object': video_obj,
            'link': link,
            'title': video_obj.title,
            'author': video_obj.author,
            'views': video_obj.views,
            'publish_date': str(video_obj.publish_date).split(' ')[0],
            'length': '%s:%s:%s:%s' % (
                str(video_obj.length//86400),
                str(video_obj.length//3600 - (24*(video_obj.length//86400))).zfill(2),
                str(video_obj.length//60 - (1440*(video_obj.length//86400) + 60*(video_obj.length//3600 - (24*(video_obj.length//86400))))).zfill(2),
                str(video_obj.length - (86400*(video_obj.length//86400) + 3600*(video_obj.length//3600 - (24*(video_obj.length//86400))) + 60*(video_obj.length//60 - (1440*(video_obj.length//86400) + 60*(video_obj.length//3600 - (24*(video_obj.length//86400))))))).zfill(2)
            ),
            'rating': video_obj.rating,
            'thumbnail': video_obj.thumbnail_url,
        }
        streams = {}
        index = 0
        for x in video_obj.streams:
            attributes = str(x).split(' ')[2:]
            type = attributes[-1][6:-2]
            extension = attributes[0][17:-1]
            quality = '%s/%s' % (attributes[1][5:-1], attributes[2][5 if type == 'video' else 8:-1])
            progressive = attributes[-2][13:-1]
            streams[index] = [x, type, extension, quality, progressive]
            index += 1
        video['streams'] = streams

        if "info" in options: self.info(video)
        elif "streams" in options: self.streams(video)
    
    def info(self, video):
        print(OUT['O0004_1'] % (
            video['title'],
            video['link'],
            video['author'],
            video['views'],
            video['publish_date'],
            video['length'],
            video['rating'],
            video['thumbnail']
        ))
    
    def streams(self, video):
        table = [['ID', 'Type', 'Extension', 'Quality', 'Progressive']]
        for x in video['streams']:
            table.append([x] + video['streams'][x][1:])
        
        print()
        print('\t"%s" from YouTube [%s]' % (video['title'], video['link']))
        print()
        print('\tList of streams:')
        for x in str(TT.AsciiTable(table).table).split('\n'):
            print('\t' + x)
        print()
    
    def download(self, video):
        pass





# -------------------------------- # Twitch downloading module # -------------------------------- #

class TTV:
    pass





# --------------------------------------- # Main program # -------------------------------------- #

INPUT = SS.argv[1:]

class MAIN:
    def __init__(self):
        if len(INPUT) == 0 or INPUT[0].lower() == 'help': self.help()
        elif INPUT[0].lower() == 'about': self.about()
        elif INPUT[0].lower() == 'option-help': self.optionHelp()
        elif INPUT[0].lower() == 'option-value': self.optionValue()
        elif INPUT[0].lower() == 'option-set': self.optionSet()
        elif INPUT[0].lower() == 'download': self.download()
        else: self.help()
    

    def help(self):
        print(OUT['O0001'])
    

    def about(self):
        print(OUT['O0002'])
    

    def optionHelp(self):
        if len(INPUT) == 1: print(OUT['O0003_0'])
        elif INPUT[1] == 'defaultPath': print(OUT['O0003_1'])
    

    def optionValue(self):
        if len(INPUT) >= 2: 
            if INPUT[1] in CFG.defaultCFG: print('\n\tKey: "%s"\n\tValue: "%s"\n' % (INPUT[1], CFG.R(INPUT[1])))
            else: print('\n\tERROR! %s\n' % (OUT['E0005'] % INPUT[1]))
    

    def optionSet(self):
        if len(INPUT) <= 3:
            pass
        key = INPUT[1]
        value = ' '.join(INPUT[2:])

        if key not in CFG.defaultCFG:
            print('\n\tERROR! %s\n' % (OUT['E0005'] % key))
            OS._exit(0)
        
        if key in [
            'defaultPath',
            'defaultFilename',
        ]: CFG.W(key, value)
        elif key == 'advancedErrors' and (
            value in ['0', '1']
        ): CFG.W(key, value)
        else:
            print('\n\tERROR! %s\n' % (OUT['E0006'] % key))
            OS._exit(0)
        print('\n\t%s\n' % (OUT['I0001'] % (key, value)))
    

    def download(self):
        link = INPUT[1]

        youtubeCond = (
            'youtube.com/watch' in link
            or
            'youtu.be' in link
        )
        twitchCond = (
            None
        )

        options = {}
        for x in INPUT[2:]:
            if x[:2] != '--': continue
            if '=' in x:
                option, value = x[2:].split('=')
                options[option] = value
            else:
                option = x[2:]
                options[option] = None

        if youtubeCond: YTB(link, options)
        elif twitchCond: TTV(link, options)
        else:
            print('\n\tERROR! %s\n' % (OUT['E0001'] % link))
            OS._exit(0)
            

MAIN()