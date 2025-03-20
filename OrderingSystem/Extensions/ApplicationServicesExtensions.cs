using Microsoft.AspNetCore.Mvc;
using OrderingSystem.API.Errors;
using OrderingSystem.API.Helpers;
using OrderingSystem.Core.Repositories.Contract;
using OrderingSystem.Core.Services.Contract;
using OrderingSystem.Repository;
using OrderingSystem.Service;

namespace OrderingSystem.API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IProductService), typeof(ProductService));
          
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddAutoMapper(typeof(MappingProfile));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                         .SelectMany(p => p.Value.Errors)
                                                         .Select(e => e.ErrorMessage).ToArray();

                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });
            return services;
        }
    }
}
