using System;
using System.Collections.Generic;

namespace Log.Models
{
    public class LogEntry
    {
        public string LogTitle { get; set; }
        public string LogLevel { get; set; } = "INFO"; 
        public String InitialTimestamp { get; set; }
        public String EndTimestamp { get; set; }
        public int Duration { get; set; }
        public RequestDetails RequestDetails { get; set; }
        public List<Steps> Steps { get; set; } = new List<Steps>();
        public List<ExternalApiCall> ExternalApiCalls { get; set; } = new List<ExternalApiCall>();
        public ResponseDetails ResponseDetails { get; set; }
    }
}