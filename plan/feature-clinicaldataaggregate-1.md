---
goal: "Implement ClinicalDataAggregate and ClinicalObservation class for family-focused clinical data (DDD)"
version: 1.0
date_created: 2025-09-27
last_updated: 2025-09-27
owner: "PatientHealthRecord Team"
status: 'Planned'
tags: [feature, DDD, clinical, aggregate, family]
---

# Introduction

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)

This plan describes the implementation of a new ClinicalDataAggregate and a simple, family-focused ClinicalObservation class using Domain-Driven Design (DDD) principles. The goal is to enable structured clinical data capture and aggregation for family health records.

## 1. Requirements & Constraints

- **REQ-001**: Implement ClinicalDataAggregate as a DDD aggregate root in `src/PatientHealthRecord.Core/ClinicalDataAggregate/`
- **REQ-002**: Implement ClinicalObservation as an entity class in `src/PatientHealthRecord.Core/ClinicalDataAggregate/ClinicalObservation.cs` with:
    - Properties: PatientId, ObservationType, Value, Unit, RecordedAt, Notes, RecordedBy, Category, IsVisibleToFamily
    - Methods: UpdateVisibility, AddNotes, Weight, Height, BloodPressure (static helpers)
    - Enum: ObservationCategory (Vital, Symptom, Medication, Exercise, General)
    - Constructor validation using Guard clauses
- **REQ-003**: Ensure ClinicalDataAggregate supports CRUD operations for ClinicalObservation, including add, update, remove, and query by PatientId and Category
- **REQ-004**: Use C# and .NET conventions, match existing aggregate/entity patterns
- **SEC-001**: No PHI or sensitive data in code or comments
- **CON-001**: Must not break existing Patient or Contributor aggregates
- **CON-002**: All new code must be covered by unit tests, including edge cases for visibility, notes, and helper methods
- **GUD-001**: Follow DDD best practices for aggregate boundaries and entity design
- **PAT-001**: Use entity for mutable clinical observation data, value objects for immutable supporting types
- **VAL-001**: Validation: All ClinicalObservation properties must be set via constructor or helper methods, and all Guard clauses must be tested

## 2. Implementation Steps

### Implementation Phase 1
- GOAL-001: Create ClinicalDataAggregate and ClinicalObservation class structure

| Task      | Description                                                                 | Completed | Date       |
|-----------|-----------------------------------------------------------------------------|-----------|------------|
| TASK-001  | Create `ClinicalDataAggregate` class in `src/PatientHealthRecord.Core/ClinicalDataAggregate/ClinicalDataAggregate.cs` |           |            |
| TASK-002  | Implement `ClinicalObservation` class in `src/PatientHealthRecord.Core/ClinicalDataAggregate/ClinicalObservation.cs` with all specified properties, methods, and enum |           |            |
| TASK-003  | Define aggregate root and observation relationships, including CRUD and query methods for ClinicalObservation |           |            |

### Implementation Phase 2
- GOAL-002: Implement CRUD operations and unit tests

| Task      | Description                                                                 | Completed | Date       |
|-----------|-----------------------------------------------------------------------------|-----------|------------|
| TASK-004  | Implement add, update, remove, and query methods for ClinicalObservation in ClinicalDataAggregate |           |            |
| TASK-005  | Create unit tests for ClinicalObservation (constructor, Guard clauses, helper methods, visibility, notes) and ClinicalDataAggregate CRUD logic in `tests/PatientHealthRecord.UnitTests/ClinicalDataAggregate/` |           |            |
| TASK-006  | Document public APIs, usage examples for helper methods, and update README |           |            |

## 3. Alternatives

- **ALT-001**: Use a flat list of observations without aggregate root (not chosen: violates DDD)
- **ALT-002**: Store observations directly in Patient aggregate (not chosen: separation of concerns)

## 4. Dependencies

- **DEP-001**: .NET 8+ SDK
- **DEP-002**: Existing PatientHealthRecord.Core domain model

## 5. Files

- **FILE-001**: `src/PatientHealthRecord.Core/ClinicalDataAggregate/ClinicalDataAggregate.cs` (new)
- **FILE-002**: `src/PatientHealthRecord.Core/ClinicalDataAggregate/ClinicalObservation.cs` (new)
- **FILE-003**: `tests/PatientHealthRecord.UnitTests/ClinicalDataAggregate/ClinicalDataAggregateTests.cs` (new)
- **FILE-004**: `src/PatientHealthRecord.Core/README.md` (update)

## 6. Testing
- **TEST-001**: Unit tests for ClinicalDataAggregate CRUD operations, including add, update, remove, and query by PatientId and Category
- **TEST-002**: Unit tests for ClinicalObservation entity, including constructor validation, Guard clauses, UpdateVisibility, AddNotes, and all static helper methods (Weight, Height, BloodPressure)
- **TEST-003**: Unit tests for ObservationCategory enum usage and edge cases

## 7. Risks & Assumptions

- **RISK-001**: Aggregate boundary may need adjustment for future clinical data types
- **ASSUMPTION-001**: Family-focused observation structure is sufficient for initial use case

## 8. Related Specifications / Further Reading

- [docs/architecture-decisions/adr-001-dotnet-di-adoption.md]
- [Microsoft DDD documentation](https://learn.microsoft.com/en-us/dotnet/architecture/ddd/)
