# Course Registration – Design Specification

## 1. Objective

Implement enrollment, dropping, and FIFO waitlist promotion for a single course offering.

The implementation uses C# and .NET 8 with an in-memory data store.

## 2. Service Boundary

The `EnrollmentService` handles the business operations:

- Enroll a learner.
- Add eligible learners to the waitlist when the course is full.
- Drop an active enrollment.
- Promote the earliest eligible waitlisted learner.

The service uses `InMemoryCourseStore` to access course, learner, and enrollment data.

## 3. Data Model

### Learner
- Id
- Name
- HasCompletedPrerequisite

### Course
- Id
- Name
- Capacity

### Enrollment
- Id
- LearnerId
- CourseId
- Status

Enrollment status values:
- Enrolled
- Waitlisted
- Dropped

## 4. Business Rules and Invariants

1. A learner can submit only one enrollment request for this course.
2. A learner must complete the prerequisite before enrolling.
3. Enrollment is successful when a seat is available.
4. When the course is full, an eligible learner is added to the waitlist.
5. Dropping an enrolled learner opens a seat.
6. The earliest eligible waitlisted learner is promoted based on enrollment ID.
7. An ineligible learner is skipped during promotion but remains on the waitlist.
8. A dropped enrollment cannot be dropped again.
9. A waitlisted learner can cancel their waitlist request.

## 5. Waitlist Policy

The system follows FIFO (First In, First Out).

Enrollment IDs are generated in increasing order. The system checks waitlisted learners in ascending enrollment ID order.

If a learner at the front of the waitlist is no longer eligible, that learner is skipped and remains waitlisted. The next eligible learner is promoted.

## 6. Persistence and Concurrency

The implementation uses an in-memory store with lists.

Data is temporary and is lost when the application stops.

The current implementation is designed for a single-process demonstration and does not provide database transactions or thread-safe concurrent enrollment handling.

For production, persistent storage and transaction or locking mechanisms would be required to prevent over-enrollment during simultaneous requests.

## 7. Assumptions and Trade-offs

- Only one course offering is supported.
- The application uses in-memory storage.
- No frontend, authentication, payment, or external identity integration is included.
- A learner cannot submit another enrollment request after dropping.
- Ineligible waitlisted learners remain in the queue and are skipped during promotion.

## 8. Testing

Automated tests use xUnit.

The tests cover:
- Enrollment when capacity is available.
- Waitlist when the course is full.
- Duplicate enrollment requests.
- FIFO promotion.
- Prerequisite validation.
- Invalid and repeated drops.
- Skipping an ineligible waitlisted learner.
- Unknown learner validation.
- Waitlist cancellation.
- Re-enrollment prevention after dropping.
- Invalid course capacity validation.
All 12 test cases passed in Visual Studio Test Explorer