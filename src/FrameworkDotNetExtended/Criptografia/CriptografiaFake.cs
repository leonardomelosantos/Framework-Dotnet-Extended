using System;
using System.Text;

namespace FrameworkDotNetExtended.Criptografia
{
    public class CriptografiaFake
    {
        public static string Decrypt(string encrypt)
        {
            Byte[] b = Convert.FromBase64String(encrypt);
            string decrypted = ASCIIEncoding.ASCII.GetString(b);
            return decrypted;
        }

        public static string Encrypt(string decrypt)
        {
            Byte[] b = ASCIIEncoding.ASCII.GetBytes(decrypt);
            string encrypt = Convert.ToBase64String(b);
            return encrypt;
        }
    }
}
