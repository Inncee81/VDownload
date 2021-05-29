"""
PyConfig
by Mateusz Skoczek

This module implements simple configuration files handling.
"""





# Libaries

import os





# Variables

filePath = '%s/config.cfg' % os.path.dirname(__file__)
defaultContent = {}
createFileIfNotExists = True
keyValueSeparator = ' = '





# Reading

def R_all():
    if not __checkIfFileExists():
        if createFileIfNotExists: __createFile()
        else: raise FileNotFoundError
    data = {}
    file = [line.split(keyValueSeparator) for line in open(filePath).read().strip().split('\n')]
    for line in file:
        try: data[line[0]] = line[1]
        except: continue
    return data

def R_key(key):
    data = R_all()
    if key not in data: return None
    else: return data[key]

R = R_key





# Writing

def W(key, value):
    data = R_all()
    cond1 = (
        type(key) != str
        or
        len(key) == 0
    )
    cond2 = (
        type(value) != str
        or
        len(value) == 0
    )
    if cond1: raise KeyError
    elif cond2: raise ValueError
    data[key] = value
    __saveData(data)





# Other functions

def __checkIfFileExists():
    try: open(filePath)
    except: return False
    else: return True

def __saveData(data):
    with open(filePath, 'w') as w:
        for key in data:
            w.write('%s%s%s\n' % (
                key,
                keyValueSeparator,
                data[key]
            ))

def __createFile():
    os.makedirs(
        os.path.dirname(filePath),
        exist_ok = True
    )
    __saveData(defaultContent)