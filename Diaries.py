import re
from os.path import basename, splitext
from Variables import *


class Diary:
    def __init__(self, path):
        with open(path, 'r', encoding='utf-8') as file:
            text = file.read()
            self.full_text = text

            session = re.split(OpenSession, text)[-1]
            session = re.split(EndSession, session)[0]
            session = re.sub(Header, '', session)
            session = re.sub(NumeroExpediente, '', session)
            cve = splitext(basename(path))[0];
            # leading zeros
            cve = re.sub('-0', '-', cve)
            cve = re.sub('-0', '-', cve)
            session = re.sub('cve: ' + cve, '', session)
            session = re.sub(r'\n+', '\n', session)
            session = re.sub(r'\n\?', '\n--', session)
            session = re.sub(r'( +?)', ' ', session)
            session = re.sub('—', '--', session)
            session = re.sub('―', '--', session)
            self.session_text = session

    def get_points(self):
        lines = self.session_text.split('\n')
        lines = [re.sub(r'^\?', '--', l) for l in lines]
        points = []
        last_is_point = False
        for line in lines:
            if line == '':
                continue

            if line.upper() == line:
                if last_is_point and not line.startswith('--') and not line.startswith('?'):
                    updated_point = re.sub(r'( +?)', ' ', points[-1] + ' ' + line)
                    points[-1] = updated_point
                    last_is_point = True
                else:
                    if not line.isnumeric():
                        points.append(line)
                        last_is_point = True
                    else:
                        last_is_point = False
            else:
                last_is_point = False

        return points

    def get_debates(self):
        session = self.session_text
        points = self.get_points()
        main_point = ''
        main_point_counts = 0
        debates_titles = []
        for point in points:
            if point.startswith('--'):
                debates_titles.append(main_point + ' ' + point)
                main_point_counts = main_point_counts + 1
            elif main_point != '' and main_point_counts == 0:
                debates_titles.append(main_point)
                main_point = point
            else:
                main_point = point

        if main_point_counts == 0:
            debates_titles.append(main_point)

        debates_titles = [re.sub(r'\s+', ' ', dt) for dt in debates_titles]
        plain_session = re.sub(r'\s+', ' ', session)

        debates = []
        i = 0
        while i < len(debates_titles):
            title = debates_titles[i]
            #title = re.sub(r'^\?', r'--', title)
            next_title = '' if i + 1 >= len(debates_titles) else debates_titles[i + 1]
            #next_title = re.sub(r'^\?', r'--', next_title)

            title_pattern = title.split('--')[-1]
            title_pattern = re.sub(r'\.', r'\.', title_pattern)
            title_pattern = re.sub(r'\(', r'\(', title_pattern)
            title_pattern = re.sub(r'\)', r'\)', title_pattern)
            next_title_pattern = next_title.split('--')[-1]
            next_title_pattern = re.sub(r'^\?', r'--', next_title_pattern)
            next_title_pattern = re.sub(r'\.', r'\.', next_title_pattern)
            next_title_pattern = re.sub(r'\(', r'\(', next_title_pattern)
            next_title_pattern = re.sub(r'\)', r'\)', next_title_pattern)

            pattern = title_pattern
            if next_title_pattern == '':
                pattern = pattern + '(.*)?'
            else:
                pattern = pattern + '(.*?)' + next_title_pattern

            match = re.search(pattern, plain_session)
            if match:
                found_text = match.group(0)
                found_text = re.sub('^' + title_pattern, '', found_text)
                if next_title != '':
                    next_title_full_pattern = re.sub(r'\.', r'\.', next_title)
                    next_title_full_pattern = re.sub(r'\(', r'\(', next_title_full_pattern)
                    next_title_full_pattern = re.sub(r'\)', r'\)', next_title_full_pattern)
                    found_text = re.sub(next_title_full_pattern + '$', '', found_text)
                    found_text = re.sub(next_title_pattern + '$', '', found_text)
                found_text = found_text.strip()
                debates.append(Debate(title, found_text))

            i = i + 1

        return debates


class Debate:
    def __init__(self, title, text):
        self.title = title
        self.text = text

    def __str__(self):
        return self.title + '\n' + self.text

    def get_speaches(self):
        sample = self.text
        speaches = []
        while True:
            match = re.search(SpeachPattern, sample)
            if not match:
                break
            found = match.group()
            end_found = re.search(SpeackerPattern + r'$', found)
            if end_found:
                found = found[:-len(end_found.group())]

            speacker = re.search(SpeackerPattern, found).group()
            if speacker:
                speach = found[len(speacker):].strip()
                speacker = speacker[:-1]
                speacker_treatment = re.search(SpeackerTreatmentPattern, speacker)
                if speacker_treatment:
                    speacker = speacker[len(speacker_treatment.group()):]

                speaches.append(Speach(speacker, speach))

            sample = sample[len(found):]

        return speaches


class Speach:
    def __init__(self, speacker, speach):
        self.speacker = speacker
        self.speach = speach

    def __str__(self):
        return self.speacker + ' :> ' + self.speach
