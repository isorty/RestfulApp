using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestfulApp.Api.Data.Models;

public class ItemDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public IdentityUser User { get; set; }
}