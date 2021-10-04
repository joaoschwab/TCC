using API.VideoSearch.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using API.VideoSearch.Service;
using System.IO;
using NReco.VideoConverter;
using NAudio.Wave;
using Newtonsoft.Json.Linq;

namespace API.VideoSearch.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ScriptController : ControllerBase
    {

        private readonly IStorageService _storageService;
        public ScriptController(IStorageService storageService)
        {
            this._storageService = storageService;
        }

        [HttpPost("trans")]

        public void PostTranscript(IFormFile file)
        {
            ScriptPython script = new ScriptPython();
            string path = @"..\..\Arquivos\ArquivoMP4\" + file.FileName;
            string filenamenoformat = file.FileName.Substring(0, file.FileName.IndexOf("."));
            string tempmp3 = @"..\..\Arquivos\TempMP3\" + file.FileName.Substring(0, file.FileName.IndexOf("."));
            string outputwav = @"..\..\Arquivos\ArquivoWAV\" + file.FileName.Substring(0, file.FileName.IndexOf("."));
            if (System.IO.File.Exists(@"..\..\Trancricoes\" + filenamenoformat + ".txt") == true)
            {

            }
            else
            {
                if (System.IO.File.Exists(path) == false)
                {
                    using (Stream fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }

                if (System.IO.File.Exists(outputwav + ".wav") == false)
                {
                    script.convertMp4toMp3(filenamenoformat + ".mp4", filenamenoformat + ".mp3");

                    using (Mp3FileReader mp3 = new Mp3FileReader(tempmp3 + ".mp3"))
                    {
                        using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                        {
                            WaveFileWriter.CreateWaveFile(outputwav + ".wav", pcm);
                        }

                    }
                    System.IO.File.Delete(tempmp3 + ".mp3");
                }
                var requestarray = new List<string>();
                script.runScript(filenamenoformat);

                script.Normalize(filenamenoformat + ".txt");

                var pathtempwords = @"..\..\Arquivos\TempNormalizedWords\tempwords.txt";
                var pathBanco = @"..\..\BancoPalavras\BancoPalavras.json";
                var jsonbanco = System.IO.File.ReadAllText(pathBanco);
                var palavras = JsonConvert.DeserializeObject<List<Banco>>(jsonbanco);

                List<string> listaTags = new List<string>();


                string[] lines = System.IO.File.ReadAllLines(pathtempwords, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var normalizedwords = new List<string>();

                foreach (string line in lines)
                {
                    normalizedwords.Add(line);
                }


                
                for (int i = 0; i < palavras.Count(); i++)
                {
                    if(normalizedwords.Contains(palavras[i].palavra_normalized))
                    {
                        listaTags.Add(palavras[i].palavra);
                        Console.WriteLine("achou a palavra " + palavras[i].palavra);
                    }
                    
                }

                if (System.IO.File.Exists(@"..\..\Tags\tags.json") == false)
                {
                    var arrayzero = new List<Tags>();

                    if (listaTags.Count() == 0)
                    {
                        List<string> notag = new List<string>();
                        notag.Add("No tags");
                        arrayzero.Add(new Tags(filenamenoformat, notag));
                        
                    }
                    else
                    {
                        
                        Tags t = new Tags(filenamenoformat, listaTags);
                        arrayzero.Add(t);
                        Console.WriteLine("adicionado a tags o video: " + t.nome);
                    }
                    
                    var jsonToOutputzero = JsonConvert.SerializeObject(arrayzero, Formatting.Indented);
                    System.IO.File.WriteAllText(@"..\..\Tags\tags.json", jsonToOutputzero);
                }
                else
                {
                    var tagpath = @"..\..\Tags\tags.json";
                    var jsontag = System.IO.File.ReadAllText(tagpath);
                    var tags = JsonConvert.DeserializeObject<List<Tags>>(jsontag);

                    if (listaTags.Count() == 0)
                    {
                        List<string> notag = new List<string>();
                        notag.Add("No tags");
                        tags.Add(new Tags(filenamenoformat, notag));
                    }
                    else
                    {
                        tags.Add(new Tags(filenamenoformat, listaTags));

                    }
                    jsontag = JsonConvert.SerializeObject(tags);
                    System.IO.File.WriteAllText(tagpath, jsontag);
                }

                System.IO.File.Delete(@"..\..\Arquivos\TempNormalizedWords\tempwords.txt");

            }
        }

        [HttpPost]
        [Route("addpalavra")]
        public void addpalavra(string palavra, string palavra_normalized)
        {
            if (System.IO.File.Exists(@"..\..\BancoPalavras\BancoPalavras.json") == false)
            {
                var arrayzero = new List<Banco>();

                arrayzero.Add(new Banco(palavra, palavra_normalized));

                var jsonToOutputzero = JsonConvert.SerializeObject(arrayzero, Formatting.Indented);
                System.IO.File.WriteAllText(@"..\..\BancoPalavras\BancoPalavras.json", jsonToOutputzero);
            }
            else
            {
                var path = @"..\..\BancoPalavras\BancoPalavras.json";
                var json = System.IO.File.ReadAllText(path);
                var people = JsonConvert.DeserializeObject<List<Banco>>(json);
                people.Add(new Banco(palavra, palavra_normalized));
                json = JsonConvert.SerializeObject(people);
                System.IO.File.WriteAllText(path, json);
            }
        }
        [HttpPost]
        [Route("removepalavra")]
        public void removepalavra(string palavra)
        {
            var path = @"..\..\BancoPalavras\BancoPalavras.json";
            var json = System.IO.File.ReadAllText(path);
            var palavras = JsonConvert.DeserializeObject<List<Banco>>(json);

            Banco palavraNoBanco = palavras.SingleOrDefault(x => x.palavra == palavra);
            if (palavraNoBanco != null)
            {
                palavras.Remove(palavraNoBanco);
            }
                

            //for (int i = 0; i < palavras.Count; i++)
            json = JsonConvert.SerializeObject(palavras);
            System.IO.File.WriteAllText(path, json);

        }


        //string[] lines = System.IO.File.ReadAllLines(@"..\..\Transcricoes\" + filenamenoformat + ".txt", System.Text.Encoding.GetEncoding("iso-8859-1"));


        //foreach (string line in lines)
        //{

        //requestarray.Add(line);
        //}
        //System.IO.File.Delete(@"..\..\Transcricoes\" + filenamenoformat + ".txt");
        //return requestarray;



        //[HttpPost]
        //[Route("uploadAzure")]
        //public IActionResult UploadAzure(IFormFile file)
        //{

        //    _storageService.Upload(file);


        //    return Ok();
        //}

        //[HttpGet]
        //[Route("get")]
        //public async Task<IActionResult> Read(string filename)
        //{
        //    var fileData = await _storageService.Read(filename);
        //    return File(fileData, "video/wav");
        //}

        [HttpGet]
        [Route("trans")]
        public IEnumerable<string> GetTranscript(string filenamenoformat)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\Transcricoes\" + filenamenoformat + ".txt", System.Text.Encoding.GetEncoding("iso-8859-1"));

            var requestarray = new List<string>();

            foreach (string line in lines)
            {
                requestarray.Add(line);
            }

            return requestarray;
        }

        [HttpGet]
        [Route("gettag")]
        public string GetTag()
        {
            var path = @"..\..\Tags\tags.json";
            var tags = System.IO.File.ReadAllText(path);
            //var tags = JsonConvert.DeserializeObject<List<Tags>>(json);

            return tags;
        }

    }
}