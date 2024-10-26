using System;
using System.Collections.Generic;

namespace Log.Models
{
    public class ResponseDetails
    {
        public int StatusCode { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public object Body { get; set; }
        public String Timestamp { get; set; }
        public long DurationMs { get; set; }
    }
}