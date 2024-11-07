namespace Clean.Domain.Exceptions
{
    public class SubmitException : Exception
    {
        public SubmitException(string message, Exception innerException)
            : base(message, innerException) { }

        public SubmitException(string message)
            : base(message) { }
    }
}
