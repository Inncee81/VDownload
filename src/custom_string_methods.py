"""
Custom String Methods 1.0 (Build 21144)
by Mateusz Skoczek

My custom string methods
"""





# Functions

def ifContainsStrFromArrayAND(str, arr):
    for s in arr:
        if s not in str: return False
    return True

def ifContainsStrFromArrayOR(str, arr):
    for s in arr:
        if s in str: return True
    return False