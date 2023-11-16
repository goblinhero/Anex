using System;
using System.Collections.Generic;

namespace Anex.Api.Exceptions;

public class NonValidException : Exception
{
    public NonValidException(object validatable, IEnumerable<string> errors)
        : base($"{validatable.GetType().Name} is not valid. Errors: {string.Join(", ", errors)}")
    {
    }
}