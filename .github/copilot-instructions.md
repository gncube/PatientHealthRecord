# Patient Health Record - Copilot Instructions

## Overview

This document provides essential guidance for AI coding assistants working on the Patient Health Record codebase. It covers architectural patterns, coding conventions, and project-specific practices to ensure consistent, high-quality contributions.

## Architecture Overview

### Solution Structure

- **PatientHealthRecord.Core**: Domain entities, aggregates, value objects, and core business logic
- **PatientHealthRecord.UseCases**: Application layer with commands, queries, and DTOs
- **PatientHealthRecord.Infrastructure**: Data access, external services, and infrastructure concerns
- **PatientHealthRecord.Web**: ASP.NET Core web API with FastEndpoints
- **PatientHealthRecord.AspireHost**: Application host using .NET Aspire

### Key Patterns

- **CQRS**: Commands for write operations, queries for read operations
- **Domain-Driven Design**: Aggregates, entities, value objects, and domain services
- **Clean Architecture**: Strict separation of concerns with dependency inversion
- **MediatR**: In-process messaging for decoupling request/response handling

## Coding Conventions

### Naming Conventions

#### Data Transfer Objects (DTOs)

- **Standard**: Use "Dto" suffix for all data transfer objects (not "DTO")
- **Examples**: `PatientDto`, `ContributorDto`, `ClinicalObservationDto`
- **Consistency**: Apply this convention across all layers and files

#### General Naming

- **Classes**: PascalCase (e.g., `PatientAggregate`, `UpdatePatientCommand`)
- **Methods**: PascalCase (e.g., `Handle`, `ExecuteAsync`)
- **Properties**: PascalCase (e.g., `PatientId`, `FirstName`)
- **Private Fields**: camelCase with underscore prefix (e.g., `_context`, `_logger`)
- **Local Variables**: camelCase (e.g., `patient`, `result`)
- **Constants**: UPPER_SNAKE_CASE (e.g., `DEFAULT_PAGE_SIZE`)

### File Organization

- **Commands/Queries**: Group by feature in subdirectories (e.g., `Patients/Create/`, `Contributors/Update/`)
- **DTOs**: Place in feature directories alongside related commands/queries
- **Handlers**: Co-locate with their corresponding commands/queries
- **Tests**: Mirror source structure in `tests/` directory

### Code Style

- **Records**: Use record types for immutable DTOs and value objects
- **Async/Await**: Always use async/await for I/O operations
- **LINQ**: Prefer method syntax over query syntax
- **Null Handling**: Use nullable reference types and null-coalescing operators
- **Dependency Injection**: Constructor injection preferred, avoid service locator pattern

## Common Patterns

### Command/Query Handlers

```csharp
public class CreatePatientHandler : ICommandHandler<CreatePatientCommand, Result<PatientDto>>
{
    private readonly IPatientRepository _repository;
    private readonly ILogger<CreatePatientHandler> _logger;

    public CreatePatientHandler(IPatientRepository repository, ILogger<CreatePatientHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<PatientDto>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

### Repository Pattern

```csharp
public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(PatientId id, CancellationToken cancellationToken = default);
    Task AddAsync(Patient patient, CancellationToken cancellationToken = default);
    Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default);
}
```

### Result Pattern

```csharp
public record Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public string? Error { get; init; }

    public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };
    public static Result<T> Failure(string error) => new() { IsSuccess = false, Error = error };
}
```

## Testing Guidelines

### Unit Tests

- **Framework**: xUnit with FluentAssertions
- **Naming**: `MethodName_Condition_ExpectedResult`
- **Structure**: Arrange, Act, Assert (AAA) pattern
- **Mocking**: Use Moq for dependencies
- **Coverage**: Aim for high coverage of business logic

### Integration Tests

- **Database**: Use in-memory SQLite for fast, isolated tests
- **Web API**: Test endpoints with `WebApplicationFactory`
- **External Services**: Mock or use test containers

## Performance Considerations

### Database

- **EF Core**: Use async operations and `AsNoTracking()` for read-only queries
- **Indexing**: Ensure proper indexes on frequently queried columns
- **Pagination**: Always implement pagination for list endpoints
- **N+1 Queries**: Avoid with `Include()` or `Select()` projections

### API Design

- **FastEndpoints**: Use for type-safe, performant API endpoints
- **Caching**: Implement response caching for stable data
- **Compression**: Enable gzip compression for responses
- **Rate Limiting**: Implement rate limiting for public endpoints

## Security Practices

### Input Validation

- **DTOs**: Validate all input DTOs with DataAnnotations or FluentValidation
- **Sanitization**: Sanitize user input to prevent XSS
- **SQL Injection**: Use parameterized queries (EF Core handles this automatically)

### Authentication & Authorization

- **JWT**: Use JWT tokens for API authentication
- **Role-Based**: Implement role-based access control
- **Principle of Least Privilege**: Grant minimum required permissions

## Development Workflow

### Git Practices

- **Branching**: Feature branches from `main`
- **Commits**: Clear, descriptive commit messages
- **PRs**: Include description, link to issues, and test results

### Code Quality

- **Linting**: Use Roslyn analyzers and StyleCop
- **Reviews**: Require code reviews for all changes
- **CI/CD**: Automated builds, tests, and deployments

## Tooling

### Essential Tools

- **.NET 9**: Latest LTS version
- **Visual Studio 2022**: Primary IDE
- **Entity Framework Core**: ORM for data access
- **Serilog**: Structured logging
- **MediatR**: In-process messaging
- **FluentValidation**: Input validation
- **xUnit**: Unit testing
- **Moq**: Mocking framework

### Extensions

- **FastEndpoints**: High-performance API framework
- **Scalar.AspNetCore**: Interactive API documentation
- **Ardalis.SharedKernel**: Shared kernel for DDD patterns

## Common Pitfalls to Avoid

1. **Blocking Calls**: Never use `.Result` or `.Wait()` in async code
2. **Large Result Sets**: Always paginate database queries
3. **Tight Coupling**: Use dependency injection and interfaces
4. **Magic Strings**: Use constants or enums instead
5. **Business Logic in Controllers**: Keep controllers thin, move logic to handlers
6. **Direct Database Access**: Always go through repositories
7. **Ignoring Cancellation**: Always respect `CancellationToken`
8. **Memory Leaks**: Dispose of resources properly
9. **Race Conditions**: Use proper synchronization for shared state
10. **Inconsistent Naming**: Follow established naming conventions

## Getting Started

1. **Setup**: Clone repository and run `dotnet restore`
2. **Database**: Run migrations with `dotnet ef database update`
3. **Run**: Start with `dotnet run --project src/PatientHealthRecord.Web`
4. **Test**: Run tests with `dotnet test`
5. **API Docs**: Visit `/scalar` for interactive documentation

## Contributing

- Follow the established patterns and conventions
- Write tests for new functionality
- Update documentation as needed
- Ensure code reviews and CI/CD passes
- Keep commits focused and descriptive

This document evolves with the codebase. Update it when introducing new patterns or conventions.
