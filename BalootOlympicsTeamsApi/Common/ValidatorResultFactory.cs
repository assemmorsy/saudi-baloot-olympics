using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Results;

namespace BalootOlympicsTeamsApi.Common;

public class ValidatorResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
    {
        var bodyError = new InvalidBodyInputError();
        foreach (var errors in validationProblemDetails!.Errors)
        {
            bodyError.ValidationErrors.Add(errors.Key, errors.Value.Select(e =>
                    {
                        if (e.StartsWith("Error converting value"))
                            return $"Invalid {errors.Key} , the provided data can't be casted to the target data type.";
                        return e;
                    }).ToList());
        };
        return bodyError.HandleToIActionResult(context.HttpContext.TraceIdentifier);
    }

    public IResult CreateResult(EndpointFilterInvocationContext context, ValidationResult validationResult)
    {
        var bodyError = new InvalidBodyInputError();
        foreach (ValidationFailure error in validationResult.Errors)
        {
            if (bodyError.ValidationErrors.TryGetValue(error.PropertyName, out var _))
                bodyError.ValidationErrors[error.PropertyName].Add(error.ErrorMessage);
            else
                bodyError.ValidationErrors.Add(error.PropertyName, [error.ErrorMessage]);
        };
        return bodyError.HandleToIResult(context.HttpContext.TraceIdentifier);
    }
}
