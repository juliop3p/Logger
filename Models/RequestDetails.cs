using System;
using System.Collections.Generic;

namespace Log.Models
{
    public class RequestDetails
    {
        public string Method { get; set; }
        public string Url { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public object Body { get; set; }
        public string Timestamp { get; set; }
    }
}