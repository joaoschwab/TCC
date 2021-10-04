using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.VideoSearch.Models
{
    public class Tags
    {
        public string nome;
        public List<string> tags;

        public Tags(string _nome, List<string> _tags)
        {
            this.nome = _nome;
            this.tags = _tags;
        }

    }

}
