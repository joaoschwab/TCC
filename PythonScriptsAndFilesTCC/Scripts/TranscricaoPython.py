#!/usr/bin/python
#imports necessarios
import speech_recognition as sr
from pydub import AudioSegment
import os
import time
import sys
import warnings
from unidecode import unidecode
r = sr.Recognizer()
warnings.filterwarnings("ignore")


#funcao para transcrer o audio do video
def reconhecer(caminho):

      temp = sr.AudioFile(caminho)
      with temp as source:
        audio = r.record(source)
      

      try:
        
        #Passa a variável para o algoritmo reconhecedor de padroes
        frase = r.recognize_google(audio,language='pt-BR')
        
        #Retorna a frase pronunciada
        return frase
        
      except sr.UnknownValueError:
        return "Google Speech Recognition could not understand audio"
      except sr.RequestError as e:
        return "Could not request results from Google Speech Recognition service"


#funcao auxiliar para retirar sequencias de palavras
def removeSequencia(lista):
  listatemp = lista[:]
  for i in range(0,len(lista)-1):
    if i == len(lista)-1:
      pass
    else:
      if unidecode(lista[i]) == unidecode(lista[i + 1]):
        listatemp[i] = "*remove*"
  repeticoes = listatemp.count("*remove*")
  for i in range(repeticoes):
    listatemp.remove("*remove*")
  return listatemp


#funcao auxiliar para considerar os acertos para cortar a parte exata da frase
def trataLista(lista2, acertos, transc1):
  count = 0
  listatemp = transc1[0].split(' ') #iniciado a listatemp que é uma lista com a nova transcrição da primeira parte dos 30 segundos
  #print(listatemp)
  #print(lista2)
  for i in range(0,len(listatemp)-1): #tratando a lista1 par que ela passe a usar a parte dela que ja foi corrigida na execao anterior que é a lista2
    #print(listatemp[i] + " " + lista2[0] + " " + lista2[1] + " " + lista2[2] + " " + lista2[3] + " " + lista2[4])
    if i+4 == len(listatemp)-1:
      return lista2
    else:
      if unidecode(listatemp[i]) == unidecode(lista2[0]):
        count += 1
      if unidecode(listatemp[i + 1]) == unidecode(lista2[1]):
        count += 1
      if unidecode(listatemp[i + 2]) == unidecode(lista2[2]):
        count += 1
      if unidecode(listatemp[i + 3]) == unidecode(lista2[3]):
        count += 1
      if unidecode(listatemp[i + 4]) == unidecode(lista2[4]):
        count += 1
      #print(count)
      if count >= acertos:
        #print("===================================")
        #print("===================================")
        return listatemp[i:]
      else:
        count = 0
  return lista2



#funcao para transcrer o audio
def transcreveAudio(audio):
  
  #pegando o tamanho do audio em segundos
  audioTotal = AudioSegment.from_file(audio)
  fimloop = audioTotal.duration_seconds

  if os.path.isfile("../../Transcricoes/" + sys.argv[1] + ".txt"):
    os.remove("../../Transcricoes/" + sys.argv[1] + ".txt")
  #else:
  #  print("arquivo txt nao criado ainda!")

#================== Definições ====================

  # Tempo inicial para iniciar o loop da transcricao em milisegundos
  start = 0 # valores fixos ao iniciar o codigo
  end = 15000 # valores fixos ao iniciar o codigo
  aux_time = 15000 # valores fixos ao iniciar o codigo
  startFormatado = 0
  stop = 0
  transc1 = []
  transc2 = []
  listaComparacao = []
  contador = 0
  lista30 = []
  tempoInicial30 = 0
  tempoFinal30 = 0
  index = 0
  listatemp = []
  rollBack = 5000
  stringLista1 = ""
  stringLista2 = ""

#================== execução do loop ===============

  while end <= fimloop*1000 and stop < 2: #euquanto o end for menor que o final do audio
    if os.path.isfile("../../PythonScriptsAndFilesTCC/Temp/temp.wav"): #exclui o arquivo temporario da transcrição
      os.remove("../../PythonScriptsAndFilesTCC/Temp/temp.wav")
    #else:
      #print("arquivo nao criado ainda!")

    if contador == 0 and index >= 2:
      start = start - rollBack # 13
    

      

    song = AudioSegment.from_wav(audio)
    extract = song[start:end]
    extract.export("../../PythonScriptsAndFilesTCC/Temp/temp.wav", format="wav")
    if start == 0: #para a primeira execucao quando start for 0
      startFormatado = 0
    else: # se nao o start e dividido por 1000 para convertelo em segundos
      startFormatado = start/1000

    if contador == 0: #primeira parte do count
      if index >= 2: #se a primeira execucao ja estiver sido feito ele entre aqui
        transc1.append(reconhecer('../../PythonScriptsAndFilesTCC/Temp/temp.wav'))
        transc1.append(str(startFormatado + rollBack/1000) + ":" + str(end/1000))
      else:
        transc1.append(reconhecer('../../PythonScriptsAndFilesTCC/Temp/temp.wav'))
        transc1.append(str(startFormatado) + ":" + str(end/1000))
    else:
      transc2.append(reconhecer('../../PythonScriptsAndFilesTCC/Temp/temp.wav'))
      transc2.append(str(startFormatado) + ":" + str(end/1000))

      #print(transc1)
      #print(transc2)


    contador = contador + 1
    index = index + 1 


    #print("contador " + str(contador))

    if contador == 1:
      start = end
      if end + aux_time > fimloop*1000:
        end = fimloop*1000
        stop = stop + 1
        #print("caiu aqui" + str(stop))
        #print("esse é o contador" + str(contador))
        
      else:
        end = end + aux_time
      
    
    if contador == 2:


      lista30 = []
      listaComparacao = []
      stringLista1 = ""
      stringLista2 = ""
      if index > 2:
        tempoInicial30 = tempoFinal30 - 20000

        lista1 = trataLista(lista2, 3, transc1)
        lista2 = transc2[0].split(' ')
        lista2 = removeSequencia(lista2)
        lista1 = removeSequencia(lista1)
            
      else:
        lista1 = transc1[0].split(' ')
        tempoInicial30 = float(transc1[1].split(':')[0]) * 1000
        lista2 = transc2[0].split(' ')
        lista2 = removeSequencia(lista2)
        lista1 = removeSequencia(lista1)
      

      listaComparacao.append(lista1 + lista2)
      tempoFinal30 = float(transc2[1].split(':')[1]) * 1000

      #print("tempo inicial e tempo final = " + str(tempoInicial30) + " " + str(tempoFinal30))
      

      if os.path.isfile("../../PythonScriptsAndFilesTCC/Temp/temp30.wav"): #exclui o arquivo temporario da transcrição
        os.remove("../../PythonScriptsAndFilesTCC/Temp/temp30.wav")
      #else:
        #print("arquivo nao criado ainda!")
      song = AudioSegment.from_wav(audio)
      extract = song[tempoInicial30:tempoFinal30]
      extract.export("../../PythonScriptsAndFilesTCC/Temp/temp30.wav", format="wav")

      lista30.append(reconhecer('../../PythonScriptsAndFilesTCC/Temp/temp30.wav').split(' '))

      indexInicial = 0
      indexFinal = 0
      contadorComparador = 0

      if index > 2:
        #tratando a lista 30 para começar a partir do começo da lista1 apos a primeira execução
        #print("tratando lista30")

        lista30 = removeSequencia(lista30) #funcao para retirar sequencias

        for i in range(0,len(lista30[0])):
          if unidecode(lista30[0][i]) == unidecode(lista1[0]) and unidecode(lista30[0][i + 1]) == unidecode(lista1[1]) and unidecode(lista30[0][i + 2]) == unidecode(lista1[2]) and unidecode(lista30[0][i + 3]) == unidecode(lista1[3]) and unidecode(lista30[0][i + 4]) == unidecode(lista1[4]) and unidecode(lista30[0][i+5]) == unidecode(lista1[5]) and unidecode(lista30[0][i+6]) == unidecode(lista1[6]):
            lista30[0] = lista30[0][i:]
            break


    
      for i in range(len(listaComparacao[0])):
        if listaComparacao[0][i] != lista30[0][i]:
          indexInicial = i
          break
      for i in range(indexInicial,len(lista1)):
        lista1[i] = lista30[0][i]

      lista2 = lista30[0][len(lista1):len(lista30[0])]


      #print(lista1)
      #print(lista2)
      #print(lista30)


      

      contador = 0
      
      for i in lista1:
        stringLista1 = stringLista1 + " " + i
      #print("esse aqui é o end " + str(end) + " e o fimloop é " + str(fimloop*1000))
      with open("../../Transcricoes/" + sys.argv[1] + ".txt", "a") as file:
        file.write(stringLista1 + " / " + transc1[1] + "\n")
        arrayrequest.append(stringLista1 + " / " + transc1[1] )#+ "\n")
        #print((stringLista1 + " / " + transc1[1]) + "\n")
        if end == fimloop*1000 or stop == 2:
          for i in lista2:
            stringLista2 = stringLista2 + " " + i
          file.write(stringLista2 + " / " + transc2[1] + "\n")
          arrayrequest.append(stringLista2 + " / " + transc2[1]) #+ "\n")
          #print((stringLista2 + " / " + transc2[1]) + "\n")
        file.close


      transc1 = []
      transc2 = []

arrayrequest = []
stringrequest =''
audio1 = '../../Arquivos/ArquivoWAV/' + sys.argv[1] + '.wav'
transcreveAudio(audio1)
#for i in arrayrequest:
#  stringrequest = stringrequest + i
#print(arrayrequest)
#print("terminou a execucao")
#f = open('C:/Users/joaof/OneDrive/Documentos/TCCVideoSearch/Arquivos/guardaInfo.txt', 'r')
#file_contents = f.read()
#print(file_contents)
#f.close
