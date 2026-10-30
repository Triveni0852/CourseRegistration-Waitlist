# Course Registration and Waitlist System

## Overview

A simple course registration system built using C# and .NET 8.

The application supports learner enrollment, course dropping, prerequisite validation, and FIFO waitlist promotion.

This project was developed as an AI-assisted engineering assessment.

## Features

- Enroll learners when seats are available.
- Add eligible learners to the waitlist when the course is full.
- Prevent duplicate enrollment requests.
- Validate learner prerequisites.
- Drop an active enrollment.
- Automatically promote the earliest eligible waitlisted learner.
- Skip ineligible learners during promotion while retaining their waitlist status.
- Cancel a waitlist request.

## Technology Stack

- C#
- .NET 8
- xUnit
- In-memory storage
- Visual Studio 2022

## Project Structure

```text
CourseRegistration/
│
├── CourseRegistration/
│   ├── Models/
│   │   ├── Learner.cs
│   │   ├── Course.cs
│   │   └── Enrollment.cs
│   │
│   ├── Services/
│   │   └── EnrollmentService.cs
│   │
│   ├── Storage/
│   │   └── InMemoryCourseStore.cs
│   │
│   ├── Program.cs
│   ├── Design-Specification.md
│   ├── AI-Prompt-Log.md
│   └── README.md
│
└── CourseRegistration.Tests/
    └── EnrollmentServiceTests.cs
```

## Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or .NET CLI

## Build the Project

```bash
dotnet build
```

## Run the Application

```bash
dotnet run --project CourseRegistration/CourseRegistration.csproj
```

The console application demonstrates enrollment, waitlisting, dropping, and automatic promotion.

## Run Automated Tests

```bash
dotnet test
```

The xUnit test suite covers:

1. Enrollment when a seat is available.
2. Waitlist when the course is full.
3. Duplicate enrollment prevention.
4. FIFO waitlist promotion.
5. Prerequisite validation.
6. Invalid and repeated drop requests.
7. Skipping an ineligible waitlisted learner.
8. Unknown learner validation.
9. Waitlist cancellation.
10. Re-enrollment prevention after dropping.
11. Invalid course capacity validation.

**Current verification:** All 12 test cases passed in Visual Studio Test Explorer.

## Waitlist Policy

The system follows First In, First Out (FIFO), using increasing enrollment IDs.

If a waitlisted learner is no longer eligible when a seat becomes available, the learner is skipped and remains on the waitlist. The next eligible learner is promoted.

## Storage and Limitations

The prototype uses in-memory storage. Data is not persisted after the application stops.

The current implementation does not provide transaction or locking protection for concurrent enrollment requests.

See `Design-Specification.md` for design decisions, assumptions, and trade-offs.

## AI Assistance

ChatGPT was used as an engineering assistant for requirement breakdown, design suggestions, implementation guidance, test-case suggestions, and documentation.

See `AI-Prompt-Log.md` for the AI usage record and verification details.