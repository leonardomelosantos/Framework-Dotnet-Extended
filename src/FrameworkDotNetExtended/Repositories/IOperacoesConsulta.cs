using FrameworkDotNetExtended.Entities;
using System.Collections.Generic;

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
