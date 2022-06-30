using Microsoft.Extensions.DependencyInjection;
using System;

namespace Touba.WebApis.Helpers.SecurityHelper
{
    public static class EncryptionDecryptionExtension
    {
        public static IServiceCollection AddEncryptionDecryptionHelper(this IServiceCollection services, EncryptionDecryptionConfig config)
        {
            services.AddTransient(f =>
            {
                return new EncryptionDecryptionHelper(config.Key);
            });
            return services;
        }

        public static IServiceCollection AddEncryptionDecryptionHelper(this IServiceCollection services, Action<EncryptionDecryptionConfig> config)
        {
            services.AddTransient(f =>
            {
                var conf = new EncryptionDecryptionConfig();
                config.Invoke(conf);
                return new EncryptionDecryptionHelper(conf.Key);
            });
            return services;
        }
    }
}
