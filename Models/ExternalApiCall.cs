namespace Log.Models
{
    public class ExternalApiCall
    {
        public string ApiName { get; set; }
        public RequestDetails Request { get; set; }
        public ResponseDetails Response { get; set; }
    }
}