using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SPList.Graph
{
    public class AdalResponse
    {
        public string ResponseContent { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool Successful { get; set; }
        public string Message { get; set; }
    }
}