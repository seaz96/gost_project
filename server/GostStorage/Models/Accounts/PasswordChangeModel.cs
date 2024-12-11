using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GostStorage.Constraints;

namespace GostStorage.Models.Accounts;

public class PasswordChangeModel
{
    [JsonPropertyName("login")]
    public required string Login { get; set; }

    [JsonPropertyName("new_password")]
    [MinLength(LoginModelConstraints.PasswordMinLength)]
    [MaxLength(LoginModelConstraints.PasswordMaxLength)]
    public required string NewPassword { get; set; }

    [JsonPropertyName("old_password")]
    public required string OldPassword { get; set; }
}