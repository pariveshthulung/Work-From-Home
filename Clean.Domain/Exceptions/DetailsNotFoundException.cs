namespace Clean.Domain.Exceptions
{
    public class DetailsNotFoundException : Exception
    {
        public DetailsNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        public DetailsNotFoundException(string message)
            : base(message) { }
    }
}
