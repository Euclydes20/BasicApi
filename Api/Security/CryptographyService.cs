using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Api.Security
{
    public static class CryptographyService
    {
        private const string SECURITYKEY = "AD737B8227DA48B5C3C7CC2C090C85A2";

        public static string EncryptString(string objText)
        {
            byte[] objInitVectorBytes = Encoding.UTF8.GetBytes("HR$2pIjHR$2pIj12");
            byte[] objPlainTextBytes = Encoding.UTF8.GetBytes(objText);
            PasswordDeriveBytes objPassword = new(SECURITYKEY, null);
            byte[] objKeyBytes = objPassword.GetBytes(256 / 8);
            //RijndaelManaged objSymmetricKey = new RijndaelManaged();
            Aes objSymmetricKey = Aes.Create();
            objSymmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform objEncryptor = objSymmetricKey.CreateEncryptor(objKeyBytes, objInitVectorBytes);
            MemoryStream objMemoryStream = new();
            CryptoStream objCryptoStream = new(objMemoryStream, objEncryptor, CryptoStreamMode.Write);
            objCryptoStream.Write(objPlainTextBytes, 0, objPlainTextBytes.Length);
            objCryptoStream.FlushFinalBlock();
            byte[] objEncrypted = objMemoryStream.ToArray();
            objMemoryStream.Close();
            objCryptoStream.Close();
            return Convert.ToBase64String(objEncrypted);
        }

        public static string DecryptString(string encryptedText)
        {
            byte[] objInitVectorBytes = Encoding.UTF8.GetBytes("HR$2pIjHR$2pIj12");
            byte[] objDeEncryptedText = Convert.FromBase64String(encryptedText);
            PasswordDeriveBytes objPassword = new(SECURITYKEY, null);
            byte[] objKeyBytes = objPassword.GetBytes(256 / 8);
            //RijndaelManaged objSymmetricKey = new RijndaelManaged();
            Aes objSymmetricKey = Aes.Create();
            objSymmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform objDecryptor = objSymmetricKey.CreateDecryptor(objKeyBytes, objInitVectorBytes);
            MemoryStream objMemoryStream = new(objDeEncryptedText);
            CryptoStream objCryptoStream = new(objMemoryStream, objDecryptor, CryptoStreamMode.Read);
            MemoryStream objDecryptedMemoryStream = new();
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = objCryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                objDecryptedMemoryStream.Write(buffer, 0, bytesRead);
            byte[] objPlainTextBytes = objDecryptedMemoryStream.ToArray();
            objMemoryStream.Close();
            objCryptoStream.Close();
            objDecryptedMemoryStream.Close();
            return Encoding.UTF8.GetString(objPlainTextBytes);
        }

        public static string Base64Encode(string objText)
        {
            return Base64UrlEncoder.Encode(objText);
        }

        public static string Base64Decode(string encryptedText)
        {
            return Base64UrlEncoder.Decode(encryptedText);
        }

        public static string Hash(this string value)
        {
            value = string.IsNullOrEmpty(value) ? string.Empty : value;
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(value));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                // can be "x2" if you want lowercase
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
