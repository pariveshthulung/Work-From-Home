namespace Clean.Domain.Exceptions
{
    public class FileNotFoundException : Exception
    {
        public FileNotFoundException(string message)
            : base(message) { }

        public FileNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
