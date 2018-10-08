using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ThorusViewer
{
    public class WebClientEx : WebClient
    {
        /// <summary>
        /// Time in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        public WebClientEx() : this(60000) { }

        public WebClientEx(int timeout)
        {
            this.Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = this.Timeout;
            }
            return request;
        }
    }

}
