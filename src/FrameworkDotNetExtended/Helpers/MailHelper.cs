using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using FrameworkDotNetExtended.Extensions;

namespace FrameworkDotNetExtended.Helpers
{
    public class MailHelper
    {
        /*private static string nomeFrom = ConfigurationManager.AppSettings["NOMEEMAILSMTP"];
        private static string from = ConfigurationManager.AppSettings["EMAILSMTP"];
        private static string usuarioSMTP = ConfigurationManager.AppSettings["USUARIOSMTP"];
        private static string senhaSMTP = ConfigurationManager.AppSettings["SENHASMTP"];
        private static string servidorSMTP = ConfigurationManager.AppSettings["SERVIDORSMTP"];
        private static string portaServidorSMTP = ConfigurationManager.AppSettings["PORTASERVIDORSMTP"];*/

        /*public static void EnviarEmail(string subject, string enderecoEmail, string emailBody, bool isHtml,
            string copiasEmail)
        {
            try
            {
                string[] copias;
                List<string> listaEmailsInvalidos = new List<string>();

                if (!MiscellaneousHelper.ValidarStringsVazias(from, usuarioSMTP, senhaSMTP, servidorSMTP, portaServidorSMTP))
                    throw new Exception("Configuração de E-mail incorreta ou não informada.");

                MailMessage email = new MailMessage(from, enderecoEmail);
                email.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                email.IsBodyHtml = isHtml;

                if ((copiasEmail != null) && (!copiasEmail.Equals("")))
                {
                    copiasEmail = copiasEmail.Replace("/", ";");
                    copiasEmail = copiasEmail.Replace("..", ".");

                    copias = copiasEmail.Split(';');
                    foreach (string e in copias)
                    {
                        if (!e.Trim().Equals(""))
                        {
                            if (ValidarFormatoEmail(e.Trim()))
                            {
                                email.Bcc.Add(e.Trim());
                            }
                            else
                            {
                                listaEmailsInvalidos.Add(e.Trim());
                            }
                        }
                    }
                }

                email.Subject = subject;
                email.Body = emailBody;
                email.From = new MailAddress(from, nomeFrom);

                SmtpClient smtpEmail = new SmtpClient(servidorSMTP, Convert.ToInt32(portaServidorSMTP));
                smtpEmail.Credentials = new NetworkCredential(usuarioSMTP, senhaSMTP);
                smtpEmail.Send(email);

                if (listaEmailsInvalidos.Count > 0)
                {
                    throw new EmailInvalidoException(LevantarExcecaoEmailsInvalidos(listaEmailsInvalidos));
                }
            }
            catch (EmailInvalidoException emailInvalido)
            {
                throw new Exception("O E-mail foi enviado, mas ocorreu o seguinte erro: " + emailInvalido.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("O E-mail não pôde ser enviado, motivo: " + ex.Message);
            }
        }*/

        /// <summary>
        /// Método que envia um e-mail de um arquivo XML (físico ou em memória)
        /// </summary>
        /// <param name="emailBody"></param>
        /// <param name="parametros">Valores que devem ser substituídos <Chave, Valor></param>
        /// <example>
        ///     KeyValuePair<string, string> keyValue = new KeyValuePair<string, string>("@NOME", "RAFAEL");
        ///     parametros.Add(keyValue);
        /// </example>
        /*public static void EnviarEmail(XmlDocument emailBody, IDictionary<string, string> parametros)
        {
            try
            {
                string fromXml = string.Empty;
                List<string> listaEmailsInvalidos = new List<string>();
                string[] copias;

                if (!MiscellaneousHelper.ValidarStringsVazias(from, usuarioSMTP, senhaSMTP, servidorSMTP, portaServidorSMTP))
                    throw new Exception("Configuração de E-mail incorreta ou não informada.");

                MailMessage email = new MailMessage();
                email.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                email.IsBodyHtml = true;

                foreach (XmlNode no in emailBody.SelectNodes("email")[0].ChildNodes)
                {
                    if (no.Name.ToLower().Equals("to"))
                    {
                        if (no.InnerText.Equals(""))
                            throw new Exception("E-mail de destino não informado.");

                        string to = no.InnerText;
                        if ((to != null) && (!to.Equals("")))
                        {
                            to = to.Replace("/", ";");
                            to = to.Replace("..", ".");

                            copias = to.Split(';');
                            foreach (string e in copias)
                            {
                                if (!e.Trim().Equals(""))
                                {
                                    if (ValidarFormatoEmail(e.Trim()))
                                    {
                                        email.To.Add(e.Trim());
                                    }
                                    else
                                    {
                                        listaEmailsInvalidos.Add(e.Trim());
                                    }
                                }
                            }
                        }

                        
                    }
                    else
                    {
                        if (no.Name.ToLower().Equals("subject"))
                        {
                            email.Subject = no.InnerText;
                        }
                        else
                        {
                            if (no.Name.ToLower().Equals("body"))
                            {
                                if (no.InnerText.Equals(""))
                                    throw new Exception("E-mail não pode estar vazio.");

                                string body = no.InnerText;

                                if ((parametros != null) && (parametros.Count > 0))
                                {
                                    foreach (KeyValuePair<string, string> keyValue in parametros)
                                    {
                                        body = body.Replace(keyValue.Key, keyValue.Value);
                                    }
                                }

                                email.Body = body;
                            }
                            else
                            {
                                if (no.Name.ToLower().Equals("ishtml"))
                                {
                                    try
                                    {
                                        email.IsBodyHtml = Convert.ToBoolean(no.InnerText);
                                    }
                                    catch
                                    {
                                        email.IsBodyHtml = true;
                                    }
                                }
                                else
                                {

                                    if (no.Name.ToLower().Equals("cc"))
                                    {
                                        string cc = no.InnerText;
                                        if ((cc != null) && (!cc.Equals("")))
                                        {
                                            cc = cc.Replace("/", ";");
                                            cc = cc.Replace("..", ".");

                                            copias = cc.Split(';');
                                            foreach (string e in copias)
                                            {
                                                if (!e.Trim().Equals(""))
                                                {
                                                    if (ValidarFormatoEmail(e.Trim()))
                                                    {
                                                        email.CC.Add(e.Trim());
                                                    }
                                                    else
                                                    {
                                                        listaEmailsInvalidos.Add(e.Trim());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (no.Name.ToLower().Equals("cco"))
                                        {
                                            string cco = no.InnerText;
                                            if ((cco != null) && (!cco.Equals("")))
                                            {
                                                cco = cco.Replace("/", ";");
                                                cco = cco.Replace("..", ".");

                                                copias = cco.Split(';');
                                                foreach (string e in copias)
                                                {
                                                    if (!e.Trim().Equals(""))
                                                    {
                                                        if (ValidarFormatoEmail(e.Trim()))
                                                        {
                                                            email.Bcc.Add(e.Trim());
                                                        }
                                                        else
                                                        {
                                                            listaEmailsInvalidos.Add(e.Trim());
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (no.Name.ToLower().Equals("from"))
                                            {
                                                if (!no.InnerText.Equals(""))
                                                    fromXml = no.InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                try
                {
                    if (!fromXml.Equals(""))
                    {
                        email.From = new MailAddress(fromXml);
                    }
                    else
                    {
                        email.From = new MailAddress(from);
                    }
                }
                catch
                {
                    email.From = new MailAddress(from);
                }

                if (email.To.Count == 0)
                    throw new Exception("E-mail sem endereço de destino.");

                SmtpClient smtpEmail = new SmtpClient(servidorSMTP, Convert.ToInt32(portaServidorSMTP));
                smtpEmail.Credentials = new NetworkCredential(usuarioSMTP, senhaSMTP);
                smtpEmail.Send(email);

                if (listaEmailsInvalidos.Count > 0)
                {
                    throw new EmailInvalidoException(LevantarExcecaoEmailsInvalidos(listaEmailsInvalidos));
                }
            }
            catch (EmailInvalidoException emailInvalido)
            {
                throw new Exception("O E-mail foi enviado, mas ocorreu o seguinte erro: " + emailInvalido.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("O E-mail não pôde ser enviado, motivo: " + ex.Message);
            }
        }
*/

        /*public static void EnviarEmail(string subject, string enderecoEmail, string emailBody, bool isHtml,
            string copiasEmail, params List<string>[] arquivosAnexos)
        {
            try
            {
                string[] copias;
                List<string> listaEmailsInvalidos = new List<string>();

                if (!MiscellaneousHelper.ValidarStringsVazias(from, usuarioSMTP, senhaSMTP, servidorSMTP, portaServidorSMTP))
                    throw new Exception("Configuração de E-mail incorreta ou não informada.");

                MailMessage email = new MailMessage(from, enderecoEmail);
                email.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                email.IsBodyHtml = isHtml;

                if ((copiasEmail != null) && (!copiasEmail.Equals("")))
                {
                    copiasEmail = copiasEmail.Replace("/", ";");
                    copiasEmail = copiasEmail.Replace("..", ".");

                    copias = copiasEmail.Split(';');
                    foreach (string e in copias)
                    {
                        if (!e.Trim().Equals(""))
                        {
                            if (ValidarFormatoEmail(e.Trim()))
                            {
                                email.Bcc.Add(e.Trim());
                            }
                            else
                            {
                                listaEmailsInvalidos.Add(e.Trim());
                            }
                        }
                    }
                }

                email.Subject = subject;
                email.Body = emailBody;

                foreach (string arquivo in arquivosAnexos[0])
                {
                    Attachment arquivoAnexado = new Attachment(arquivo);
                    email.Attachments.Add(arquivoAnexado);
                }

                SmtpClient smtpEmail = new SmtpClient(servidorSMTP, Convert.ToInt32(portaServidorSMTP));
                smtpEmail.Credentials = new NetworkCredential(usuarioSMTP, senhaSMTP);
                smtpEmail.Send(email);

                if (listaEmailsInvalidos.Count > 0)
                {
                    throw new EmailInvalidoException(LevantarExcecaoEmailsInvalidos(listaEmailsInvalidos));
                }
            }
            catch (EmailInvalidoException emailInvalido)
            {
                throw new Exception("O E-mail foi enviado, mas ocorreu o seguinte erro: " + emailInvalido.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("O E-mail não pôde ser enviado, motivo: " + ex.Message);
            }
        }*/

        /// <summary>
        /// Método que envia um e-mail de um arquivo XML (físico ou em memória)
        /// </summary>
        /// <param name="emailBody"></param>
        /// <param name="parametros">Valores que devem ser substituídos <Chave, Valor></param>
        /// <example>
        ///     KeyValuePair<string, string> keyValue = new KeyValuePair<string, string>("@NOME", "RAFAEL");
        ///     parametros.Add(keyValue);
        /// </example>
        /*
        public static void EnviarEmail(XmlDocument emailBody, IDictionary<string, string> parametros,
            params List<string>[] arquivosAnexos)
        {
            try
            {
                string fromXml = string.Empty;
                List<string> listaEmailsInvalidos = new List<string>();
                string[] copias;

                if (!MiscellaneousHelper.ValidarStringsVazias(from, usuarioSMTP, senhaSMTP, servidorSMTP, portaServidorSMTP))
                    throw new Exception("Configuração de E-mail incorreta ou não informada.");

                MailMessage email = new MailMessage();
                email.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                email.IsBodyHtml = true;

                foreach (XmlNode no in emailBody.SelectNodes("email")[0].ChildNodes)
                {
                    if (no.Name.ToLower().Equals("to"))
                    {
                        if (no.InnerText.Equals(""))
                            throw new Exception("E-mail de destino não informado.");

                        string to = no.InnerText;
                        if ((to != null) && (!to.Equals("")))
                        {
                            to = to.Replace("/", ";");
                            to = to.Replace("..", ".");

                            copias = to.Split(';');
                            foreach (string e in copias)
                            {
                                if (!e.Trim().Equals(""))
                                {
                                    if (ValidarFormatoEmail(e.Trim()))
                                    {
                                        email.To.Add(e.Trim());
                                    }
                                    else
                                    {
                                        listaEmailsInvalidos.Add(e.Trim());
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (no.Name.ToLower().Equals("subject"))
                        {
                            email.Subject = no.InnerText;
                        }
                        else
                        {
                            if (no.Name.ToLower().Equals("body"))
                            {

                                string body = no.InnerText;

                                if ((parametros != null) && (parametros.Count > 0))
                                {
                                    foreach (KeyValuePair<string, string> keyValue in parametros)
                                    {
                                        body = body.Replace(keyValue.Key, keyValue.Value);
                                    }
                                }

                                email.Body = body;
                            }
                            else
                            {
                                if (no.Name.ToLower().Equals("ishtml"))
                                {
                                    try
                                    {
                                        email.IsBodyHtml = Convert.ToBoolean(no.InnerText);
                                    }
                                    catch
                                    {
                                        email.IsBodyHtml = true;
                                    }
                                }
                                else
                                {
                                    if (no.Name.ToLower().Equals("cc"))
                                    {
                                        string cc = no.InnerText;
                                        if ((cc != null) && (!cc.Equals("")))
                                        {
                                            cc = cc.Replace("/", ";");
                                            cc = cc.Replace("..", ".");

                                            copias = cc.Split(';');
                                            foreach (string e in copias)
                                            {
                                                if (!e.Trim().Equals(""))
                                                {
                                                    if (ValidarFormatoEmail(e.Trim()))
                                                    {
                                                        email.CC.Add(e.Trim());
                                                    }
                                                    else
                                                    {
                                                        listaEmailsInvalidos.Add(e.Trim());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (no.Name.ToLower().Equals("cco"))
                                        {
                                            string cco = no.InnerText;
                                            if ((cco != null) && (!cco.Equals("")))
                                            {
                                                cco = cco.Replace("/", ";");
                                                cco = cco.Replace("..", ".");

                                                copias = cco.Split(';');
                                                foreach (string e in copias)
                                                {
                                                    if (!e.Trim().Equals(""))
                                                    {
                                                        if (ValidarFormatoEmail(e.Trim()))
                                                        {
                                                            email.Bcc.Add(e.Trim());
                                                        }
                                                        else
                                                        {
                                                            listaEmailsInvalidos.Add(e.Trim());
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (no.Name.ToLower().Equals("from"))
                                            {
                                                if (!no.InnerText.Equals(""))
                                                    fromXml = no.InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                try
                {
                    if (!fromXml.Equals(""))
                    {
                        email.From = new MailAddress(fromXml);
                    }
                    else
                    {
                        email.From = new MailAddress(from);
                    }
                }
                catch
                {
                    email.From = new MailAddress(from);
                }

                if (email.To.Count == 0)
                    throw new Exception("E-mail sem endereço de destino.");

                foreach (string arquivo in arquivosAnexos[0])
                {
                    Attachment arquivoAnexado = new Attachment(arquivo);
                    email.Attachments.Add(arquivoAnexado);
                }

                SmtpClient smtpEmail = new SmtpClient(servidorSMTP, Convert.ToInt32(portaServidorSMTP));
                smtpEmail.Credentials = new NetworkCredential(usuarioSMTP, senhaSMTP);
                smtpEmail.Send(email);

                if (listaEmailsInvalidos.Count > 0)
                {
                    throw new EmailInvalidoException(LevantarExcecaoEmailsInvalidos(listaEmailsInvalidos));
                }
            }
            catch (EmailInvalidoException emailInvalido)
            {
                throw new Exception("O E-mail foi enviado, mas ocorreu o seguinte erro: " + emailInvalido.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("O E-mail não pôde ser enviado, motivo: " + ex.Message);
            }
        }
        */
        

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
