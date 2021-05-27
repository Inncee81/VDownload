"""
Console Output Files Parser 1.0 (Build 21147)
by Mateusz Skoczek

Module, that converts a console output file to printable string
"""





# Modules

import codecs as COD





# Variables

GLOBAL = None
outputFilesFolder = 'output'
filesEncoding = 'utf-8'





# Functions

def get(path, ARGS = ()):
    content = COD.open('%s\%s' % (outputFilesFolder, path), 'r', filesEncoding).read()
    ARGS = list(ARGS)

    while True:
        if content.find('{') == -1: break
        start = content.find('{') + 1
        end = content.find('}')
        command = content[start:end]

        if command == '#':
            content = content.replace('{#}', str(ARGS[0]), 1)
            ARGS.pop(0)
        else:
            before = '{%s}' % command
            after = eval(command)
            content = content.replace(before, after)
    
    return '\n' + content + '\n'