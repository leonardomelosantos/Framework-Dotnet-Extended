using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkDotNetExtended.Helpers
{
    public class FileHelper
    {

        #region Geração de hash SHA1 e MD5


        public static string GetSHA1Hash(byte[] conteudo)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
                return GetHash(conteudo, sha1);
        }

        public static string GetMD5Hash(byte[] conteudo)
        {
            using (var md5 = new MD5CryptoServiceProvider())
                return GetHash(conteudo, md5);
        }

        private static string GetHash(byte[] s, HashAlgorithm hasher)
        {
            var hashGenerated = hasher.ComputeHash(s);
            return BitConverter.ToString(hashGenerated).Replace("-", string.Empty);
        }

        #endregion

    }
}
