using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rijndael256;

namespace AuthLibrary
{
    static class EncryptRijndael
    {
        private static string password = "sFEDrsf?SF-4rfe54s";
        public static string EncryptAes256(string plaintext)
        {
            return RijndaelEtM.Encrypt(plaintext, password, KeySize.Aes256);
        }

        public static string DecryptAes256(string encryptedText)
        {
            return RijndaelEtM.Decrypt(encryptedText, password, KeySize.Aes256);
        }
    }
}
