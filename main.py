from Diaries import Diary

file = './data/diaries-txts/DSCD-12-PL-170.txt'

print(file)
diary = Diary(file)

poits = diary.get_points()
print(poits)

debates = diary.get_debates()
for d in range(len(debates)):
    debate = debates[d]
    print(debate.title)