using FluentValidation;
using RestfulApp.Contracts.V1.Requests;

namespace RestfulApp.Validators;

public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
{
    public CreateItemRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*$");
    }
}