using ContactsManager.Infrastructure;
using ContactsManager.Interfaces;
using ContactsManager.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContactsManager.Extentions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services, string corsPolicy)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy,
                    builder => builder.WithOrigins(
                                "https://localhost:44350",
                                "http://localhost:8080"
                            )
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("ContactConn");
            services.AddDbContext<RepositoryContext>(opts => 
                    opts.UseSqlServer(connectionString)
                );
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IAsyncRepository<Contact>, GenericRepository<Contact>>();
            services.AddScoped<IAsyncRepository<Career>, GenericRepository<Career>>();
        }

        /// <summary>
        /// Converts JSON object value to DateTime object
        /// </summary>
        public class DateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateTime.Parse(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd"));
            }
        }
    }
}
