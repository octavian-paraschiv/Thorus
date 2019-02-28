using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;
using System.IO;

namespace ThorusViewer
{
    public sealed class EmailClient : IDisposable
    {
        string _server = "";
        int _port = 110;
        string _user = "";
        string _password = "";
        string _sender = "";
        bool _useSSl = false;

        OpenPop.Pop3.Pop3Client _popClient = null;

        public EmailClient()
        {
            _server = ConfigurationManager.AppSettings["emailServer"];
            _user = ConfigurationManager.AppSettings["emailAccount"];
            _password = ConfigurationManager.AppSettings["emailPassword"];
            _sender = ConfigurationManager.AppSettings["emailSenderAddress"];
            
            var sslSetting = ConfigurationManager.AppSettings["emailUseSSL"];
            _useSSl = (sslSetting != null && (sslSetting == "1" || sslSetting.ToLowerInvariant() == "true"));

            try
            {
                _port = int.Parse(ConfigurationManager.AppSettings["emailPort"]);
            }
            catch
            {
                _port = _useSSl ? 495 : 110;
            }
        }

        public bool Connect()
        {
            try
            {
                Disconnect();

                _popClient = new OpenPop.Pop3.Pop3Client();
                _popClient.Connect(_server, _port, _useSSl);
                _popClient.Authenticate(_user, _password);

                return true;
            }
            catch(Exception ex)
            {
            }

            return false;
        }

        public string FetchWeatherData(string destFolder)
        {
            StringBuilder sb = new StringBuilder();

            int msgCount = _popClient.GetMessageCount();

            sb.Append($" total: {msgCount} messages for {_user}");

            for (int i = 1; i <= msgCount; i++)
            {
                var header = _popClient.GetMessageHeaders(i);

                sb.Append($" parsing message #{i} ...");

                if (string.Compare(header.From.Address, _sender, true) == 0)
                {
                    var message = _popClient.GetMessage(i);

                    var body = message.FindFirstPlainTextVersion().Body;
                    var encoding = message.FindFirstPlainTextVersion().BodyEncoding;
                    var msgBody = encoding.GetString(body);

                    var msgLines = msgBody
                        .Replace("\r\n", "\n")
                        .Replace("\r", "\n")
                        .Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    string dataType = msgLines[6].Trim(" \t".ToCharArray()).Replace("Parameters: ", "");
                    string ftpUrl = msgLines[14].Trim(" \t".ToCharArray());

                    string file = Path.Combine(destFolder, string.Format("{0}.nc", dataType));
                    if (File.Exists(file) == false)
                    {
                        using (WebClient wc = new WebClient())
                        {
                            sb.Append($" downloading {dataType} data from {ftpUrl} ...");

                            wc.DownloadFile(ftpUrl, file);
                        }
                    }
                }
            }

            return sb.ToString();
        }

        public void Disconnect()
        {
            if (_popClient != null)
            {
                _popClient.Disconnect();
                _popClient = null;
            }
        }

        public void Dispose()
        {
            Disconnect();
        }

        internal void DeleteAllMessages()
        {
            _popClient.DeleteAllMessages();
        }
    }
}
