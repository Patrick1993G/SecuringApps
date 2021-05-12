using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Cryptography_SWD62B
{
    public static class CryptographicHelpers
    {
        private const string _pass = "Pa$$w0rd?MY_";
        private static readonly byte[] _salt;
        private static readonly Tuple<byte[], byte[]> _keyIVPair;

        static CryptographicHelpers()
        {
            _salt = GenerateSalt();
            _keyIVPair = GenerateKeys();
        }

        /// <summary>
        /// You need to store in your database both the digest and the salt!
        /// </summary>
        /// <param name="message"></param>
        /// <param name="salt">Optional, add salt to your message</param>
        /// <returns></returns>
        public static byte[] Hash(byte[] message, byte[] salt = null)
        {
            byte[] messageWithSalt;

            if (salt == null)
            {
                messageWithSalt = message;
            } else // a salt exists, we want to append it to our message!
            {
                List<byte> temp = new List<byte>(message.Length + salt.Length);
                temp.AddRange(message);
                temp.AddRange(salt);

                messageWithSalt = temp.ToArray();
            }

            SHA512 sha = SHA512.Create();
            byte[] digest = sha.ComputeHash(messageWithSalt);

            return digest;
        }

        public static string Hash(string message, byte[] salt = null)
        {
            byte[] encodedMessage = Encoding.UTF32.GetBytes(message);

            byte[] digest = Hash(encodedMessage, salt);

            return Convert.ToBase64String(digest);
        }

        public static byte[] GenerateSalt(int size = 64)
        {
            byte[] salt = new byte[size]; // default = 64 * 8 = 512 bit
            RandomNumberGenerator rng = RandomNumberGenerator.Create();

            rng.GetBytes(salt);

            return salt;
        }

        public static byte[] SymmetricEncrypt(byte[] plainTextMessage, Tuple<byte[], byte[]> _keyIVPair)
        {
            Aes aes = Aes.Create();
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            // GenerateKeys() in the static contructor

            // apply symmetric encryption to:
            // 1. query string parameters
            // 2. as part of hybrid encryption

            // plainTextMethod and we are going to create a stream from this data
            using (MemoryStream msIn = new MemoryStream(plainTextMessage))
            {
                // Be very careful to position the memory stream at position 0!
                msIn.Position = 0;
                // msIn.Seek(0, SeekOrigin.Begin);

                // create a stream for the output of the cryptographic algorithm
                using (MemoryStream msOut = new MemoryStream())
                {

                    // create a cryptostream <- will take as inputs, the aes algorithm, the output stream
                    // we will pass the values from the input stream to the cryptostream and obtain the encrypted output
                    using (CryptoStream cs = new CryptoStream(
                        msOut,
                        aes.CreateEncryptor(_keyIVPair.Item1, _keyIVPair.Item2),
                        CryptoStreamMode.Write))
                    {
                        msIn.CopyTo(cs);
                        cs.FlushFinalBlock();
                    }

                    return msOut.ToArray();
                }
            }
        }

        public static byte[] SymmetricDecrypt(byte[] encryptedMessage, Tuple<byte[], byte[]> _keyIVPair)
        {
            Aes aes = Aes.Create();
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            using (MemoryStream msIn = new MemoryStream(encryptedMessage))
            {
                msIn.Position = 0;

                // create a stream for the output of the cryptographic algorithm
                using (MemoryStream msOut = new MemoryStream())
                {

                    // create a cryptostream <- will take as inputs, the aes algorithm, the output stream
                    // we will pass the values from the input stream to the cryptostream and obtain the encrypted output
                    using (CryptoStream cs = new CryptoStream(
                                msOut,
                                aes.CreateDecryptor(_keyIVPair.Item1, _keyIVPair.Item2),
                                CryptoStreamMode.Write))
                    {
                        msIn.CopyTo(cs);
                        cs.FlushFinalBlock();
                    }

                    return msOut.ToArray();
                }
            }
        }
        
        public static Tuple<byte[], byte[]> GenerateKeys()
        {
            Aes aes = Aes.Create();

            // Input: password and a salt
            // Output: key and an IV
            Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(_pass, _salt, 100);

            // e.g. aes.KeySize = 256
            // a bit is 8 bits long
            // so we need 256/8 byes
            byte[] key = keyGenerator.GetBytes(aes.KeySize / 8);
            byte[] iv = keyGenerator.GetBytes(aes.BlockSize / 8);

            return new Tuple<byte[], byte[]>(key, iv);
        }

        public static Tuple<string, string> GenerateAsymmetricKeys()
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();

            string publicKey = provider.ToXmlString(false);
            string privateKey = provider.ToXmlString(true);

            return new Tuple<string, string>(publicKey, privateKey);
        }

        public static byte[] AsymetricEncrypt(byte[] data, string publicKey)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(publicKey);

            byte[] cipher = provider.Encrypt(data, RSAEncryptionPadding.Pkcs1);
            return cipher;
        }

        public static byte[] AsymmetricDecrypt(byte[] data, string privateKey)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(privateKey);
            byte[] cipher = provider.Decrypt(data, RSAEncryptionPadding.Pkcs1);
            return cipher;
        }

        public static string CreateSigniture(byte[] objectArr, RSA rsa, string privateKey)
        {
            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(objectArr);
            }

            rsa.FromXmlString(privateKey);
            RSAPKCS1SignatureFormatter signitureFormatter = new RSAPKCS1SignatureFormatter(rsa);
            signitureFormatter.SetHashAlgorithm("SHA256");
            byte[] signedHashValue = signitureFormatter.CreateSignature(hash);
            return Convert.ToBase64String(signedHashValue);
        }

        public static bool VerifySigniture(byte[] fileLocalBytes, string signiture, RSA rsa, string publicKey)
        {

            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(fileLocalBytes);
            }
            rsa.FromXmlString(publicKey);
            byte[] signature = Convert.FromBase64String(signiture);
            RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            rsaDeformatter.SetHashAlgorithm("SHA256");
            bool isValid = rsaDeformatter.VerifySignature(hash, signature);
            return isValid;
        }
    }
}
