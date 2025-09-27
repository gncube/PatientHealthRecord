using Ardalis.SharedKernel;
using PatientHealthRecord.Core.ContributorAggregate;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.UseCases.Contributors.Create;
using PatientHealthRecord.UseCases.Patients.List;
using MediatR;
using System.Reflection;

namespace PatientHealthRecord.Web.Configurations;

public static class MediatrConfigs
{
  public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
  {
    var mediatRAssemblies = new[]
      {
        Assembly.GetAssembly(typeof(Contributor)), // Core
        Assembly.GetAssembly(typeof(Patient)), // Core - Patient
        Assembly.GetAssembly(typeof(CreateContributorCommand)), // UseCases
        Assembly.GetAssembly(typeof(ListPatientsQuery)) // UseCases - Patient
      };

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

    return services;
  }
}
