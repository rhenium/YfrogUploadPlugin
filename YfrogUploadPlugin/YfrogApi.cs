using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Dulcet.Network;
using Dulcet.Twitter.Credential;
using Dulcet.Util;
using System.Xml;
using System.Runtime.Serialization.Json;

namespace YfrogUploader
{
    /// <summary>
    /// Yfrog API Implementation
    /// </summary>
    public static class YfrogApi
    {
        static string UploadApiUrl = "https://yfrog.com/api/xauth_upload";
        
        /// <summary>
        /// Upload picture to Yfrog
        /// </summary>
        public static bool UploadToYfrog(this OAuth provider, string apiKey, string mediaFilePath, out string url)
        {
            url = null;
            var doc = provider.UploadToYfrog(apiKey, mediaFilePath);
            if (doc == null || doc.Element("rsp").Attribute("stat").Value != "ok")
            {
                return false;
            }
            else
            {
                url = doc.Element("rsp").Element("mediaurl").ParseString();
                return true;
            }
        }

        /// <summary>
        /// Upload picture to Yfrog
        /// </summary>
        public static XDocument UploadToYfrog(this OAuth provider, string apiKey, string mediaFilePath)
        {
            var req = Http.CreateRequest(new Uri(UploadApiUrl), "POST", contentType: "application/x-www-form-urlencoded");

            // use OAuth Echo
            provider.MakeOAuthEchoRequest(ref req, null, "https://api.twitter.com/1/account/verify_credentials.xml");
            // verify_credentials.json だとレスポンスまでJSONになっちゃう？
            //req.Headers["X-Auth-Service-Provider"] = "https://api.twitter.com/1/account/verify_credentials.xml";

            List<SendData> sd = new List<SendData>();
            sd.Add(new SendData("key", apiKey));
            sd.Add(new SendData("media", file: mediaFilePath));

            /*var doc = Http.WebUpload<XDocument>(req, sd, Encoding.UTF8, (s) =>
            {
                return XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(s, XmlDictionaryReaderQuotas.Max));
            });*/
            var doc = Http.WebUpload<XDocument>(req, sd, Encoding.UTF8, (s) => XDocument.Load(s));
            if (doc.ThrownException != null)
                throw doc.ThrownException;
            if (doc.Succeeded == false)
                return null;
            return doc.Data;
        }
    }
}
