using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Application.Exceptions;
public sealed class ValidationException : Exception
{
    public ValidationException(IEnumerable<ValidationError> validationErrors)
    {
        ValidationErrors = validationErrors;
    }

    public IEnumerable<ValidationError> ValidationErrors { get; }
}
