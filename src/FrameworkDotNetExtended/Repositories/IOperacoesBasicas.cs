using FrameworkDotNetExtended.Entities;
using System.Collections.Generic;

namespace FrameworkDotNetExtended.Repositories
{
    public interface IOperacoesBasicas<K, E>
        where K : struct
        where E : class
    {
        IList<E> ConsultarTodos();

        IList<ChaveDescricaoDTO<K>> ConsultarChaveDescricaoTodos();

        E ObterPorId(K id);

        E Incluir(E entidade);

        void Alterar(E entidade);

        void Excluir(K id);
    }
}
