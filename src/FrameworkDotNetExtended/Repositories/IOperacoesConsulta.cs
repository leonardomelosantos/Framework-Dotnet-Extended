using FrameworkDotNetExtended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkDotNetExtended.Repositories
{
    public interface IOperacoesConsulta<K, E>
        where K : struct
        where E : class
    {
        IList<E> ConsultarTodos();

        E ObterPorId(K id);

        IList<ChaveDescricaoDTO<K>> ConsultarChaveDescricaoTodos();
    }
}
