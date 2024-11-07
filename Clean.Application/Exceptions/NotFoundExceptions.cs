using System;

namespace Clean.Application.Exceptions;

public class NotFoundExceptions : Exception
{
    public NotFoundExceptions(string name, object id)
        : base($"{name} of {id} not found.") { }
}
