# AI-Assisted Engineering – Prompt and Usage Log

## 1. AI Tool Used

**Tool:** ChatGPT

**Purpose:** Assistance with task decomposition, design decisions, C# implementation, unit test creation, documentation, and troubleshooting.

## 2. Engineering Activities

### Activity 1: Understanding and Breaking Down the Requirement

**Prompt summary:** Asked ChatGPT to explain the Course Waitlist assessment and divide the work into manageable implementation steps.

**AI response summary:**
- Identified enrollment, dropping, and FIFO waitlist promotion as the main operations.
- Identified prerequisite validation and duplicate request prevention.
- Recommended an in-memory implementation because no starter project was provided and the assessment allowed this fallback.

**Decision:** Accepted the proposed bounded implementation approach.

### Activity 2: Data Model and Storage Design

**Prompt summary:** Asked for simple C# model classes and an in-memory storage structure.

**AI response summary:**
- Suggested Learner, Course, and Enrollment models.
- Suggested an EnrollmentStatus enum.
- Suggested an in-memory store with an increasing enrollment ID.

**Decision:** Accepted the model structure and integrated the classes into the .NET 8 project.

### Activity 3: Enrollment and Waitlist Business Logic

**Prompt summary:** Asked for implementation guidance for enrollment, dropping, duplicate prevention, prerequisite validation, and FIFO promotion.

**AI response summary:**
- Suggested an EnrollmentService.
- Suggested checking available capacity before enrollment.
- Suggested ordering waitlisted requests by enrollment ID.
- Suggested skipping an ineligible learner during promotion while retaining that learner on the waitlist.

**Decision:** Accepted these business rules and implemented them in EnrollmentService.

### Activity 4: Automated Testing

**Prompt summary:** Asked for xUnit test cases covering the assessment acceptance criteria.

**AI response summary:**
- Suggested tests for available capacity, full capacity, duplicate requests, FIFO promotion, prerequisite failure, invalid drops, and ineligible waitlist promotion.

**Decision:** Added the suggested tests to EnrollmentServiceTests.cs.

**Verification:** Ran the tests in Visual Studio Test Explorer. All twelve tests passed.

### Activity 5: Console Demonstration

**Prompt summary:** Asked how to connect the service to Program.cs and demonstrate enrollment, waitlisting, dropping, and promotion.

**AI response summary:**
- Provided a sample course and learner setup.
- Demonstrated enrollment and waitlist results.
- Displayed the final enrollment status.

**Decision:** Integrated the demonstration into Program.cs.

**Verification:** Ran the console application successfully and checked the output. The first eligible waitlisted learner was promoted after a seat became available.

## 3. Suggestions Accepted, Edited, and Rejected

### Accepted
- Separation of models, services, and storage.
- In-memory storage for the bounded prototype.
- Increasing enrollment IDs for FIFO ordering.
- Skipping ineligible waitlisted learners while retaining their records.
- Focused xUnit tests.

### Edited
- Added a Dropped status to EnrollmentStatus to preserve enrollment history and distinguish dropped requests from active requests.
- Integrated the generated code into the Visual Studio project structure.

### Rejected
- No major AI-generated implementation suggestion has been explicitly rejected so far.

## 4. Independent Verification

The following checks were performed in Visual Studio:

1. Built and ran the .NET 8 console application.
2. Ran all twelve xUnit tests in Test Explorer.
3. Verified that all twelve tests passed and additional edge-case tests were added.
4. Checked the console output for enrollment, waitlisting, dropping, and promotion.
5. Compared the observed behavior with the documented business rules.

## 5. Current Limitations

- The implementation uses temporary in-memory storage.
- Concurrent enrollment requests are not protected by transactions or locking.
- No database integration or API layer is included.
- Additional edge-case and concurrency testing is still required.

## 6. AI Usage Transparency

ChatGPT assisted with design suggestions, implementation examples, unit test examples, explanations, and documentation.

The generated code was integrated into the Visual Studio project and executed locally. The twelve automated tests and console demonstration were used as verification evidence.

AI-generated suggestions were treated as assistance and were not considered proof of correctness without execution and testing.