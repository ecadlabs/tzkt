using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Netezos.Keys;

namespace Tzkt.Api.Services.Auth
{
    public class AuthConfig
    {
        public AuthMethod Method { get; set; } = AuthMethod.None;
        public int NonceLifetime { get; set; } = 10;
        public List<AuthUser> Users { get; set; } = new();
    }

    public static class AuthConfigExt
    {
        public static AuthConfig GetAuthConfig(this IConfiguration config)
        {
            return config.GetSection("Authentication")?.Get<AuthConfig>() ?? new();
        }

        public static void ValidateAuthConfig(this IConfiguration config)
        {
            var authConfig = config.GetAuthConfig();

            if (authConfig.Method < AuthMethod.None || authConfig.Method > AuthMethod.PubKey)
                throw new ConfigurationException("Invalid auth method");

            foreach (var user in authConfig.Users)
            {
                if (user.Name == null)
                    throw new ConfigurationException("Invalid user name");

                if (authConfig.Method == AuthMethod.PubKey)
                {
                    try { _ = PubKey.FromBase58(user.PubKey); }
                    catch { throw new ConfigurationException("Invalid user pubkey"); }
                }
                else if (authConfig.Method == AuthMethod.Password)
                {
                    if (user.Password == null)
                        throw new ConfigurationException("Invalid user password");
                }

                if (user.Rights != null)
                {
                    foreach (var right in user.Rights)
                    {
                        if (right.Access < Access.None || right.Access > Access.Write)
                            throw new ConfigurationException("Invalid user access type");
                    }
                }
            }
        }
    }
}