using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace lab03_nhom
{
    class RSAAlgorithm
    {
        

        public static string GetKeyString(RSAParameters publicKey)
        {

            var stringWriter = new System.IO.StringWriter();
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            xmlSerializer.Serialize(stringWriter, publicKey);
            return stringWriter.ToString();
        }

        public static string Encrypt(string textToEncrypt, string publicKeyString)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    rsa.FromXmlString(publicKeyString.ToString());
                    var encryptedData = rsa.Encrypt(bytesToEncrypt, true);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    return base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        public static string Decrypt(string textToDecrypt, string privateKeyString)
        {
            var bytesToDescrypt = Encoding.UTF8.GetBytes(textToDecrypt);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {

                    // server decrypting data with private key                    
                    rsa.FromXmlString(privateKeyString);

                    var resultBytes = Convert.FromBase64String(textToDecrypt);
                    var decryptedBytes = rsa.Decrypt(resultBytes, true);
                    var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                    return decryptedData.ToString();
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        private static string GenerateTestString()
        {
            Guid opportinityId = Guid.NewGuid();
            Guid systemUserId = Guid.NewGuid();
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("opportunityid={0}", opportinityId.ToString());
            sb.AppendFormat("&systemuserid={0}", systemUserId.ToString());
            sb.AppendFormat("&currenttime={0}", currentTime);

            return sb.ToString();
        }

        public static string HexToBase64(string hexstring)
        {
            try
            {
                var bytes = new byte[hexstring.Length / 2];
                for (var i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(hexstring.Substring(i * 2, 2), 16);
                }
                return Convert.ToBase64String(bytes);
            }
            catch (Exception)
            {
                return "-1";
            }
        }

        public static string Base64ToHex(string base64string)
        {
            try
            {
                var bytes = Convert.FromBase64String(base64string);
                var hex = BitConverter.ToString(bytes);
                return hex.Replace("-", "").ToLower();
            }
            catch (Exception)
            {
                return "-1";
            }
        }
    }
}
