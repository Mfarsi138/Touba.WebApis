using Touba.WebApis.API.Models;
using Touba.WebApis.IdentityServer.DataLayer;
using Touba.WebApis.IdentityServer.DataLayer.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Touba.WebApis.API.Data
{
    public class SeedData
    {
        public static async Task Seed(IApplicationBuilder appBuilder)
        {
            using var scope = appBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var logger = scope.ServiceProvider.GetService<ILogger>();
            var appSettings = scope.ServiceProvider.GetService<IOptions<AppSettings>>().Value;
            try
            {
                logger.Debug("Start to seed the information");

                logger.Debug("Migrating the PersistedGrantDbContext");
                await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync().ConfigureAwait(false);

                using var confDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                logger.Debug("Migrating the ConfigurationDbContext");
                await confDbContext.Database.MigrateAsync().ConfigureAwait(false);

                logger.Debug("Seeding the clients");
                foreach (var client in Clients.GetClients(appSettings))
                {
                    if (await confDbContext.Clients.AnyAsync(f => f.ClientId == client.ClientId).ConfigureAwait(false))
                        continue;

                    confDbContext.Clients.Add(client.ToEntity());
                }
                await confDbContext.SaveChangesAsync().ConfigureAwait(false);

                logger.Debug("Seeding the Identity resources");
                foreach (var resource in IdentityResources.GetIdentityResources())
                {
                    if (await confDbContext.IdentityResources.AnyAsync(f => f.Name == resource.Name).ConfigureAwait(false))
                        continue;

                    confDbContext.IdentityResources.Add(resource.ToEntity());
                }
                await confDbContext.SaveChangesAsync().ConfigureAwait(false);

                logger.Debug("Seeding the API Scopes");
                foreach (var apiScope in IdentityResources.GetApiScopes())
                {
                    if (await confDbContext.ApiScopes.AnyAsync(f => f.Name == apiScope.Name).ConfigureAwait(false))
                        continue;
                    confDbContext.ApiScopes.Add(apiScope.ToEntity());
                }
                await confDbContext.SaveChangesAsync().ConfigureAwait(false);

                logger.Debug("Seeding the API Resources");
                foreach (var resource in IdentityResources.GetApiResources())
                {
                    if (await confDbContext.ApiResources.AnyAsync(f => f.Name == resource.Name).ConfigureAwait(false))
                        continue;

                    confDbContext.ApiResources.Add(resource.ToEntity());
                }
                await confDbContext.SaveChangesAsync().ConfigureAwait(false);


                logger.Debug("Migrating the DataContext");
                await scope.ServiceProvider.GetRequiredService<DataContext>().Database.MigrateAsync().ConfigureAwait(false);

                using RoleManager<IdentityRole> roleManager =
                    scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                logger.Debug("Seeding the Roles");
                foreach (var role in typeof(Roles).GetFields())
                {
                    var currentRole = await roleManager.FindByNameAsync(role.GetValue(null).ToString()).ConfigureAwait(false);
                    if (currentRole is null)
                    {
                        var identityRole = new IdentityRole { Name = role.GetValue(null).ToString() };
                        await roleManager.CreateAsync(identityRole).ConfigureAwait(false);
                    }
                }

                logger.Debug("Seeding the perdefined users");
                using UserManager<User> userManager =
                    scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                foreach (var testUser in PredefinedUsers.GetAll())
                {
                    if (await userManager.FindByNameAsync(testUser.Username) != null)
                        continue;

                    var user = new User
                    {
                        UserName = testUser.Username,
                        FirstName = testUser.FirstName,
                        LastName = testUser.LastName,
                        IsActive = testUser.IsActive,
                        MiddleName = testUser.MiddleName,
                        PhoneNumberConfirmed = true,
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(user, testUser.Password).ConfigureAwait(false);

                    var claims = new List<Claim>();
                    foreach (var claim in testUser.Role.Permissions)
                    {
                        claims.Add(new Claim(claim.Replace(".", ""), claim));
                    }

                    await userManager.AddToRoleAsync(user, testUser.Role.Name).ConfigureAwait(false);
                    await userManager.AddClaimsAsync(user, claims).ConfigureAwait(false);
                }

                logger.Debug("Seeding completed");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred when migrating and seeding the information.");
            }
        }
    }
}
