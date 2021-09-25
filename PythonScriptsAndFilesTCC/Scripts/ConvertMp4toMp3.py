import os
import sys
from moviepy.editor import *

mp3_file = sys.argv[2]
mp4_file = sys.argv[1]


videoClip = VideoFileClip("../../Arquivos/ArquivoMP4/" + mp4_file)

audioClip = videoClip.audio

audioClip.write_audiofile("../../Arquivos/TempMP3/" + mp3_file)

audioClip.close()
videoClip.close()