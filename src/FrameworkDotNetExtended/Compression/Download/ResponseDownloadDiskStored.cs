namespace FrameworkDotNetExtended.Compression.Download
{
    using System;
    using System.IO;

    public class ResponseDownloadDiskStored : ResponseDownload
    {
        public string FullPhysicalPath { get; set; }

        /// <summary>
        /// Método utilizado para obter um ResponseDownload passando apenas o conteúdo e o nome. O método se responsabilizará por 
        /// gravar fisicamente e retornar a url física, garantindo não ocorrer problemas de arquivos com mesmo nome solicitado por usuários diferentes.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public override void GenerateReponseDownload(byte[] content, string filename)
        {
            string pastaArquivosTemporarios = ""; // TODO Obter pasta de arquivos temporários

            string identificacaoArquivoFinal = MontarIdentificacaoUnica("Download");
            string caminhoCompletoDestinoArquivosTemp = MontarCaminhoDestino(Path.Combine(pastaArquivosTemporarios, identificacaoArquivoFinal), true);

            File.WriteAllBytes(Path.Combine(caminhoCompletoDestinoArquivosTemp, filename), content);
            this.FullPhysicalPath = Path.Combine(caminhoCompletoDestinoArquivosTemp, filename);
            this.Filename = filename;
        }

        public static String MontarCaminhoDestino(string pathDestino, bool criarPastaCasoNaoExista)
        {
            string pastaArquivosTemporarios = ""; // TODO Obter pasta de arquivos temporários
            string pathDestinoTratado = String.IsNullOrEmpty(pathDestino) ? pastaArquivosTemporarios : pathDestino;

            if (criarPastaCasoNaoExista)
            {
                var pastaDestino = new DirectoryInfo(pathDestinoTratado);
                if (!pastaDestino.Exists) pastaDestino.Create();
            }

            return pathDestinoTratado + (pathDestinoTratado.EndsWith("\\") ? "" : "\\");
        }
    }
}
