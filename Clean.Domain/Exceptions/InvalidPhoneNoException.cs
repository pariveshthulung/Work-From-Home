namespace Clean.Domain.Exceptions
{
    public class InvalidPhoneNoException : Exception
    {
        public InvalidPhoneNoException(string msg)
            : base(msg) { }

        public InvalidPhoneNoException(string msg, Exception innerException)
            : base(msg, innerException) { }
    }
}
