using PatientHealthRecord.Core.Interfaces;
using PatientHealthRecord.Core.Services;
using PatientHealthRecord.Infrastructure.Data;
using PatientHealthRecord.Infrastructure.Data.Queries;
using PatientHealthRecord.UseCases.Contributors.List;
using PatientHealthRecord.UseCases.Patients.List;
using PatientHealthRecord.UseCases.ClinicalObservations.List;
using PatientHealthRecord.UseCases.Conditions;
using PatientHealthRecord.UseCases.Conditions.List;


namespace PatientHealthRecord.Infrastructure;

public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    string? connectionString = config.GetConnectionString("SqliteConnection");
    Guard.Against.Null(connectionString);
    services.AddDbContext<AppDbContext>(options =>
     options.UseSqlite(connectionString));

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
           .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
           .AddScoped<IPatientRepository, PatientRepository>()
           .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
           .AddScoped<IListPatientsQueryService, ListPatientsQueryService>()
           .AddScoped<IListClinicalObservationsQueryService, ListClinicalObservationsQueryService>()
           .AddScoped<IListConditionsQueryService, PatientHealthRecord.Infrastructure.Data.Queries.ListConditionsQueryService>()
           .AddScoped<IDeleteContributorService, DeleteContributorService>();


    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
