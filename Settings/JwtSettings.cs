using System.ComponentModel.DataAnnotations;

namespace AuthenticationExample.Settings;

public class JwtSettings
{
    public const string Key = "JWT";
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter valid audience")]
    public required string ValidAudience { get; init; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter valid issuer")]
    public required string ValidIssuer { get; init; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter secret")]
    public required string Secret { get; init; }
}