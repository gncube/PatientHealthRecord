using Microsoft.Extensions.DependencyInjection;

namespace PatientHealthRecord.Core;

public static class CoreServiceExtensions
{
  public static IServiceCollection AddCoreServices(this IServiceCollection services, ILogger logger)
  {
    //services.AddScoped<IToDoItemSearchService, ToDoItemSearchService>();
    //services.AddScoped<IDeleteContributorService, DeleteContributorService>();

    logger.LogInformation("{Project} services registered", "Core");

    return services;
  }
}
