﻿using FluentValidation;
using RestfulApp.Api.Contracts.V1.Requests;

namespace RestfulApp.Api.Validation.Validators;

public class UpdateItemRequestValidator : AbstractValidator<UpdateItemRequest>
{
    public UpdateItemRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*$");
    }
}