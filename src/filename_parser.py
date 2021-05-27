"""
Filename Parser 1.0 (Build 21148)
by Mateusz Skoczek

Module, that converts filename code to string
"""

"""
{title}
{pub_date}
"""

def get(fn_code, data):
    filename = fn_code
    filename = filename.replace('{title}', data['Title'])
    filename = filename.replace('{pub_date}', data['Publish date'])
    return filename