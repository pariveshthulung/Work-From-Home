using FluentValidation.Results;

namespace Clean.Application.Exceptions;

public class ValidationException : ApplicationException
{
    private readonly List<string> _error = [];

    public ValidationException(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            _error.Add(error.ErrorMessage);
        }
    }
}
