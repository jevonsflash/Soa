using System;

namespace Soa.Client.ApiGateway.Core
{
    public class SoaHttpStatusCodeException : Exception
    {
        public int StatusCode { get; set; }
        public string ContentType { get; set; } = @"application/json";
        public string Path { get; set; }
        public SoaHttpStatusCodeException(int statusCode)
        {
            StatusCode = statusCode;
        }
        public SoaHttpStatusCodeException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public SoaHttpStatusCodeException(int statusCode, string message, string path) : base(message)
        {
            StatusCode = statusCode;
            Path = path;
        }
    }
}
