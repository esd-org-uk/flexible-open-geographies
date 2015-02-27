using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Esd.FlexibleOpenGeographies
{
    public class KmlReader : IKmlReader
    {
        public string KmlStringForCode(string code)
        {
            var relativeUri = string.Format("areas/{0}/shape", code);
            var kmlString = Download(relativeUri);
            //If no shape file available we get the document but with no placemark
            //Placemark may or may not have a namespace attribute so don't check for tag close
            return kmlString.Contains("<Placemark") ? kmlString : null;
        }

        public string KmlStringForUri(string uri)
        {
            //TODO: Check that the returned string is actually KML? Custom exception if not so client can deal
            return new WebClient().DownloadString(uri);
        }

        private static string Download(string relativeUri)
        {
            var uri = string.Format("{0}{1}", ConfigurationManager.AppSettings["WebServicesRootUrl"], relativeUri);
            var key = ConfigurationManager.AppSettings["PpkConsumerKey"];
            var password = ConfigurationManager.AppSettings["PpkPassword"];
            var signedUri = GetSignedUrl(uri, key, password);
            return new WebClient().DownloadString(signedUri);
        }

        private static string GetSignedUrl(string uri, string key, string password)
        {
            uri = HttpUtility.UrlDecode(uri);
            if (uri == null) throw new NullReferenceException("Cannot sign null URL");
            var queryCharacter = uri.Contains("?") ? "&" : "?";
            uri = string.Format("{0}{1}ApplicationKey={2}", uri, queryCharacter, key);
            var signature = CalculateSignature(password, uri);

            return string.Format("{0}&Signature={1}", uri, signature);
        }

        private static string CalculateSignature(string password, string uri)
        {
            var authenticationCode = new HMACSHA1(Encoding.UTF8.GetBytes(password));
            var byteArray = Encoding.UTF8.GetBytes(uri);
            using (var stream = new MemoryStream(byteArray))
            {
                var hashValue = authenticationCode.ComputeHash(stream);
                return Convert.ToBase64String(hashValue);
            }
        }
    }
}
