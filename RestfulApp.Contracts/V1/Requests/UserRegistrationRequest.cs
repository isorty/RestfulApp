using RestfulApp.Contracts.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RestfulApp.Contracts.V1.Requests;

public class UserRegistrationRequest : IRequest
{
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}