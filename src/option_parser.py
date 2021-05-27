"""
PyOptionParser 1.0 (Build 21144)
by Mateusz Skoczek

Simple module, which converts a array of terminal options to dictionary
"""





# Variables

mark = '--'
separator = '='





# Funcion

def OPP(a):
    options = {}
    for str in a:
        if str[:len(mark)] != mark: continue
        if separator in str:
            option, value = str[len(mark):].split(separator)
            if not option or not value: continue
            options[option] = value
        else:
            option = str[len(mark):]
            if not option: continue
            options[option] = None
    return options