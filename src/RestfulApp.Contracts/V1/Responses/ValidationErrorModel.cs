namespace RestfulApp.Contracts.V1.Responses;

public class ValidationErrorModel
{
    public string FieldName { get; set; }
    public string Message { get; set; }
}