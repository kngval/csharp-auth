


using System.IdentityModel.Tokens.Jwt;

public class TokenHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? GetUserIdFromToken()
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            throw new InvalidOperationException("HttpContext is null");
        }
        var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer"))
        {
            return null;
        }

        var token = authHeader.Replace("Bearer ", "");
        var handler = new JwtSecurityTokenHandler();
        try
        {

            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
        } catch (Exception ex)
        {
          Console.WriteLine($"Error reading JWT token: {ex.Message}");
        }


     return null;
    }
}
