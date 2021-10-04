using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.VideoSearch.Models
{
    public class Banco
    {
        public string palavra;
        public string palavra_normalized;

        public Banco(string _palavra, string _palavra_normalized)
        {
            this.palavra = _palavra;
            this.palavra_normalized = _palavra_normalized;
        }
    }
}
