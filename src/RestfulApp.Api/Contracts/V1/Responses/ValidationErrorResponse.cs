using RestfulApp.Api.Contracts.Interfaces;

namespace RestfulApp.Api.Contracts.V1.Responses;

public sealed class ValidationErrorResponse : IResponse
{
    public List<ValidationErrorModel> Errors { get; set; } = new();
}