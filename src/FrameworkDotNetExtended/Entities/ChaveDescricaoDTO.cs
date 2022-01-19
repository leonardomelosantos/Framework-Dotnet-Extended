using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkDotNetExtended.Entities
{
    public class ChaveDescricaoDTO<T> : IEquatable<ChaveDescricaoDTO<T>>
    {
        public T Chave { get; set; }
        public string Descricao { get; set; }

        public ChaveDescricaoDTO()
        {

        }

        public ChaveDescricaoDTO(T chave, string descricao)
        {
            this.Chave = chave;
            this.Descricao = descricao;
        }

        public override string ToString()
        {
            return Descricao;
        }

        public bool Equals(ChaveDescricaoDTO<T> other)
        {
            return this.Chave.Equals(other.Chave);
        }
    }
}
