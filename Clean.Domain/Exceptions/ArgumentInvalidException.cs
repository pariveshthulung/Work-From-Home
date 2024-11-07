namespace WorkFromHome.Domain.Exceptions;

public class ArgumentInvalidException : Exception
{
    public ArgumentInvalidException(string message)
        : base(message) { }

    public ArgumentInvalidException(string message, Exception innerException)
        : base(message, innerException) { }
}
