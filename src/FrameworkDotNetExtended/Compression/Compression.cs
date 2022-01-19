using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using FrameworkDotNetExtended.Compression.Download;
using FrameworkDotNetExtended.Compression.Files;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace FrameworkDotNetExtended.Compression
{

    public static class Compression
    {

        #region Compactação de arquivos em memória

        public static CompressedFileMemory CompactarArquivosEmMemoria(Func<IList<string>, IList<CompressableFile>> funcaoObtencaoArquivos, IList<string> idsArquivos, string identificacaoArquivo)
        {
            return CompactarArquivosEmMemoria(funcaoObtencaoArquivos.Invoke(idsArquivos), identificacaoArquivo);
        }

        public static CompressedFileMemory CompactarArquivosEmMemoria(IList<CompressableFile> arquivos, string identificacaoArquivo)
        {
            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);
            zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
            foreach (var arquivo in arquivos)
            {
                ZipEntry newEntry = new ZipEntry(arquivo.Filename);
                newEntry.DateTime = DateTime.Now;
                zipStream.PutNextEntry(newEntry);
                StreamUtils.Copy(new MemoryStream(arquivo.Content), zipStream, new byte[4096]);
                zipStream.CloseEntry();
            }
            zipStream.IsStreamOwner = false; // False stops the Close also Closing the underlying stream.
            zipStream.Close(); // Must finish the ZipOutputStream before using outputMemStream.
            outputMemStream.Position = 0;

            // Preparando o retorno do método
            CompressedFileMemory compressedFileMemory = new CompressedFileMemory()
            {
                Filename = ResponseDownload.MontarIdentificacaoUnica(identificacaoArquivo) + ".zip",
                Content = outputMemStream.ToArray()
            };

            return compressedFileMemory;
        }

        #endregion

        #region Compactação através de arquivos armazenados em disco

        /// <summary>
        /// Método responsável pela compactação de arquivos em um único arquivo (A pasta utilizada como destino é a mencionada no arquivo de configurações)
        /// </summary>
        /// <param name="arquivos"></param>
        /// <param name="identificacaoArquivo">Identificação amigável do conjunto de arquivos para ser usado como nome do arquivo final. (Não precisa informar a extensão .zip e nem um número único)</param>
        /// <returns></returns>
        public static CompressedFileDiskStored CompactarArquivos(IList<CompressableFile> arquivos, string identificacaoArquivo)
        {
            string pastaArquivosTemporarios = ""; // TODO Obter pastar temporária
            return CompactarArquivos(arquivos, identificacaoArquivo, pastaArquivosTemporarios);
        }

        public static CompressedFileDiskStored CompactarArquivos(Func<IList<string>, IList<CompressableFile>> funcaoObtencaoArquivos, IList<string> idsArquivos, string identificacaoArquivo)
        {
            return CompactarArquivos(funcaoObtencaoArquivos.Invoke(idsArquivos), identificacaoArquivo);
        }

        /// <summary>
        /// Método responsável pela compactação de arquivos em um único arquivo.
        /// </summary>
        /// <param name="arquivos"></param>
        /// <param name="pathDestino"></param>
        /// <param name="identificacaoArquivo">Identificação amigável do conjunto de arquivos para ser usado como nome do arquivo final. (Não precisa informar a extensão .zip e nem um número único)</param>
        /// <returns></returns>
        public static CompressedFileDiskStored CompactarArquivos(IList<CompressableFile> arquivos, string identificacaoArquivo, string pathDestino)
        {
            string identificacaoArquivoFinal = ResponseDownload.MontarIdentificacaoUnica(identificacaoArquivo);
            string caminhoCompletoDestinoArquivosTemp = ResponseDownloadDiskStored.MontarCaminhoDestino(Path.Combine(pathDestino, identificacaoArquivoFinal), true);

            foreach (var fileModel in arquivos)
            {
                // Tratando existência de arquivos com mesmo nome e que ficarão dentro do mesmo arquivo compactado
                int qtdArquivosComEsteNome = arquivos.Count(elemento => elemento.Filename.ToUpper().Equals(fileModel.Filename.ToUpper()));
                if (qtdArquivosComEsteNome > 1) fileModel.Filename = AdicionarStringFinalNomeArquivo(fileModel.Filename, "_" + qtdArquivosComEsteNome);
                // Gravando fisicamente o arquivo
                File.WriteAllBytes(Path.Combine(caminhoCompletoDestinoArquivosTemp, fileModel.Filename), fileModel.Content);
            }

            // Compactando os arquivos passando o path da pasta
            var zip = new ICSharpCode.SharpZipLib.Zip.FastZip();
            zip.CreateZip(Path.Combine(pathDestino, identificacaoArquivoFinal + ".zip"), caminhoCompletoDestinoArquivosTemp, true, null);
            Directory.Delete(Path.Combine(pathDestino, identificacaoArquivoFinal), true);

            var compressedFile = new CompressedFileDiskStored();
            compressedFile.Filename = identificacaoArquivoFinal + ".zip";
            compressedFile.FullPhysicalPath = Path.Combine(pathDestino, identificacaoArquivoFinal + ".zip");

            return compressedFile;
        }

        #endregion

        #region Junção/empacotamento de arquivos (não ocorre compactação)

        /// <summary>
        /// Método responsável pela junção de arquivos em um único arquivo .ZIP. (não ocorre compactação de dados e a pasta utilizada como destino é a mencionada no arquivo de configurações)
        /// </summary>
        /// <param name="arquivos"></param>
        /// <param name="identificacaoArquivo">Identificação amigável do conjunto de arquivos para ser usado como nome do arquivo final. (Não precisa informar a extensão .zip e nem um número único)</param>
        /// <returns></returns>
        public static CompressedFileDiskStored JuntarArquivos(IList<CompressableFile> arquivos, string identificacaoArquivo)
        {
            string pastaArquivosTemporarios = ""; // TODO Obter pastar temporária
            return JuntarArquivos(arquivos, identificacaoArquivo, pastaArquivosTemporarios);
        }

        /// <summary>
        /// Método responsável pela junção de arquivos em um único arquivo .ZIP. (não ocorre compactação de dados)
        /// </summary>
        /// <param name="arquivos"></param>
        /// <param name="pathDestino">Identificação amigável do conjunto de arquivos para ser usado como nome do arquivo final. (Não precisa informar a extensão .zip e nem um número único)</param>
        /// <param name="identificacaoArquivo">Identificação amigável do conjunto de arquivos para ser usado como nome do arquivo final. (Não precisa informar a extensão .zip e nem um número único)</param>
        /// <returns></returns>
        public static CompressedFileDiskStored JuntarArquivos(IList<CompressableFile> arquivos, string identificacaoArquivo, string pathDestino)
        {
            string identificacaoArquivoFinal = ResponseDownload.MontarIdentificacaoUnica(identificacaoArquivo);
            string caminhoCompletoDestino = ResponseDownloadDiskStored.MontarCaminhoDestino(Path.Combine(pathDestino, identificacaoArquivoFinal), true);

            foreach (var fileModel in arquivos)
            {
                // Tratando existência de arquivos com mesmo nome e que ficarão dentro do mesmo arquivo empacotado
                int qtdArquivosComEsteNome = arquivos.Count(elemento => elemento.Filename.ToUpper().Equals(fileModel.Filename.ToUpper()));
                if (qtdArquivosComEsteNome > 1) fileModel.Filename = AdicionarStringFinalNomeArquivo(fileModel.Filename, "_" + qtdArquivosComEsteNome);
                // Gravando fisicamento o arquivo
                File.WriteAllBytes(Path.Combine(caminhoCompletoDestino, fileModel.Filename), fileModel.Content);
                // Adicionando o arquivo recém gravado fisicamente ao pacote de arquivos
                AddFileToZip(Path.Combine(pathDestino, identificacaoArquivoFinal + ".zip"), Path.Combine(caminhoCompletoDestino, fileModel.Filename));
            }
            Directory.Delete(Path.Combine(pathDestino, identificacaoArquivoFinal), true);

            var compressedFile = new CompressedFileDiskStored();
            compressedFile.Filename = identificacaoArquivoFinal + ".zip";
            compressedFile.FullPhysicalPath = Path.Combine(pathDestino, identificacaoArquivoFinal + ".zip");

            return compressedFile;
        }

        #endregion

        #region Métodos auxiliares

        public static string AdicionarStringFinalNomeArquivo(string nomeArquivoComExtensao, string palavraParaAdicionar)
        {
            if (nomeArquivoComExtensao.Contains("."))
            {
                string[] nomeRepartido = nomeArquivoComExtensao.Split('.');
                nomeRepartido[nomeRepartido.Length - 2] = nomeRepartido[nomeRepartido.Length - 2] + palavraParaAdicionar;
                return string.Join(".", nomeRepartido);
            }
            return nomeArquivoComExtensao + palavraParaAdicionar;
        }

        #endregion

        #region Métodos responsáveis pelo empacotamento de arquivos

        private const long BUFFER_SIZE = 4096;

        private static void AddFileToZip(string zipFilename, string fileToAdd)
        {
            using (Package zip = Package.Open(zipFilename, FileMode.OpenOrCreate))
            {
                string destFilename = ".\\" + Path.GetFileName(fileToAdd);
                Uri uri = PackUriHelper.CreatePartUri(new Uri(destFilename, UriKind.Relative));
                if (zip.PartExists(uri))
                {
                    zip.DeletePart(uri);
                }
                PackagePart part = zip.CreatePart(uri, "", CompressionOption.Normal);
                using (FileStream fileStream = new FileStream(fileToAdd, FileMode.Open, FileAccess.Read))
                {
                    using (Stream dest = part.GetStream())
                    {
                        CopyStream(fileStream, dest);
                    }
                }
            }
        }

        private static void CopyStream(FileStream inputStream, Stream outputStream)
        {
            long bufferSize = inputStream.Length < BUFFER_SIZE ? inputStream.Length : BUFFER_SIZE;
            byte[] buffer = new byte[bufferSize];
            int bytesRead = 0;
            long bytesWritten = 0;
            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                outputStream.Write(buffer, 0, bytesRead);
                bytesWritten += bufferSize;
            }
        }

        #endregion

    }
}
