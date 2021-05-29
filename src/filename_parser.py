"""
Filename Parser
by Mateusz Skoczek

Module, that converts filename code to string
"""

"""
{id}
{title}
{pub_date}
{author}
"""

data = ('#', '#', '#', '#')
forbidden_chars = ('/', ':', '*', '?', '"', '<', '>', '|')

def get(fn_code):
    filename = fn_code
    filename = filename.replace(r'%id%', data[0])
    filename = filename.replace(r'%title%', data[1])
    filename = filename.replace(r'%pub_date%', data[2])
    filename = filename.replace(r'%author%', data[3])
    for c in forbidden_chars:
        filename = filename.replace(c, '')
    return filename