using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace FrameworkDotNetExtended.Helpers
{
    #region Enumeradores

    /// <summary>
    /// Enum que indica que tipo de valor será para ser retornado.
    /// </summary>
    public enum TipoValorNumerico
    {
        Moeda,
        Numero,
        Percentual
    }

    /// <summary>
    /// Enum que indica em qual tipo será retornado uma diferença entre duas datas.
    /// </summary>
    public enum TipoDataDiferenca
    {
        Dias,
        Meses,
        Anos
    }

    #endregion Enumeradores

    public sealed class MiscellaneousHelper
    {
        #region Data

        public static int QuantidadeDiasMes(int ano, int mes)
        {
            int dias;

            switch (mes)
            {
                case 1:
                    dias = 31;
                    break;
                case 2:
                    if (Convert.ToInt32(ano.ToString().Substring(2, 2)) % 4 == 0)
                    {
                        dias = 29;
                    }
                    else
                    {
                        dias = 28;
                    }
                    break;
                case 3:
                    dias = 31;
                    break;
                case 4:
                    dias = 30;
                    break;
                case 5:
                    dias = 31;
                    break;
                case 6:
                    dias = 30;
                    break;
                case 7:
                    dias = 31;
                    break;
                case 8:
                    dias = 31;
                    break;
                case 9:
                    dias = 30;
                    break;
                case 10:
                    dias = 31;
                    break;
                case 11:
                    dias = 30;
                    break;
                case 12:
                    dias = 31;
                    break;
                default:
                    dias = 31;
                    break;
            }

            return dias;
        }

        /// <summary>
        /// Método que informa o valor de uma diferença entre duas datas, podendo ser escolhido o tipo
        /// de retorno (dias, meses ou anos).
        /// </summary>
        /// <param name="data"></param>
        /// <param name="data2"></param>
        /// <param name="tipoDataDiferenca"></param>
        /// <returns></returns>
        public static int DiferencaData(DateTime data, DateTime data2, TipoDataDiferenca tipoDataDiferenca)
        {
            int valor = 0;
            DateTime dataMaior;
            DateTime dataMenor;
            int compare = data.CompareTo(data2);

            if (compare >= 0)
            {
                dataMaior = data;
                dataMenor = data2;
            }
            else
            {
                dataMenor = data;
                dataMaior = data2;
            }

            if (!(compare == 0))
            {
                TimeSpan tsData = dataMaior.Subtract(dataMenor);

                switch (tipoDataDiferenca)
                {
                    case TipoDataDiferenca.Dias:
                        valor = tsData.Days;
                        break;
                    case TipoDataDiferenca.Meses:
                        if (dataMaior.Year != dataMenor.Year || dataMaior.Month != dataMenor.Month)
                        {
                            int meses = dataMaior.Month - dataMenor.Month;
                            int dias = tsData.Days;

                            for (int i = dataMenor.Year; i < dataMaior.Year; i++)
                            {
                                meses += 12;
                                TimeSpan tsDataMes = (new DateTime(i, 12, 31)).Subtract(new DateTime(i, 1, 1));
                                dias -= tsDataMes.Days;

                            }

                            if (dataMaior.Month != dataMenor.Month && dias < DateTime.DaysInMonth(dataMenor.Year, dataMenor.Month))
                            {
                                meses--;
                            }

                            valor = meses;
                        }
                        break;
                    case TipoDataDiferenca.Anos:
                        valor = dataMaior.Year - dataMenor.Year;
                        if (dataMenor.Month > dataMaior.Month)
                        {
                            valor--;
                        }
                        else
                        {
                            if (dataMenor.Month == dataMaior.Month)
                            {
                                if (dataMenor.Day > dataMaior.Day)
                                {
                                    valor--;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return valor;
        }

        public static string GetMesExtenso(int mes)
        {
            string mesExtenso = string.Empty;

            switch (mes)
            {
                case 1:
                    mesExtenso = "Janeiro";
                    break;
                case 2:
                    mesExtenso = "Fevereiro";
                    break;
                case 3:
                    mesExtenso = "Março";
                    break;
                case 4:
                    mesExtenso = "Abril";
                    break;
                case 5:
                    mesExtenso = "Maio";
                    break;
                case 6:
                    mesExtenso = "Junho";
                    break;
                case 7:
                    mesExtenso = "Julho";
                    break;
                case 8:
                    mesExtenso = "Agosto";
                    break;
                case 9:
                    mesExtenso = "Setembro";
                    break;
                case 10:
                    mesExtenso = "Outubro";
                    break;
                case 11:
                    mesExtenso = "Novembro";
                    break;
                case 12:
                    mesExtenso = "Dezembro";
                    break;
                default:
                    throw new Exception("@Mês inválido.@");
            }

            return mesExtenso;
        }

        #endregion Data

        #region UtilDiversos

        /// <summary>
        /// Método que obtem um valor de um objeto, serve para tratar os valores nulos.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string ObterValor(object valor)
        {
            string valorRetornado = string.Empty;

            if (valor != null && valor.GetType() != typeof(DBNull))
            {
                valorRetornado = valor.ToString();
            }

            return valorRetornado.Trim();
        }

        /// <summary>
        /// Método que retorna Não caso o objeto seja nulo ou Sim caso contrário.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns>Sim ou Não</returns>
        public static string ObterValorSimNao(object valor)
        {
            string valorRetornado = "Não";

            if (valor != null && valor.GetType() != typeof(DBNull) && valor.ToString().Trim() != string.Empty)
            {
                valorRetornado = "Sim";
            }

            return valorRetornado;
        }

        /// <summary>
        /// Método que retorna Não caso o objeto seja nulo ou Sim caso contrário.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns>Sim ou Não</returns>
        public static string ObterValorSimNao(bool valor)
        {
            string valorRetornado = "Não";

            if (valor)
            {
                valorRetornado = "Sim";
            }

            return valorRetornado;
        }

        /// <summary>
        /// Método que retorna true caso o objeto tenha o valor 'Sim' e retorna false
        /// caso contrário.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns>true e false</returns>
        public static bool ObterValorTrueFalse(object valor)
        {
            bool valorRetornado = false;

            if (valor != null && valor.GetType() != typeof(DBNull) && valor.ToString().Equals("Sim"))
            {
                valorRetornado = true;
            }

            return valorRetornado;
        }

        public static bool ObterValorTrueFalse(object valor, bool valorNulo)
        {
            bool valorRetornado = valorNulo;

            if (valor != null && valor.GetType() != typeof(DBNull))
            {
                if (valor.ToString().Equals("Sim"))
                {
                    valorRetornado = true;
                }
                else
                {
                    valorRetornado = false;
                }
            }

            return valorRetornado;
        }



        /// <summary>
        /// Método para tratar as mensagens de exceçao
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string TratarMsgException(string msg, Dictionary<string, string> valoresException, string stackTrace)
        {
            string[] msgs = msg.Split('@');
            string msgFinal = string.Empty;

            if (msgs.Length > 1)
            {
                msgFinal = msgs[1];
            }
            else
            {
                if (msg.ToLower().Contains("timeout"))
                {
                    msgFinal = "Nossos servidores estão sobrecarregados. Tente novamente dentro de alguns minutos.";
                }
                else if (msg.ToLower().Contains("unable to connect"))
                {
                    msgFinal = "Nossos servidores estão sobrecarregados. Tente novamente dentro de alguns minutos.";
                }
                else if (msg.ToLower().Contains("timed out"))
                {
                    msgFinal = "Nossos servidores estão sobrecarregados. Tente novamente dentro de alguns minutos.";
                }
                else
                {
                    msgFinal = "Ocorreu um erro inesperado, tente novamente ou entre em contato com uma de nossas unidades.";
                }

                //string erroIgnorado1 = "aborted";
                //string erroIgnorado2 = "timed out";

                //msg = msg.Trim();

                //if (!msg.Contains(erroIgnorado1) && !msg.Contains(erroIgnorado2))
                //{
                //    try
                //    {
                //        string path = valoresException["PATH"];
                //        string urlXml = path + "suporte.xml";
                //        string tipoUsuario = valoresException["TIPO"];

                //        Dictionary<string, string> parametros = new Dictionary<string, string>();

                //        parametros.Add("@PAGINA", valoresException["PAGINA"]);
                //        parametros.Add("@DIA", DateTime.Now.Day.ToString());
                //        parametros.Add("@HORA", DateTime.Now.ToString("HH:mm:ss"));
                //        parametros.Add("@STACK", stackTrace);
                //        parametros.Add("@ERRO", msg);                        

                //        if (tipoUsuario.Equals("ESTUDANTE"))
                //        {
                //            urlXml = path + "suporteEstudante.xml";
                //            parametros.Add("@IDESTUDANTE", valoresException["ID"]);
                //            parametros.Add("@NOME", valoresException["NOME"]);
                //            parametros.Add("@FILIAL", valoresException["IDFILIALCIEE"]);
                //        }
                //        else
                //        {
                //            if (tipoUsuario.Equals("UCE") || tipoUsuario.Equals("IE"))
                //            {
                //                parametros.Add("@IDUCEIE", valoresException["IDUCEIE"]);
                //                parametros.Add("@NOMEUCEIE", valoresException["NOMEUCEIE"]);
                //                parametros.Add("@IDUSUARIO", valoresException["ID"]);
                //                parametros.Add("@NOMEUSUARIO", valoresException["NOME"]);
                //                parametros.Add("@FILIAL", valoresException["IDFILIALCIEE"]);

                //                if (tipoUsuario.Equals("IE"))
                //                {
                //                    urlXml = path + "suporteIE.xml";
                //                }
                //                else
                //                {
                //                    urlXml = path + "suporteUCE.xml";
                //                }
                //            }
                //        }


                //        //Carregando o e-mail de cadastro de vaga de estágio.
                //        XmlDocument xmlDocument = new XmlDocument();
                //        xmlDocument.Load(urlXml);
                //        xmlDocument.SelectNodes("email")[0]["to"].InnerText = valoresException["EMAILSUPORTE"];

                //        //FuncoesEmail.EnviarEmail(xmlDocument, parametros);
                //    }
                //    catch
                //    {
                //    }
                //}
            }

            return msgFinal;
        }

        /// <summary>
        /// Método para tratar as mensagens de exceçao
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string TratarMsgException(string msg)
        {
            string[] msgs = msg.Split('@');
            string msgFinal = string.Empty;

            if (msgs.Length > 1)
            {
                msgFinal = msgs[1];
            }
            else
            {
                if (msg.ToLower().Contains("timeout"))
                {
                    msgFinal = "Nossos servidores estão sobrecarregados. Tente novamente dentro de alguns minutos.";
                }
                else if (msg.ToLower().Contains("unable to connect"))
                {
                    msgFinal = "Nossos servidores estão sobrecarregados. Tente novamente dentro de alguns minutos.";
                }
                else
                {
                    msgFinal = "Ocorreu um problema interno no processamento dos dados. Tente fazer esta operação dentro de alguns minutos.";
                }

                //FuncoesEmail.EnviarEmail("Msg Erro Site", "leonardomelosantos@gmail.com", msg, true, string.Empty);
            }

            return msgFinal;
        }

        /// <summary>
        /// Este método retira as tags HTML de uma string e elimina as quebras de linha.
        /// </summary>
        /// <param name="textoHTML"></param>
        /// <returns></returns>
        public static string ConverteTextoHtmlEmTextoCorrido(string textoHTML)
        {
            string retorno = textoHTML;
            Regex expressaoRegular = new Regex("<[^>]*>");

            retorno = expressaoRegular.Replace(textoHTML, "").Replace("\r", "").Replace("\n", " ");

            return retorno;
        }

        #endregion UtilDiversos

        #region Validação

        public static bool ValidarCPF(string numeroCPF)
        {
            if (numeroCPF == null)
            {
                numeroCPF = string.Empty;
            }

            bool valido = false;
            numeroCPF = numeroCPF.Replace(".", "").Replace("-", "").Replace("/", "");

            if (numeroCPF.Length == 11)
            {
                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                string tempCpf;
                string digito;
                int soma;
                int resto;

                tempCpf = numeroCPF.Substring(0, 9);
                soma = 0;
                for (int i = 0; i < 9; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = resto.ToString();

                tempCpf = tempCpf + digito;

                soma = 0;
                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = digito + resto.ToString();

                valido = numeroCPF.EndsWith(digito);
            }

            return valido;
        }

        public static bool ValidarCNPJ(string numeroCNPJ)
        {
            if (numeroCNPJ == null)
            {
                numeroCNPJ = string.Empty;
            }

            bool valido = false;
            numeroCNPJ = numeroCNPJ.Replace(".", "").Replace("-", "").Replace("/", "");

            if (numeroCNPJ.Length == 14)
            {
                int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int soma;
                int resto;
                string digito;
                string tempCnpj;

                tempCnpj = numeroCNPJ.Substring(0, 12);

                soma = 0;
                for (int i = 0; i < 12; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

                resto = (soma % 11);
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = resto.ToString();

                tempCnpj = tempCnpj + digito;
                soma = 0;
                for (int i = 0; i < 13; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

                resto = (soma % 11);
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = digito + resto.ToString();

                valido = numeroCNPJ.EndsWith(digito);
            }

            return valido;
        }


        /// <summary>
        /// Verifica se as strings são nulas ou vazias.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public static bool ValidarStringsVazias(params string[] parametros)
        {
            bool retorno = true;

            if (parametros == null)
            {
                retorno = false;
            }
            else
            {
                foreach (string p in parametros)
                {
                    if ((p == null) || (p == string.Empty))
                        retorno = false;
                }
            }

            return retorno;
        }

        #endregion Validação

        #region Formatação



        /// <summary>
        /// Método que formata o número de acordo com o tipo de formatação escolhida
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tipoValorNumerico"></param>
        /// <returns></returns>
        public static string FormatarValorNumerico(Double valor, TipoValorNumerico tipoValorNumerico)
        {
            string valorNumerico = string.Empty;

            switch (tipoValorNumerico)
            {
                case TipoValorNumerico.Moeda:
                    valorNumerico = string.Format("{0:c2}", valor);
                    break;
                case TipoValorNumerico.Numero:
                    valorNumerico = string.Format("{0:n2}", valor); ;
                    break;
                case TipoValorNumerico.Percentual:
                    valorNumerico = string.Format("{0:p2}", valor); ;
                    break;
                default:
                    break;
            }

            return valorNumerico;

        }

        #endregion Formatação

        #region Enumerações

        public static DataTable EnumDataSource(Type tipoEnumerador, string columnName)
        {
            DataTable tbData = new DataTable();
            DataColumn column = new DataColumn(columnName, typeof(string));
            tbData.Columns.Add(column);

            foreach (Enum s in Enum.GetValues(tipoEnumerador))
            {
                DataRow row = tbData.NewRow();
                row[columnName] = s.ToString();
                tbData.Rows.Add(row);
            }

            return tbData;
        }

        public static DataTable EnumDataSourceDesc(Type tipoEnumerador, string columnName, Type tipoClasseDescricao)
        {
            DataTable tbData = new DataTable();
            DataColumn column = new DataColumn(columnName, typeof(string));
            tbData.Columns.Add(column);
            object dscEnum = ReflectionHelper.CriarObjeto(tipoClasseDescricao);

            foreach (Enum s in Enum.GetValues(tipoEnumerador))
            {
                DataRow row = tbData.NewRow();
                row[columnName] = ReflectionHelper.ExecutarMetodo(dscEnum, tipoClasseDescricao, "ToString", s).ToString();
                tbData.Rows.Add(row);
            }

            return tbData;
        }

        #endregion Enumerações

        #region DataBase

        /// <summary>
        /// Método que converte valores vindo do banco para inteiro. Caso seja vazio ou nulo retorna zero.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static int CToInt32(object o)
        {
            int retorno = 0;

            if (o != null && o != DBNull.Value && !o.ToString().Equals(""))
            {
                retorno = Convert.ToInt32(o);
            }

            return retorno;
        }

        #endregion DataBase

        #region Manipulação de arquivos

        /// <summary>
        /// Este método escreve uma string em um arquivo.
        /// </summary>
        /// <param name="caminhoCompleto">Caminho completo do arquivo de destino</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <param name="conteudoQueSeraEscrito">Conteúdo que será inserido no arquivo</param>
        /// <param name="excluirCasoJaExista">Se for TRUE, então o arquivo será excluído caso exista</param>
        public static void EscreverConteudoArquivo(string caminhoCompleto, string nomeArquivo, string conteudoQueSeraEscrito, bool excluirCasoJaExista)
        {
            caminhoCompleto = caminhoCompleto + "\\" + nomeArquivo;
            // Excluindo o arquivo
            if (excluirCasoJaExista)
            {
                System.IO.FileInfo arquivo = new System.IO.FileInfo(caminhoCompleto);
                if (arquivo != null && arquivo.Exists)
                {
                    arquivo.Delete();
                }
            }
            // Criando o arquivo e alimenando-o com o conteúdo HTML
            System.IO.StreamWriter fileStream = new System.IO.StreamWriter(caminhoCompleto, true, Encoding.UTF8);
            fileStream.WriteLine(conteudoQueSeraEscrito);
            fileStream.Flush();
            fileStream.Close();
            fileStream.Dispose();
        }

        #endregion Manipulação de arquivos
    }
}
