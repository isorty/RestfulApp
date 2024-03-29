﻿using FluentValidation;
using RestfulApp.Api.Contracts.V1.Requests;

namespace RestfulApp.Api.Validation.Validators;

public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
{
    public CreateItemRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*$");
    }
}