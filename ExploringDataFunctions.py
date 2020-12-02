import subprocess
import os
import random
import string
from nltk.corpus import stopwords
from nltk.corpus.reader.wordnet import WordNetCorpusReader
import numpy as np
import urllib
from bs4 import BeautifulSoup as soup
import unidecode
from nltk.util import ngrams

wncr = WordNetCorpusReader('./wordnet_spa', None)
cache = {}
spanish_stopwords = stopwords.words('spanish')
postagConjunction = 'C'
postagDeterminer = 'D'
postagPronoun = 'P'
postagAdposition = 'S'
postagPunctuation = 'F'

def sentenceTokenizer(text):
    spanish_sentence_tokenizer = nltk.data.load('tokenizers/punkt/spanish.pickle')
    sentences = spanish_sentence_tokenizer.tokenize(text)
    return sentences


def getRandomString(length):
    return ''.join(random.choice(string.ascii_lowercase) for i in range(length))


class FreelingResult:
    def __init__(self, line):
        parts = line.split()
        self.word = parts[0]
        self.lema = parts[1]
        self.postag = parts[2]
        self.probability = parts[3]
        
    def __str__(self):
        return f'{self.word} {self.lema} {self.postag} {self.probability}'
        
        
class Freeling:
    def __init__(self, port = 50005):
        self.port = port
            
    def analyzer(self, text):
        temp = f'{getRandomString(10)}.txt'
        with open(temp, 'w', encoding='utf-8') as f:
            f.write(text)
        command = f'analyzer_client.exe {self.port} < {temp}'
        process = subprocess.Popen(command, stdout=subprocess.PIPE, stderr=None, shell=True)
        out, _ = process.communicate()
        os.remove(temp)
        out = out.decode('utf-8').strip()
        lines = out.splitlines()
        return [FreelingResult(line) for line in lines if line != ""]


def getTokens(freelingResult):
    return [f.word for f in freelingResult]


def filterStopwordsViaPosTagCategory(freelingResult, postTagsCategoryExcluded = [postagConjunction, postagDeterminer, postagPronoun, postagAdposition]):
    filtered = filter(lambda f : not f.postag[0] in postTagsCategoryExcluded, freelingResult)
    return list(filtered)

def containsSomeElement(sublist, list):
    for e in sublist:
        if e in list:
            return True
    return False

def filterStopwordsViaList(freelingResult, stopwords = spanish_stopwords):
    filtered = filter(lambda f: not containsSomeElement(f.word.split('_'), stopwords), freelingResult)
    return list(filtered)

def filterStopwords(freelingResult, postTagsCategoryExcluded = [postagConjunction, postagDeterminer, postagPronoun, postagAdposition], stopwords = spanish_stopwords):
    filtered = filterStopwordsViaPosTagCategory(freelingResult, postTagsCategoryExcluded)
    filtered = filterStopwordsViaList(filtered, stopwords)
    return filtered

def getSynonyms(word):
    wn_synonyms = [ss.name().split('.')[0] for ss in wncr.synsets(word)]
    
    eduacalingo_synonyms = []
    try:
        data = urllib.request.urlopen(f'https://educalingo.com/en/dic-es/{unidecode.unidecode(word)}').read()
        eduacalingo_synonyms = [s.text for s in soup(data, 'html.parser').select('.contenido_sinonimos_antonimos0')[0].select('a')]
    except:
        None
    
    synonyms = np.unique(wn_synonyms + eduacalingo_synonyms)
    return synonyms

def getCacheSynonyms(word):
    word = word.lower()
    if word in cache:
        return cache[word]
    else:
        synonyms = getSynonyms(word)
        cache[word] = synonyms
        return synonyms


def getNGrams(freelingResult, size = 2):
    listNGrams = list(ngrams(freelingResult, size))
    return listNGrams

def containsPunctuationMarks(ngramFreeLing):
    for ng in ngramFreeLing:
        if ng.postag.startswith('F'):
            return True
    return False

def getNGramsFilteringPunctuationMarks(freelingResult, size = 2):
    listNGrams = getNGrams(freelingResult, size)    
    filtered = filter(lambda ng: not containsPunctuationMarks(ng), listNGrams)
    return filtered