using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BookMe.Auth.Cryptography.Abstract;

namespace BookMe.Auth.Cryptography.Concrete
{
    public class AESCryptographyService : ISymmetricCryptographyService
    {
        private readonly ISimetricCryptographyKeyProvider keyProvider;

        private readonly byte[] salt = new byte[]
        {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76};

        private const int keyBytes = 32;
        private const int IVBytes = 16;

        public AESCryptographyService(ISimetricCryptographyKeyProvider keyProvider)
        {
            this.keyProvider = keyProvider;
        }

        public string Encrypt(string clearText)
        {
            if (string.IsNullOrEmpty(clearText))
            {
                throw new ArgumentNullException(nameof(clearText));
            }

            var EncryptionKey = keyProvider.Key;
            var cryptogram = string.Empty;
            var clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(EncryptionKey, salt);
                encryptor.Key = pdb.GetBytes(keyBytes);
                encryptor.IV = pdb.GetBytes(IVBytes);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    cryptogram = Convert.ToBase64String(ms.ToArray());
                }
            }
            return cryptogram;
        }

        public string Decrypt(string cryptogram)
        {
            if (string.IsNullOrEmpty(cryptogram))
            {
                throw new ArgumentNullException(nameof(cryptogram));
            }

            var EncryptionKey = keyProvider.Key;
            var clearText = string.Empty;
            var cipherBytes = Convert.FromBase64String(cryptogram);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(EncryptionKey, salt);
                encryptor.Key = pdb.GetBytes(keyBytes);
                encryptor.IV = pdb.GetBytes(IVBytes);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    clearText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return clearText;
        }
    }
}
