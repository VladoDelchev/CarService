using CarService.DL.Interfaces;
using CarService.DL.Repositories;
using CarService.Models.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace CarService.DL
{
    public static class DependencyInjection
    {
        public static IServiceCollection 
            AddDataLayer(this IServiceCollection services,
                IConfiguration config)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            services.Configure<MongoDbConfiguration>(
                config.GetSection(
                    nameof(MongoDbConfiguration)));

            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            return services;

        }
    }
}
