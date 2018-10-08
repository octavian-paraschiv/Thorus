using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;
using System.Windows.Forms;
using System.Linq;
using Majestic12;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using ThorusCommon.Engine;

namespace ThorusViewer
{
    public class ServerRequestor : IDisposable
    {
        string _noaaServerPostUrl;
        string _noaaRefererUrl;
        string _noaaPostContentType;
        string _noaaPostBody;
        string _noaaSstGetUrl;

        public ServerRequestor()
        {
            _noaaServerPostUrl = ConfigurationManager.AppSettings["noaaServerPostUrl"];
            _noaaRefererUrl = ConfigurationManager.AppSettings["noaaRefererUrl"];
            _noaaPostContentType = ConfigurationManager.AppSettings["noaaPostContentType"];
            _noaaPostBody = ConfigurationManager.AppSettings["noaaPostBody"];
            _noaaSstGetUrl = ConfigurationManager.AppSettings["noaaSstGetUrl"];
        }

        public void Dispose()
        {
        }

        public bool RequestNewFile(string fileName)
        {
            if (fileName == "SST.NC")
                return RequestSstFile();

            using (WebClient wc = new WebClient())
            {
            }

            return false;
        }
            
        private bool RequestSstFile()
        {
            try
            {
                DateTime dt = DateTime.Today.AddDays(-10);
                string getUrl = ReplaceMacros(_noaaSstGetUrl, dt);
                string response = "";

                ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2

                using (WebClient wc = new WebClient())
                {
                    response = wc.DownloadString(getUrl);
                }

                HTMLparser parser = new HTMLparser();
                parser.Init(response);

                HTMLchunk chunk = null;

                while ((chunk = parser.ParseNext()) != null)
                {
                    if (chunk.sTag != "a")
                        continue;
                    if (chunk.oType != HTMLchunkType.OpenTag)
                        continue;
                    if (chunk.oParams == null || chunk.oParams.Count < 1)
                        continue;

                    string href = chunk.oParams["href"]?.ToString();
                    if (string.IsNullOrEmpty(href))
                        continue;
                    if (href.StartsWith("ftp://"))
                    {
                        string file = Path.Combine(SimulationData.WorkFolder, "SST.nc");
                        if (File.Exists(file) == false)
                        {
                            using (WebClient wc = new WebClient())
                            {
                                wc.DownloadFile(href, file);
                            }
                        }
                    }


                }
            }
            catch(Exception ex)
            {
                int s = 0;
            }

            return false;
        }

        /// <summary>
        /// Certificate validation callback.
        /// </summary>
        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        private string ReplaceMacros(string str, DateTime dt, string paramName = "", string emailAccount = "")
        {
            return str
                .Replace("##EMAIL_ACCOUNT##", paramName)
                .Replace("##PARAM_NAME##", paramName)
                .Replace("##TODAY##", dt.ToString("yyyy-MM-dd"))
                .Replace("##DAY##", dt.Day.ToString())
                .Replace("##MONTH##", dt.Month.ToString())
                .Replace("##YEAR##", dt.Year.ToString());
        }
    }
}
