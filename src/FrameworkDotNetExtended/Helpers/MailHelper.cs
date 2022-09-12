using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FrameworkDotNetExtended.Helpers
{
    public partial class MailHelper
    {
        public static bool ValidarEmailUsandoNslookup(string email)
        {
            bool retorno = false;
            try
            {
                if (email.IsEmailValid())
                {
                    string[] userDominio = email.Split('@');
                    String response = string.Empty;
                    Regex regex = new Regex(@"mail exchanger = (?<server>[^\\\s]+)");

                    ProcessStartInfo info = new ProcessStartInfo("nslookup", "-type=MX " + userDominio[1].Trim().ToUpper());
                    info.UseShellExecute = false;
                    info.RedirectStandardInput = true;
                    info.RedirectStandardOutput = true;

                    Process process = Process.Start(info);
                    StreamReader streamReader = process.StandardOutput;

                    while (streamReader.Peek() > -1)
                    {
                        response = streamReader.ReadLine();
                        Match match = regex.Match(response);

                        if (match.Success)
                        {
                            retorno = true;
                            break;
                        }
                    }
                }
            }
            catch
            {
                retorno = true;
            }

            return retorno;
        }

        private static string LevantarExcecaoEmailsInvalidos(IList<string> listaEmailInvalidos)
        {
            string msg = string.Empty;

            if (listaEmailInvalidos.Count > 0)
            {
                StringBuilder str = new StringBuilder();
                int i = 0;

                str.Append("Os seguintes e-mails não foram enviados: ");
                foreach (string s in listaEmailInvalidos)
                {
                    str.Append(s);
                    i++;
                    if (i <= listaEmailInvalidos.Count - 2)
                    {
                        str.Append(", ");
                    }
                    else
                    {
                        if (i == listaEmailInvalidos.Count - 1)
                        {
                            str.Append(" e ");
                        }
                        else
                        {
                            str.Append(".");
                        }
                    }
                }

                msg = str.ToString();
            }
            else
            {
                msg = "Alguns e-mails não puderam ser enviados por estarem incorretos";
            }

            return msg;
        }
    }

    public class EmailInvalidoException : Exception
    {
        public EmailInvalidoException(string msg)
            : base(msg)
        {
        }
    }
}
