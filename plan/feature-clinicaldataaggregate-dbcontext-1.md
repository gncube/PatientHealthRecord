---
goal: "Add ClinicalDataAggregate entities to AppDbContext for database persistence"
version: 1.0
date_created: 2025-09-28
last_updated: 2025-09-28
owner: "PatientHealthRecord Team"
status: 'Planned'
tags: [feature, database, entity-framework, clinical-data]
---

# Introduction

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)

This plan describes the implementation of database persistence for the ClinicalDataAggregate entities (ClinicalObservation, Condition, Medication) by adding them to the AppDbContext. This will enable CRUD operations against the database for clinical data management.

## 1. Requirements & Constraints

- **REQ-001**: Add DbSet properties for ClinicalObservation, Condition, and Medication to AppDbContext.cs
- **REQ-002**: Fix namespace inconsistency in Medication.cs (currently in Entities.ClinicalDataAggregate, should be ClinicalDataAggregate)
- **REQ-003**: Create EntityTypeConfiguration classes for each clinical data entity in Infrastructure/Data/Config/
- **REQ-004**: Add necessary schema constants to DataSchemaConstants.cs for clinical data fields
- **REQ-005**: Create database migration for the new tables
- **REQ-006**: Ensure all entities follow existing EF Core configuration patterns
- **SEC-001**: Maintain data integrity and proper indexing for clinical data queries
- **CON-001**: Must not break existing database schema or functionality
- **CON-002**: All configurations must be compatible with SQLite database provider
- **GUD-001**: Follow existing patterns for entity configuration and migration naming
- **PAT-001**: Use owned types for value objects, proper enum conversions, and JSON serialization for complex types
- **VAL-001**: All entity configurations must include proper constraints, indexes, and data type mappings

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-001: Prepare entities and configurations for database integration

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-001 | Fix namespace in Medication.cs from Entities.ClinicalDataAggregate to ClinicalDataAggregate | | |
| TASK-002 | Add clinical data schema constants to DataSchemaConstants.cs (observation types, medication names, condition descriptions, etc.) | | |
| TASK-003 | Create ClinicalObservationConfiguration.cs in Infrastructure/Data/Config/ with proper mappings, indexes, and constraints | | |
| TASK-004 | Create ConditionConfiguration.cs in Infrastructure/Data/Config/ with enum conversions and relationships | | |
| TASK-005 | Create MedicationConfiguration.cs in Infrastructure/Data/Config/ with status enum and date handling | | |

### Implementation Phase 2

- GOAL-002: Integrate entities into DbContext and create migration

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-006 | Add DbSet properties for ClinicalObservation, Condition, and Medication to AppDbContext.cs | | |
| TASK-007 | Add using statements for ClinicalDataAggregate namespace in AppDbContext.cs | | |
| TASK-008 | Create EF Core migration for new clinical data tables | | |
| TASK-009 | Update any existing tests to account for new DbSets if needed | | |
| TASK-010 | Verify migration applies correctly and tables are created | | |

## 3. Alternatives

- **ALT-001**: Use separate DbContext for clinical data (not chosen: violates single responsibility for AppDbContext)
- **ALT-002**: Configure entities inline in OnModelCreating (not chosen: reduces maintainability and follows existing pattern)

## 4. Dependencies

- **DEP-001**: Existing ClinicalDataAggregate classes (ClinicalObservation, Condition, Medication)
- **DEP-002**: Entity Framework Core with SQLite provider
- **DEP-003**: Existing AppDbContext and configuration patterns

## 5. Files

- **FILE-001**: `src/PatientHealthRecord.Core/ClinicalDataAggregate/Medication.cs` (namespace fix)
- **FILE-002**: `src/PatientHealthRecord.Infrastructure/Data/Config/DataSchemaConstants.cs` (add constants)
- **FILE-003**: `src/PatientHealthRecord.Infrastructure/Data/Config/ClinicalObservationConfiguration.cs` (new)
- **FILE-004**: `src/PatientHealthRecord.Infrastructure/Data/Config/ConditionConfiguration.cs` (new)
- **FILE-005**: `src/PatientHealthRecord.Infrastructure/Data/Config/MedicationConfiguration.cs` (new)
- **FILE-006**: `src/PatientHealthRecord.Infrastructure/Data/AppDbContext.cs` (add DbSets)
- **FILE-007**: `src/PatientHealthRecord.Infrastructure/Data/Migrations/[timestamp]_AddClinicalDataTables.cs` (new migration)

## 6. Testing

- **TEST-001**: Unit tests for entity configurations (ensure proper mappings and constraints)
- **TEST-002**: Integration tests for DbContext with new DbSets
- **TEST-003**: Migration tests to verify schema creation
- **TEST-004**: Data seeding tests for clinical data if applicable

## 7. Risks & Assumptions

- **RISK-001**: Migration conflicts with existing schema
- **ASSUMPTION-001**: Clinical data entities follow existing domain patterns
- **ASSUMPTION-002**: SQLite compatibility maintained for all new configurations

## 8. Related Specifications / Further Reading

- [Existing AppDbContext configuration patterns]
- [Entity Framework Core documentation for SQLite](https://learn.microsoft.com/en-us/ef/core/providers/sqlite/)
- [feature-clinicaldataaggregate-1.md](plan/feature-clinicaldataaggregate-1.md)
