namespace WorkFromHome.Domain.Exceptions;

public class DateTimeException : Exception
{
    public DateTimeException(string message)
        : base(message) { }

    public DateTimeException(string message, Exception innerException)
        : base(message, innerException) { }
}
