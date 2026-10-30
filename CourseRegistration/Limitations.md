# Limitations and Future Improvements

## 1. Current Limitations

### In-memory storage
The application stores course, learner, and enrollment information in memory. All data is lost when the application stops.

### No concurrency protection
The current implementation does not use database transactions, locks, or other concurrency controls. Simultaneous enrollment requests could potentially exceed course capacity.

### Single course offering
The implementation supports only one course offering.

### No API or user interface
The application is a console-based prototype. It does not include a frontend or REST API.

### No persistent enrollment history
Enrollment records are retained only while the application is running.

## 2. Future Improvements

- Introduce SQL Server persistence.
- Use database transactions to protect enrollment and promotion operations.
- Add concurrency tests.
- Add REST API endpoints for enrollment and dropping.
- Support multiple course offerings.
- Add structured logging and improved error handling.
- Add integration tests using a persistent data store.

## 3. Scope

These limitations are intentional for this bounded assessment prototype. The implementation focuses on enrollment, dropping, prerequisite validation, duplicate prevention, and FIFO waitlist promotion.