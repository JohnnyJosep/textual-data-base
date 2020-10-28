from Diaries import Diary

path = './diaries-txts/DSCD-14-PL-6.txt'

diary = Diary(path)

debates = diary.get_debates()
for debate in debates:
    print("______________")
    print(debate.title)
    print("______________")
    speaches = debate.get_speaches()
    for speach in speaches:
        print(speach)
        print('\n')
    print('\n')