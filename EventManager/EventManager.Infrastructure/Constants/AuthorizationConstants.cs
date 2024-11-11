namespace EventManager.Infrastructure.Constants;
public static class AuthorizationConstants
{
    public const string JWT_SECRET_KEY = "SecretKey_For_EventManager_WA_App_2024!@#$%";
    public const int JWT_EXPIRATION_MINUTES = 30;
    public const string ISSUER = "EventManager";
    public const string AUDIENCE = "EventManager.Client";
}