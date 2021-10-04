using Newtonsoft.Json.Linq;
using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;


namespace API.VideoSearch.Models
{
    public class ScriptPython
    {

        string scriptTrans = @"..\..\PythonScriptsAndFilesTCC\Scripts\TranscricaoPython.py";
        string scriptConvert = @"..\..\PythonScriptsAndFilesTCC\Scripts\ConvertMp4toMp3.py";
        string scriptNormalize = @"..\..\PythonScriptsAndFilesTCC\Scripts\Normalize.py";
        string pythonEXE = @"C:\Users\joaof\AppData\Local\Programs\Python\Python39\python.exe";


        public void runScript(string filename)
        {
            Console.WriteLine("Iniciando execução da transcricao em python!");
            Console.WriteLine(filename);
            var psi = new ProcessStartInfo();
            psi.FileName = pythonEXE;
            psi.Arguments = $"\"{scriptTrans}\" \"{filename}\"";

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            var erros = "";
            var results = "";
            using (var process = Process.Start(psi))
            {
                erros = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }
            Console.WriteLine(erros);
            Console.WriteLine("Transcricao finalizada");
        }
        public void convertMp4toMp3(string path, string tempmp3)
        {
            Console.WriteLine("Iniciando execução da conversao de mp4 para mp3 em python!");
            var psi = new ProcessStartInfo();
            psi.FileName = pythonEXE;
            psi.Arguments = $"\"{scriptConvert}\" \"{path}\" \"{tempmp3}\"";

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            var erros = "";
            var results = "";
            using (var process = Process.Start(psi))
            {
                erros = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }
            Console.WriteLine(erros);
            Console.WriteLine("Conversao finalizada!");
            
        }
        public void Normalize(string path)
        {
            Console.WriteLine("Iniciando execução da normalizacao da trasncricao!");
            var psi = new ProcessStartInfo();
            psi.FileName = pythonEXE;
            psi.Arguments = $"\"{scriptNormalize}\" \"{path}\"";

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            var erros = "";
            var results = "";
            using (var process = Process.Start(psi))
            {
                erros = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }
            Console.WriteLine(erros);
            Console.WriteLine("Normalizacao finalizada!");

        }
    }
}
