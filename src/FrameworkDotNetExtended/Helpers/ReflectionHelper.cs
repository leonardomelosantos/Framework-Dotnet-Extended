using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkDotNetExtended.Helpers
{
    public sealed class ReflectionHelper
    {
        /// <summary>
        /// Obtém o valor da propriedade do objeto.
        /// </summary>
        /// <param name="o">objeto a ser reflexionado</param>
        /// <param name="propriedade">propriedade a ser obtida os atributos e metadados</param>
        /// <returns>objeto com valor dos atributos e metadados da propriedade</returns>
        public static Object ObterValorPropriedade(Object o, string propriedade)
        {
            Type tipoObjeto = o.GetType();
            PropertyInfo propertyInfo;
            Object retorno = o;
            string nomePropriedade;
            int posicaoPonto = propriedade.IndexOf('.');

            do
            {
                tipoObjeto = retorno.GetType();

                if (posicaoPonto > -1)
                {
                    nomePropriedade = propriedade.Substring(0, posicaoPonto);
                    propriedade = propriedade.Remove(0, posicaoPonto + 1);
                }
                else
                {
                    nomePropriedade = propriedade;
                    propriedade = "";
                }

                propertyInfo = tipoObjeto.GetProperty(nomePropriedade);

                if (propertyInfo != null)
                {
                    retorno = propertyInfo.GetValue(retorno, null);

                    posicaoPonto = propriedade.IndexOf('.');
                }
                else
                {
                    retorno = null;
                }
            } while (((posicaoPonto > -1) || (propriedade != "")) && (retorno != null));

            return retorno;
        }

        /// <summary>
        /// Obtém um array de valores das propriedades do objeto.
        /// </summary>
        /// <param name="o">objeto a ser reflexionado</param>
        /// <param name="propriedades">array de valores para obter propriedades</param>
        /// <returns>array de objetos contendo os valores dos atributos e metadados da propriedade</returns>
        public static Object[] ObterValoresPropriedades(Object o, string[] propriedades)
        {
            Object[] retorno = new object[propriedades.Length];

            for (int i = 0; i < propriedades.Length; i++)
            {
                retorno[i] = ObterValorPropriedade(o, propriedades[i]);
            }

            return retorno;
        }

        /// <summary>
        /// Atribui um valor a propriedade do objeto.
        /// </summary>
        /// <param name="o">objeto a ser reflexionado</param>
        /// <param name="propriedade">nome da propriedade a ser atribuído os valores</param>
        /// <param name="valorPropriedade">valor da propriedade a ser atribuída</param>
        public static void AtribuirValorPropriedade(Object o, string propriedade, Object valorPropriedade)
        {
            Type tipoObjeto = o.GetType();
            PropertyInfo propertyInfo;
            Object retorno = o;

            int posicaoPonto = propriedade.LastIndexOf('.');

            if (posicaoPonto > -1)
            {
                retorno = ObterValorPropriedade(o, propriedade.Substring(0, posicaoPonto));
            }

            propertyInfo = retorno.GetType().GetProperty(propriedade.Substring(posicaoPonto + 1));

            try
            {
                propertyInfo.SetValue(retorno, valorPropriedade, null);
            }
            catch { }
        }

        /// <summary>
        /// Adiciona um delegate ao evento do objeto informado.
        /// </summary>
        /// <param name="o">Objeto que possui o evento que será adicionado o delegate. </param>
        /// <param name="evento"> Nome do evento do objeto, onde o delegate será adicionado. </param>
        /// <param name="valorDelegate"> Delegate que será adicionado. </param>
        public static void AdicionarDelegateEvento(Object o, string evento, Delegate valorDelegate)
        {
            EventInfo eventInfo;

            eventInfo = o.GetType().GetEvent(evento);

            eventInfo.AddEventHandler(o, valorDelegate);
        }

        /// <summary>
        /// Executa um método do objeto passado como parâmetro.
        /// </summary>
        /// <param name="o">Objeto que possui o método. </param>
        /// <param name="tipoObjeto">Tipo do objeto cujo método será executado. </param>
        /// <param name="nomeMetodo">Nome do método que será executado. </param>
        /// <param name="parametros">Parâmetros que serão passados. </param>
        /// <returns></returns>
        public static Object ExecutarMetodo(Object o, Type tipoObjeto, string nomeMetodo, params Object[] parametros)
        {
            Object retorno = null;

            retorno = tipoObjeto.InvokeMember(nomeMetodo,
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.InvokeMethod |
                BindingFlags.Static | BindingFlags.GetProperty,
                System.Type.DefaultBinder,
                o,
                parametros);

            return retorno;
        }

        /// <summary>
        /// Cria uma instância do tipo informado.
        /// </summary>
        /// <param name="tipoObjeto">Tipo da classe cuja instância será criada. </param>
        /// <param name="parametros"> Parâmetros do construtor.</param>
        /// <returns></returns>
        public static object CriarObjeto(Type tipoObjeto, params Object[] parametros)
        {
            Object retorno = null;

            retorno = Activator.CreateInstance(tipoObjeto, parametros);

            return retorno;
        }

        /// <summary>
        /// Copia as propriedades públicas de escrita do objeto origem para o objeto destino.
        /// </summary>
        /// <param name="origem">Objeto que contém os dados que serão copiados.</param>
        /// <param name="destino">Objeto que receberá os dados copiados do objeto origem.</param>
        /// <returns></returns>
        public static void Copiar(object origem, object destino)
        {
            Type tipoOrigem = origem.GetType();
            Type tipoDestino = destino.GetType();

            if ((tipoOrigem != tipoDestino) && (!tipoDestino.IsSubclassOf(tipoOrigem)))
            {
                throw new InvalidCastException("destino e origem não pertecem a mesma hierarquia!");
            }

            PropertyInfo[] propriedades = tipoOrigem.GetProperties(BindingFlags.SetProperty |
                BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propriedadeOrigem in propriedades)
            {
                AtribuirValorPropriedade(destino, propriedadeOrigem.Name, ObterValorPropriedade(origem, propriedadeOrigem.Name));
            }
        }

        public static void CopiarObjetosClassesDiferentes(object origem, object destino)
        {
            Type tipoOrigem = origem.GetType();
            Type tipoDestino = destino.GetType();

            PropertyInfo[] propriedades = tipoOrigem.GetProperties(BindingFlags.SetProperty |
                BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propriedadeOrigem in propriedades)
            {
                AtribuirValorPropriedade(destino, propriedadeOrigem.Name, ObterValorPropriedade(origem, propriedadeOrigem.Name));
            }
        }
    }

}
