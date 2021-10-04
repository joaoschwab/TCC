import nltk
import os
import sys
import re

filename = sys.argv[1]

nltk.download('stopwords')
nltk.download('rslp')
tokenizador = nltk.WhitespaceTokenizer()
radicalizador = nltk.RSLPStemmer()
stopwords = nltk.corpus.stopwords.words('portuguese')



#armazena em txt temporario as palavras normalizadas
with open("../../Transcricoes/" + filename) as f:
    contents = f.read()

text = contents.lower()

text = re.sub(r'[^\w\s]', ' ', text)
text = re.sub("\d+", ' ', text)

texto_tokenizado = tokenizador.tokenize(text)
palavras_radicalizadas = [ ]
for palavra in texto_tokenizado:
  palavra_radicalizada = radicalizador.stem(palavra)
  palavras_radicalizadas.append(palavra_radicalizada)

clean_tokens = []

for token in palavras_radicalizadas:
    if token not in stopwords and len(token) < 20:
        clean_tokens.append(token)

f = open("../../Arquivos/TempNormalizedWords/tempwords.txt", "x")
for i in clean_tokens:
  f.write(i+"\n")
f.close