using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace QLHOCSINH.Helpers
{
    public class AesEncryptionHelper
    {
        private static readonly string EncryptionKey = "P@ssw0rd!_EnCrYpt#12345$";

        // Mã hóa chuỗi
        public static string Encrypt(string plainText)
        {
            byte[] key = Encoding.UTF8.GetBytes(EncryptionKey);
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    byte[] combinedBytes = new byte[iv.Length + encryptedBytes.Length];
                    Array.Copy(iv, 0, combinedBytes, 0, iv.Length);
                    Array.Copy(encryptedBytes, 0, combinedBytes, iv.Length, encryptedBytes.Length);

                    return Convert.ToBase64String(combinedBytes);
                }
            }
        }

        // Giải mã chuỗi
        public static string Decrypt(string encryptedText)
        {
            byte[] key = Encoding.UTF8.GetBytes(EncryptionKey);
            byte[] combinedBytes = Convert.FromBase64String(encryptedText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] encryptedBytes = new byte[combinedBytes.Length - iv.Length];
                Array.Copy(combinedBytes, 0, iv, 0, iv.Length);
                Array.Copy(combinedBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(plainBytes);
                }
            }
        }
    }
}