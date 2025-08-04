
using Domain.Common.Results;

namespace Application.Common.Errors;

public static class ApplicationErrors
{
    public static Error TestError() => Error.Failure();
}
