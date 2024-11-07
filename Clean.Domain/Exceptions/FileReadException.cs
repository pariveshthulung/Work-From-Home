namespace Clean.Domain.Exceptions
{
    public class FileReadException : Exception
    {
        public FileReadException(string message)
            : base(message) { }

        public FileReadException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
