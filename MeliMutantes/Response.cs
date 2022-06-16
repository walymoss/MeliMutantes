using System.Net;

namespace MeliMutantes
{
    public class Response
    {
        public bool response { get; set; }
        public string message { get; set; }
        public HttpStatusCode statusCode { get; set; }

    }
}
