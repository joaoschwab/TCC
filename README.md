# TCC

Para rodar esse repositorio é necessario instalar as seguintes bibliotecas python:

pip install SpeechRecognition</br>
pip install pydub</br>
pip install unidecode</br>
pip install moviepy</br>

Tambem é necessario definir no projeto da api dentro do arquivo ScriptPython.cs (/API.VideoSearch/API.VideoSearch/Models) onde o python esta instalado em sua maquina, aqui nesse projeto utilizei o python 3.9.2. (mudar a variavel pythonEXE)

Com essas configurações o projeto deve rodar normalmente.

Na chamada da API é só passar um arquivo mp4 no body e chamada ira retornar a transcrição do audio.

https://localhost:5001/api/Script/trans

![image](https://user-images.githubusercontent.com/15850964/134782030-71fa5634-b613-49e7-866c-680cad5f7b17.png)

