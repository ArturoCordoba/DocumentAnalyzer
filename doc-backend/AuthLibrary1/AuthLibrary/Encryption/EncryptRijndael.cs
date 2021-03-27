using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rijndael256;

namespace AuthLibrary.Encryption
{
    static class EncryptRijndael
    {
        /// <summary>
        /// Method to encrypt a string with the algorithm AES256
        /// </summary>
        /// <param name="plaintext">Text to encrypt</param>
        /// <param name="password">Encryption password</param>
        /// <returns></returns>
        public static string EncryptAes256(string plaintext, string password)
        {
            return RijndaelEtM.Encrypt(plaintext, password, KeySize.Aes256);
        }

        /// <summary>
        /// Method to decrypt a string encoded with the algorithm AES256
        /// </summary>
        /// <param name="encryptedText">Text to decrypt</param>
        /// <param name="password">Encryption password</param>
        /// <returns></returns>
        public static string DecryptAes256(string encryptedText, string password)
        {
            return RijndaelEtM.Decrypt(encryptedText, password, KeySize.Aes256);
        }
    }
}
