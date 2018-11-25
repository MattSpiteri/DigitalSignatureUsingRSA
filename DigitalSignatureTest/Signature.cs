using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using DigitalSignatureTest;

namespace DigitalSignatureTest
{
    public static class Signature
    {

        /// <summary>
        /// This method is used to sign the data using the private key
        /// </summary>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="privateKeyFilePath"></param>
        /// <returns></returns>
        public static string Sign(string method, string data, string privateKeyFilePath)
        {

            RSACryptoServiceProvider rsaCsp = PemKeyUtils.GetRSAProviderFromPemFile(privateKeyFilePath);

            string plainText = method  + SerializeData(data);
            byte[] dataBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] signatureBytes = rsaCsp.SignData(dataBytes, "SHA1");

            return Convert.ToBase64String(signatureBytes);
        }

        /// <summary>
        /// This method is used to verified the signed data with the public key
        /// </summary>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="publicKeyPath"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static bool Verify(string method, string data, string publicKeyPath, string signature)
        {
            string plainText = method + SerializeData(data);

            byte[] dataBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] signBytes = Convert.FromBase64String(signature);

            RSACryptoServiceProvider rsa = PemKeyUtils.GetRSAProviderFromPemFile(publicKeyPath);

            bool valid = rsa.VerifyData(dataBytes, "SHA1", signBytes);

            return valid;
        }

        public static bool TryParseJson(String data, out JObject result)
        {
            bool parseResult = false;
            JObject json = null;

            try
            {
                json = JObject.Parse(data);
                parseResult = true;
            }
            catch (Exception)
            {
                //no need to log it
            }

            result = json;

            return parseResult;
        }

        public static string SerializeData(string jsonObject)
        {
            var values = new Dictionary<string, string>();
            var serialized = string.Empty;
            var validJson = new JObject();
            bool isValidJson = TryParseJson(jsonObject, out validJson);

            if (isValidJson)
            {
                foreach (var item in validJson)
                {
                    if (item.Key != "attributes") //this is going to be skipped for now as its unused, but we might need to add it in the future
                        values.Add(Convert.ToString(item.Key), Convert.ToString(item.Value));
                }

                if (values.Count > 1)
                {
                    // Acquire keys and sort them.
                    var list = values.Keys.ToList();
                    list.Sort();

                    // Loop through keys.
                    foreach (var key in list)
                    {
                        serialized += key + SerializeData(values[key]);
                    }
                }
                else
                {
                    if (values.Keys.Count != 0)
                    {
                        var key = values.Keys.ToList()[0];
                        serialized += key + SerializeData(values[key]);
                    }
                }
            }
            else
            {
                return jsonObject;
            }

            return serialized;
        }

     }
}
