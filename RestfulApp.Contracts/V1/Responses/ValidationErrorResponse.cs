using RestfulApp.Contracts.Interfaces;

namespace RestfulApp.Contracts.V1.Responses;

public class ValidationErrorResponse : IResponse
{
    public List<ValidationErrorModel> Errors { get; set; } = new();
}