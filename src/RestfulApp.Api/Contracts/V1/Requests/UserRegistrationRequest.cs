using RestfulApp.Api.Contracts.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RestfulApp.Api.Contracts.V1.Requests;

public sealed class UserRegistrationRequest : IRequest
{
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}