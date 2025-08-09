using Domain.Common.Results;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features
{
    public static class FluentValidationExtensions
    {
        public static List<Error> MapFromFluentValidationErrors(this List<ValidationFailure> errors)
            => [.. errors.Select(x => new Error(x.ErrorCode, x.ErrorMessage, ErrorKind.Validation))];
    }
}
