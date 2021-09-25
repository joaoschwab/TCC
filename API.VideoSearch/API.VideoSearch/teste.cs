using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.VideoSearch
{
    public class teste
    {

        public IEnumerable<string> tester()
        {
            var requestarray = new List<string>();
            string path = @"MyTest.txt";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Hello");
                    sw.WriteLine("And");
                    sw.WriteLine("Welcome");
                }
            }
            string s = "";
            // Open the file to read from.
            string[] lines = System.IO.File.ReadAllLines(path);


            foreach (string line in lines)
            {

                requestarray.Add(line);
            }
            
            return requestarray;

        }
    }
}
