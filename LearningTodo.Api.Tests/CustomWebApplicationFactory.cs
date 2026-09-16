using System;
using LearningTodo.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using LearningTodo.Api.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace LearningTodo.Api.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(Services =>
            {
                var dbContextDescriptor = Services.SingleOrDefault(service => service.ServiceType == typeof(IDbContextOptionsConfiguration<AppDbContext>));
                if (dbContextDescriptor != null)
                {
                    Services.Remove(dbContextDescriptor);
                }

                Services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();
                    return connection;
                });

                Services.AddDbContext<AppDbContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });


            });
        }

    }
}
