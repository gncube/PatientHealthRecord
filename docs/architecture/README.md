# PatientHealthRecord - Architecture Documentation

This folder contains the architectural documentation for the Patient Health Record system.

## Documents

### Architecture Overview Document (AOD)
- **[PatientHealthRecord-AOD.md](./PatientHealthRecord-AOD.md)** - Complete Arc42-compliant architecture overview following C4 model standards
  - System context and boundaries
  - Solution architecture with technology stack
  - C4 diagrams (Context, Container, Component)
  - Cross-cutting concerns (security, scalability, resilience)
  - Quality scenarios and performance metrics
  - HIPAA compliance and healthcare-specific considerations
  - Roadmap and future architectural direction

## Architecture Decision Records (ADRs)

Architectural decisions are documented in the [architecture-decisions](../architecture-decisions/) folder:

- **[ADR-001](../architecture-decisions/adr-001-dotnet-di-adoption.md)** - .NET Core DI Adoption
- **[ADR-002](../architecture-decisions/adr-002-azure-functions-cold-starts.md)** - Azure Functions for Cold Start Mitigation

## Diagrams

All architectural diagrams are embedded in the AOD document using PlantUML format:

- **System Context Diagram** - Shows system boundaries and external actors
- **Container Diagram** - Shows high-level technology choices and container interactions
- **Component Diagram** - Shows internal structure of key containers
- **Sequence Diagrams** - Shows key user journey flows

## Standards and Guidelines

### Documentation Framework
- **Arc42 Template** - Structured architecture documentation
- **C4 Model** - Hierarchical architectural views
- **PlantUML** - Diagram as code for maintainability

### Architecture Principles
- **Clean Architecture** - Separation of concerns and dependency inversion
- **Domain-Driven Design** - Business-focused domain modeling
- **CQRS** - Command Query Responsibility Segregation
- **Privacy by Design** - HIPAA compliance built into architecture

## Review and Updates

- **Review Cycle**: Quarterly architecture reviews
- **Update Triggers**: Major feature releases, technology changes, compliance updates
- **Stakeholders**: Architecture team, development leads, security team
- **Next Review**: 2026-03-28

## Contact

For questions about the architecture or to propose changes:

- **Architecture Team**: Create an issue or ADR for architectural discussions
- **Security Concerns**: Contact the security team for compliance-related questions
- **Documentation Updates**: Submit PRs with changes to architecture documentation
