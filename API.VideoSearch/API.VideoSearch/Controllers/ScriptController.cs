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

        public IEnumerable<string> GetTranscript(IFormFile file)
        {

            ScriptPython script = new ScriptPython();
            string path = @"..\..\Arquivos\ArquivoMP4\" + file.FileName; 
            string filenamenoformat = file.FileName.Substring(0, file.FileName.IndexOf("."));
            string tempmp3 = @"..\..\Arquivos\TempMP3\" + file.FileName.Substring(0, file.FileName.IndexOf(".")); 
            string outputwav = @"..\..\Arquivos\ArquivoWAV\" + file.FileName.Substring(0, file.FileName.IndexOf("."));
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


            string[] lines = System.IO.File.ReadAllLines(@"..\..\Transcricoes\" + filenamenoformat + ".txt", System.Text.Encoding.GetEncoding("iso-8859-1"));


            foreach (string line in lines)
            {

            requestarray.Add(line);
            }
            System.IO.File.Delete(@"..\..\Transcricoes\" + filenamenoformat + ".txt");
            return requestarray;
        }


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


       
    }
}
    

