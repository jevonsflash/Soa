namespace Soa.Protocols.Exception
{
    public class TransportException : System.Exception
    {
        public TransportException(string msg, System.Exception innerException) : base(msg, innerException)
        {
        }

    }
}
