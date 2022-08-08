using Microsoft.AspNetCore.Authentication;

namespace RecipeService.Auth;

public class ApiKeyAuthSchemeOptions : AuthenticationSchemeOptions
{
    public string ApiKey { get; set; } = "VerySecret"; // Move this to Azure Key Vault
}