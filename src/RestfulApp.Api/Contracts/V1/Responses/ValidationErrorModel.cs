namespace RestfulApp.Api.Contracts.V1.Responses;

public sealed class ValidationErrorModel
{
    public string FieldName { get; set; }
    public string Message { get; set; }
}