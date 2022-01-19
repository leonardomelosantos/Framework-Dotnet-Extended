using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// Enum que indica qual tipo de caracter será para ser filtrado.
    /// </summary>
    public enum TipoCaracter
    {
        Letra,
        Número,
        LetraNúmero
    }


    public static class StringExtensions
    {
        public static string SubStringSafed(this string texto, int tamanho)
        {
            if (texto == null)
            {
                texto = String.Empty;
            }

            if (texto.Length > tamanho)
                texto = texto.Substring(0, tamanho);

            return texto;
        }

        public static string TrimSafed(this string texto)
        {
            if (texto != null)
            {
                return texto.Trim();
            }
            return String.Empty;
        }

        public static string TirarAcentos(this string texto)
        {
            if (texto == null)
            {
                return texto;
            }

            string acentuacao = "ëËÿüÜïÏöÖäÄçÇéÉýÝúÚíÍóÓáÁèÈùÙìÌòÒàÀêÊûÛîÎôÔâÂõÕãÃ¨´`^~";
            string semAcentuacao = "eEyuUiIoOaAcCeEyYuUiIoOaAeEuUiIoOaAeEuUiIoOaAoOaA     ";
            for (int i = 0; i < acentuacao.Length; i++)
            {
                texto = texto.Replace(acentuacao[i].ToString(), semAcentuacao[i].ToString().Trim());
            }

            return texto;
        }

        /// <summary>
        /// Busca todos os index de "value" em uma string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<int> FindAll(this string str, string value)
        {
            var index = 0;

            while (true)
            {
                index = str.IndexOf(value, index + 1, StringComparison.Ordinal);

                if (index == -1)
                {
                    break;
                }

                yield return index;
            }
        }

        public static double ToDoubleAlowPoint(this string valor)
        {
            int index = valor.IndexOf('E');
            if (index > 0)
            {
                valor = valor.Substring(0, index);
            }
            return Double.Parse(
                valor,
                NumberStyles.AllowDecimalPoint,
                NumberFormatInfo.InvariantInfo
            );
        }

        public static decimal ToDecimalAlowPoint(this string valor)
        {
            int index = valor.IndexOf('E');
            if (index > 0)
            {
                valor = valor.Substring(0, index);
            }
            return Decimal.Parse(
                valor,
                NumberStyles.AllowDecimalPoint,
                NumberFormatInfo.InvariantInfo
            );
        }

        /// <summary>
        /// Compara duas strings ignorando o case sensitive e aplicando Trim().
        /// </summary>
        /// <param name="str">String a ser comparada</param>
        /// <param name="value">String a ser comparada</param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string str, string value)
        {
            bool result = false;

            if (str != null && value != null)
            {
                result = str.Trim().Equals(value.Trim(), StringComparison.InvariantCultureIgnoreCase);
            }
            else if (str == null && value == null)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Método que aplica uma máscara específica a um texto.
        /// </summary>
        /// <remarks>
        ///     Na máscara usar:
        ///         # para caracteres não numéricos
        ///         0 para caracteres numéricos
        /// </remarks>
        /// <example>
        ///     texto = "12345678909"
        ///     mascara = "000.000.000-00"
        ///     retorno = "123.456.789-09"
        /// </example>
        /// <param name="texto"></param>
        /// <param name="mascara"></param>
        /// <returns></returns>
        public static string FormatarMascaraTexto(this string texto, string mascara)
        {
            StringBuilder strBuilder = new StringBuilder();
            int i = 0;

            if (texto != null)
            {
                foreach (char c in mascara)
                {
                    if (i < texto.Length)
                    {
                        if (c.Equals('#') || c.Equals('0'))
                        {
                            strBuilder.Append(texto[i]);
                            i++;
                        }
                        else
                        {
                            strBuilder.Append(c);
                        }
                    }
                }
            }

            return strBuilder.ToString();
        }

        /// <summary>
        /// Método que realiza a validação de um CPF
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static bool IsCPFValido(this string cpf)
        {
            bool valido = false;

            try
            {
                cpf = cpf.ApenasNumerosOuTexto(TipoCaracter.Número);

                if (cpf.Length == 11)
                {
                    //CPF's genéricos
                    if (!cpf.Equals("00000000000") && !cpf.Equals("12345678909") &&
                        !cpf.Equals("11111111111") && !cpf.Equals("22222222222") &&
                        !cpf.Equals("33333333333") && !cpf.Equals("44444444444") &&
                        !cpf.Equals("55555555555") && !cpf.Equals("66666666666") &&
                        !cpf.Equals("77777777777") && !cpf.Equals("88888888888") &&
                        !cpf.Equals("99999999999"))
                    {
                        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                        string tempCpf;
                        string digito;
                        int soma;
                        int resto;

                        tempCpf = cpf.Substring(0, 9);
                        soma = 0;
                        for (int i = 0; i < 9; i++)
                            soma += Int32.Parse(tempCpf[i].ToString()) * multiplicador1[i];

                        resto = soma % 11;
                        if (resto < 2)
                            resto = 0;
                        else
                            resto = 11 - resto;

                        digito = resto.ToString();

                        tempCpf = tempCpf + digito;

                        soma = 0;
                        for (int i = 0; i < 10; i++)
                            soma += Int32.Parse(tempCpf[i].ToString()) * multiplicador2[i];

                        resto = soma % 11;
                        if (resto < 2)
                            resto = 0;
                        else
                            resto = 11 - resto;

                        digito = digito + resto.ToString();

                        valido = cpf.EndsWith(digito);
                    }
                }
            }
            catch (Exception)
            {
                valido = false;
            }


            return valido;
        }

        /// <summary>
        /// Método que realiza a validação de um CNPJ
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public static bool ValidarCNPJ(this string cnpj)
        {
            bool valido = false;

            try
            {
                cnpj = cnpj.ApenasNumerosOuTexto(TipoCaracter.Número);

                if (cnpj.Length == 14)
                {
                    int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                    int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                    int soma;
                    int resto;
                    string digito;
                    string tempCnpj;

                    tempCnpj = cnpj.Substring(0, 12);

                    soma = 0;
                    for (int i = 0; i < 12; i++)
                        soma += Int32.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

                    resto = (soma % 11);
                    if (resto < 2)
                        resto = 0;
                    else
                        resto = 11 - resto;

                    digito = resto.ToString();

                    tempCnpj = tempCnpj + digito;
                    soma = 0;
                    for (int i = 0; i < 13; i++)
                        soma += Int32.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

                    resto = (soma % 11);
                    if (resto < 2)
                        resto = 0;
                    else
                        resto = 11 - resto;

                    digito = digito + resto.ToString();

                    valido = cnpj.EndsWith(digito);
                }
            }
            catch (Exception)
            {
                valido = false;
            }

            return valido;
        }

        /// <summary>
        /// Verifica se um email informado é um endereço válido de acordo com expressão de validação.
        /// </summary>
        /// <param name="stringToTest">Parametro que contém o endereço de email para validação.</param>
        /// <returns>True, quando o parâmetro não está em branco e contém um endereço de E-mail válido;
        /// senão false.</returns>
        public static bool IsEmailValid(this string stringToTest)
        {
            string MatchEmailPattern =
                  @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
           + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
           + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
           + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

            return !String.IsNullOrWhiteSpace(stringToTest) && Regex.IsMatch(stringToTest, MatchEmailPattern);

            /*
            bool retorno = false;
            Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            Match match = regex.Match(email);

            if (match.Success)
            {
                retorno = true;
            }

            return retorno;
            */
        }

        /// <summary>
        /// Método que verifica se um texto possui uma quantidade mínima de palavras.
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="quantidadeMinima"></param>
        /// <returns></returns>
        public static bool ValidarQuantidadePalavras(this string texto, int quantidadeMinima)
        {
            if (texto == null)
            {
                texto = String.Empty;
            }

            bool retorno = false;

            string[] palavras = texto.Split(' ');

            if (palavras.Length >= quantidadeMinima)
            {
                retorno = true;
            }

            return retorno;
        }

        /// <summary>
        /// Método que retira os caracteres inválidos.
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="tipoCaracter"></param>
        /// <returns></returns>
        public static string ApenasNumerosOuTexto(this string texto, TipoCaracter tipoCaracter)
        {
            if (texto == null)
            {
                texto = String.Empty;
            }

            StringBuilder strBuilder = new StringBuilder();

            if (tipoCaracter == TipoCaracter.Letra)
            {
                foreach (char c in texto)
                {
                    if (Char.IsLetter(c))
                    {
                        strBuilder.Append(c);
                    }
                }
            }
            else
            {
                if (tipoCaracter == TipoCaracter.Número)
                {
                    foreach (char c in texto)
                    {
                        if (Char.IsNumber(c))
                        {
                            strBuilder.Append(c);
                        }
                    }
                }
                else
                {
                    if (tipoCaracter == TipoCaracter.LetraNúmero)
                    {
                        foreach (char c in texto)
                        {
                            if (Char.IsLetter(c) || Char.IsNumber(c))
                            {
                                strBuilder.Append(c);
                            }
                        }
                    }
                }
            }

            return strBuilder.ToString();
        }
    }
}
