﻿using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using OneInteg.Server.DataAccess;
using OneInteg.Server.Domain.Repositories;
using OneInteg.Server.Domain.Services;
using OneInteg.Server.Services;

namespace OneInteg.Server.IoCConfig
{
    public static class ConfigureServicesExtensions
    {
        public static void AddCustomMongoDbService(this IServiceCollection services)
        {
            string mongoDbUri = Environment.GetEnvironmentVariable("MONGO_DB_URI");
            string databaseName = Environment.GetEnvironmentVariable("MONGO_DB_NAME");

            services.AddDbContext<OneIntegDbContext>(optionsBuilder => optionsBuilder.UseMongoDB(mongoDbUri, databaseName));

            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        }

        public static void AddServiceAndRepositories(this IServiceCollection services) 
        {
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

            services.AddScoped<ISubscriptionService, SubscriptionService>();
        }
    }
}
