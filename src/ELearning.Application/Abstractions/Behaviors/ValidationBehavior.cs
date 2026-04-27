using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Exceptions;
using ELearning.Domain.Shared;
using FluentValidation;
using MediatR;

namespace ELearning.Application.Abstractions.Behaviors;
internal sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<IRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<IRequest>> _validators = validators;
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        if (!_validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context)));

        var validationFailures = validationResults
            .Where(v => !v.IsValid)
            .SelectMany(v => v.Errors)
            .Select(failure => new ValidationError(
                failure.PropertyName,
                failure.ErrorMessage)
            ).ToList();

        if (validationFailures.Any())
        {
            throw new Exceptions.ValidationException(validationFailures);
        }

        return await next(cancellationToken);
    }
}
