namespace RestfulApp.Contracts.V1.Responses;

public class ValidationErrorResponse
{
    public List<ValidationErrorModel> Errors { get; set; } = new();
}