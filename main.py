from Diaries import Diary
from os import listdir
from os.path import isfile, join
import json
from TextData import TextData
from TextData import TextDataEncoder

txts = [join('./data/diaries-txts/', f) for f in listdir('./data/diaries-txts') if isfile(join('./data/diaries-txts/', f))]
diaries = {f:Diary(f) for f in txts}

data = []
for path in diaries:
    diary = diaries[path]
    # print(path)

    debates = diary.get_debates()
    for debate in debates:
        sps = debate.get_speaches()
        for s in sps:
            data.append(TextData(s.speach, {
                "speacker": s.speacker
            }))

