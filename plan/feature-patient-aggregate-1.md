---
goal: Implement Patient Aggregate: CreatePatient, GetPatient, GetFamilyMembers, GetFamilyDashboard
version: 1.0
date_created: 2025-09-27
last_updated: 2025-09-27
owner: PatientHealthRecord Team
status: 'Completed'
tags: [feature, patient, aggregate, api, dashboard]
---

# Introduction

![Status: Completed](https://img.shields.io/badge/status-Completed-green)

This plan defines the implementation of core Patient aggregate features: CreatePatient, GetPatient, GetFamilyMembers, and GetFamilyDashboard. The approach follows the established patterns used for Contributors, ensuring modularity, testability, and compliance with domain and security requirements.

## 1. Requirements & Constraints

- **REQ-001**: Implement CreatePatient command and handler for patient creation.
- **REQ-002**: Implement GetPatient query and handler to retrieve patient details.
- **REQ-003**: Implement GetFamilyMembers query and handler to list family members for a patient.
- **REQ-004**: Implement GetFamilyDashboard query and handler to provide a dashboard view for a patient's family.
- **SEC-001**: Ensure all patient data is handled securely and complies with privacy regulations.
- **CON-001**: Use existing aggregate, service, and DTO patterns as in Contributors.
- **GUD-001**: All endpoints must be covered by integration and unit tests.
- **PAT-001**: Use MediatR for CQRS command/query handling.

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-001: Define Patient aggregate, DTOs, and interfaces.

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-001 | Create `PatientAggregate` class in `src/PatientHealthRecord.Core/PatientAggregate/` | Yes | 2025-09-27 |
| TASK-002 | Define `PatientDTO` in `src/PatientHealthRecord.UseCases/PatientDTO.cs` | Yes | 2025-09-27 |
| TASK-003 | Add interfaces for patient repository and services in `src/PatientHealthRecord.Core/Interfaces/` | Yes | 2025-09-27 |

### Implementation Phase 2

- GOAL-002: Implement commands, queries, handlers, and endpoints.

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-004 | Implement `CreatePatientCommand` and handler in `src/PatientHealthRecord.UseCases/Patients/` | Yes | 2025-09-27 |
| TASK-005 | Implement `GetPatientQuery` and handler in `src/PatientHealthRecord.UseCases/Patients/` | Yes | 2025-09-27 |
| TASK-006 | Implement `GetFamilyMembersQuery` and handler in `src/PatientHealthRecord.UseCases/Patients/` | Yes | 2025-09-27 |
| TASK-007 | Implement `GetFamilyDashboardQuery` and handler in `src/PatientHealthRecord.UseCases/Patients/` | Yes | 2025-09-27 |
| TASK-008 | Add API endpoints in `src/PatientHealthRecord.Web/Patients/` | Yes | 2025-09-27 |

### Implementation Phase 4

- GOAL-004: Expose all Patient use cases in Swagger endpoints.

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-013 | Create POST /Patients endpoint for CreatePatient | Yes | 2025-09-27 |
| TASK-014 | Create GET /Patients/{id} endpoint for GetPatient | Yes | 2025-09-27 |
| TASK-015 | Create GET /Patients/{familyId}/family endpoint for GetFamilyMembers | Yes | 2025-09-27 |
| TASK-016 | Create GET /Patients/{familyId}/dashboard endpoint for GetFamilyDashboard | Yes | 2025-09-27 |
| TASK-017 | Validate all endpoints are visible in Swagger UI | Yes | 2025-09-27 |

## 3. Alternatives

- **ALT-001**: Directly implement logic in controllers (not chosen: violates separation of concerns and testability).
- **ALT-002**: Use a generic aggregate for all entities (not chosen: reduces domain clarity and maintainability).

## 4. Dependencies

- **DEP-001**: MediatR for CQRS pattern.
- **DEP-002**: Existing Contributor aggregate and service patterns.
- **DEP-003**: Entity Framework Core for data access.

## 5. Files

- **FILE-001**: `src/PatientHealthRecord.Core/PatientAggregate/PatientAggregate.cs` - Patient domain model.
- **FILE-002**: `src/PatientHealthRecord.UseCases/PatientDTO.cs` - Data transfer object.
- **FILE-003**: `src/PatientHealthRecord.Core/Interfaces/IPatientRepository.cs` - Patient repository interface.
- **FILE-004**: `src/PatientHealthRecord.UseCases/Patients/CreatePatientCommand.cs` - Command for patient creation.
- **FILE-005**: `src/PatientHealthRecord.UseCases/Patients/GetPatientQuery.cs` - Query for patient retrieval.
- **FILE-006**: `src/PatientHealthRecord.UseCases/Patients/GetFamilyMembersQuery.cs` - Query for family members.
- **FILE-007**: `src/PatientHealthRecord.UseCases/Patients/GetFamilyDashboardQuery.cs` - Query for family dashboard.
- **FILE-008**: `src/PatientHealthRecord.Web/Patients/PatientController.cs` - API endpoints.
- **FILE-009**: `src/PatientHealthRecord.Infrastructure/Data/PatientRepository.cs` - Data access implementation.
- **FILE-010**: `tests/PatientHealthRecord.UnitTests/Core/PatientAggregateTests.cs` - Unit tests.
- **FILE-011**: `tests/PatientHealthRecord.IntegrationTests/Data/PatientRepositoryTests.cs` - Integration tests.
- **FILE-012**: `tests/PatientHealthRecord.FunctionalTests/ApiEndpoints/PatientApiTests.cs` - Functional API tests.

## 6. Testing

- **TEST-001**: Unit tests for Patient aggregate and DTOs.
- **TEST-002**: Integration tests for repository and data access.
- **TEST-003**: Functional tests for API endpoints and command/query handlers.
- **TEST-004**: Security/privacy tests for patient data handling.

## 7. Risks & Assumptions

- **RISK-001**: Incomplete domain modeling may lead to future refactoring.
- **RISK-002**: Privacy/security issues if patient data is mishandled.
- **ASSUMPTION-001**: Contributor patterns are stable and can be reused for Patient aggregate.

## 8. Related Specifications / Further Reading

- [feature-clinicaldataaggregate-1.md](./feature-clinicaldataaggregate-1.md)
- [adr-001-dotnet-di-adoption.md](../docs/architecture-decisions/adr-001-dotnet-di-adoption.md)
- [CQRS with MediatR](https://github.com/jbogard/MediatR)
- [Entity Framework Core Docs](https://learn.microsoft.com/en-us/ef/core/)

## 9. Validation Summary

**Test Results**: All 9 tests pass (3 unit, 3 integration, 3 functional)

- **Unit Tests**: Patient aggregate domain logic validated
- **Integration Tests**: Repository data access confirmed
- **Functional Tests**: API endpoints and handlers working correctly
- **Build Status**: Successful compilation and migration
- **Code Quality**: Clean architecture following DDD and CQRS patterns
- **API Documentation**: All Patient endpoints exposed in Swagger UI:
  - POST /Patients - Create new patient
  - GET /Patients - List all patients
  - GET /Patients/{id} - Get patient by ID
  - GET /Patients/{familyId}/family - Get family members
  - GET /Patients/{familyId}/dashboard - Get family dashboard
