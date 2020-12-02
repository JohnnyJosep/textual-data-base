import re
from os.path import basename, splitext
from Variables import *


class Diary:
    def __init__(self, path):
        with open(path, 'r', encoding='utf-8') as file:
            text = file.read()
            self.path = path
            self.full_text = text

            # Celebrated
            self.celebrated = None
            celebratedSearch = re.search(Celebrated, text)
            if celebratedSearch:
                celebratedFound = celebratedSearch.group()
                self.celebrated = celebratedFound[13:]

            # Legislature
            self.legislature = None
            legislatureSearch = re.search(Legislature, text)
            if legislatureSearch:
                self.legislature = legislatureSearch.group().split()[0]

            # President
            self.president = None
            self.presidentTreatment = None
            presidentSearch = re.search(Presidencia, text)
            if presidentSearch:
                presidentFound = presidentSearch.group()
                if presidentFound.startswith('PRESIDENCIA DEL EXCMO. SR. D. '):
                    self.presidentTreatment = 'Presidente'
                    self.president = presidentFound[30:]
                else:
                    self.presidentTreatment = 'Presidenta'
                    self.president = presidentFound[34:]

            # First day order
            self.firstDayOrder = None
            fdoSearch = re.search(OrdenDelDia, text)
            if fdoSearch:
                fdo = fdoSearch.group()
                fdo = re.sub(r'\n+', ' ', fdo)
                fdo = re.sub(r'\n\?', '\n--', fdo)
                fdo = re.sub(r'( +?)', ' ', fdo)
                fdo = re.sub('—', '--', fdo)
                fdo = re.sub('―', '--', fdo)
                fdo = re.sub('«(.*)?', '', fdo)
                fdo = re.sub('\((.*)?', '', fdo)
                fdo = re.sub('\s*\.\.+\s*\d*', '', fdo)

                self.firstDayOrder = fdo


            openigs = len(re.findall(OpenSession, text))
            session = text
            if openigs % 2 == 0:
                summaryOpenings = int(openigs / 2)
                for i in range(summaryOpenings + 1):
                    headerSearch = re.search(OpenSession, session)
                    searchEnd = headerSearch.end()
                    session = session[searchEnd:len(session)]

            else:
                session_first_page = None
                sessionPageSeach = re.search(OrdenDelDia, text)
                if sessionPageSeach:
                    sessionPageFound = sessionPageSeach.group()
                    sessionFistPageSearch = re.search(LastDigit, sessionPageFound)
                    if sessionFistPageSearch:
                        session_first_page = sessionFistPageSearch.group()

                if session_first_page:
                    startSession = 'Núm. \d+ ' + Date + r' Pág. ' + session_first_page + '\n'
                    session = re.split(startSession, text)[-1]
                headerSearch = re.search(OpenSession, session)
                searchEnd = headerSearch.end()
                session = session[searchEnd:len(session)]


            footer = re.split(EndSession, session)[-1]
            session = session[0:len(session)-len(footer)]

            session = re.sub(OpenSession, '', session)
            session = re.sub(EndSession, '', session)
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
        open_parenthesis = 0
        for line in lines:
            lstrip = line.strip()
            if lstrip == '' or lstrip == '.' or lstrip == '*':
                continue

            if line.upper() == line:
                if last_is_point and not line.startswith('--') and not line.startswith('?'):
                    updated_point = re.sub(r'( +?)', ' ', points[-1] + ' ' + line)
                    points[-1] = updated_point
                    last_is_point = True
                else:
                    if not line.strip().isnumeric() and open_parenthesis == 0:
                        points.append(line)
                        last_is_point = True
                    else:
                        last_is_point = False
            else:
                last_is_point = False

            open_parenthesis = open_parenthesis + len(re.findall('\(', line))
            open_parenthesis = open_parenthesis - len(re.findall('\)', line))

        if len(points) > 0:
            return points
        else:
            return [self.firstDayOrder]

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
            title_pattern = re.sub(r'\?', r'\?', title_pattern)
            title_pattern = re.sub(r'\/', r'\/', title_pattern)
            next_title_pattern = next_title.split('--')[-1]
            next_title_pattern = re.sub(r'^\?', r'--', next_title_pattern)
            next_title_pattern = re.sub(r'\.', r'\.', next_title_pattern)
            next_title_pattern = re.sub(r'\(', r'\(', next_title_pattern)
            next_title_pattern = re.sub(r'\)', r'\)', next_title_pattern)
            next_title_pattern = re.sub(r'\?', r'\?', next_title_pattern)
            next_title_pattern = re.sub(r'\/', r'\/', next_title_pattern)

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
                treatment = None
                if speacker_treatment:
                    treatment = speacker_treatment.group()
                    speacker = speacker[len(treatment):]

                speaches.append(Speach(treatment, speacker, speach))

            sample = sample[len(found):]

        return speaches


class Speach:
    def __init__(self, speackerTreatment, speacker, speach):
        self.speackerTreatment = speackerTreatment.strip()
        self.speacker = speacker
        self.speach = speach

    def __str__(self):
        return self.speacker + ' :> ' + self.speach
