"""
Console Output Files Parser
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

def get(path, ARGS = (), **kwargs):
    upSP = True
    downSP = True
    for k, v in kwargs.items():
        if k == 'up': upSP = v
        elif k == 'down': downSP = v
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
    
    return ('\n' if upSP else '') + content + ('\n' if downSP else '')