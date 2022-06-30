using System;
using System.Security.Cryptography;
using System.Text;

namespace Touba.WebApis.Helpers.SecurityHelper
{
    public class EncryptionDecryptionHelper
    {

        private readonly string _key;

        public EncryptionDecryptionHelper(string key)
        {
            _key = key;
        }

        public string EncryptString(string text)
        {
            try
            {
                return Encrypt(text, _key);
            }
            catch (Exception ex)
            {
                throw new EncryptDecryptException("An error has been occurred when encrypting the value.", ex);
            }
        }

        public string DecryptString(string cipherText)
        {
            try
            {
                return Decrypt(cipherText, _key);
            }
            catch (Exception ex)
            {
                throw new EncryptDecryptException("An error has been occurred when decrypting the value.", ex);
            }
        }

        private string Encrypt(string textToEncrypt, string key)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,

                KeySize = 128,
                BlockSize = 128
            };
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = pwdBytes;
            rijndaelCipher.IV = keyBytes;
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
            rijndaelCipher.Dispose();
            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }

        private string Decrypt(string textToDecrypt, string key)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,

                KeySize = 128,
                BlockSize = 128
            };
            byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = pwdBytes;
            rijndaelCipher.IV = keyBytes;
            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            rijndaelCipher.Dispose();
            return Encoding.UTF8.GetString(plainText);
        }
    }
}
