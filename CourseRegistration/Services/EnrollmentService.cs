using CourseRegistration.Models;
using CourseRegistration.Storage;

namespace CourseRegistration.Services;

public record OperationResult(bool Success, string Message);

public class EnrollmentService
{
    private readonly InMemoryCourseStore _store;

    public EnrollmentService(InMemoryCourseStore store)
    {
        _store = store;
    }

    // Enroll a learner or add them to the waitlist
    public OperationResult Enroll(int learnerId)
    {
        var learner = _store.Learners
            .FirstOrDefault(x => x.Id == learnerId);

        if (learner == null)
            return new OperationResult(false, "Learner not found.");

        if (!learner.HasCompletedPrerequisite)
            return new OperationResult(false, "Prerequisite not completed.");

        bool alreadyRequested = _store.Enrollments
            .Any(x => x.LearnerId == learnerId);

        if (alreadyRequested)
            return new OperationResult(false, "Learner has already submitted an enrollment request.");

        int enrolledCount = _store.Enrollments
            .Count(x => x.Status == EnrollmentStatus.Enrolled);

        var status = enrolledCount < _store.Course.Capacity
            ? EnrollmentStatus.Enrolled
            : EnrollmentStatus.Waitlisted;

        _store.Enrollments.Add(new Enrollment
        {
            Id = _store.GetNextEnrollmentId(),
            LearnerId = learnerId,
            CourseId = _store.Course.Id,
            Status = status
        });

        if (status == EnrollmentStatus.Enrolled)
            return new OperationResult(true, "Enrollment successful.");

        return new OperationResult(true, "Course is full. Learner added to waitlist.");
    }

    // Drop an enrollment or cancel a waitlist request
    public OperationResult Drop(int learnerId)
    {
        var enrollment = _store.Enrollments
            .FirstOrDefault(x =>
                x.LearnerId == learnerId &&
                x.CourseId == _store.Course.Id &&
                (x.Status == EnrollmentStatus.Enrolled ||
                 x.Status == EnrollmentStatus.Waitlisted));

        if (enrollment == null)
            return new OperationResult(false, "No active enrollment found.");

        bool wasEnrolled = enrollment.Status == EnrollmentStatus.Enrolled;

        enrollment.Status = EnrollmentStatus.Dropped;

        if (!wasEnrolled)
            return new OperationResult(true, "Waitlist request cancelled.");

        PromoteNextEligibleLearner();

        return new OperationResult(true, "Enrollment dropped. Waitlist promotion completed.");
    }

    // Promote the earliest eligible waitlisted learner
    private void PromoteNextEligibleLearner()
    {
        var waitlistedEnrollments = _store.Enrollments
            .Where(x => x.Status == EnrollmentStatus.Waitlisted)
            .OrderBy(x => x.Id);

        foreach (var waitingEnrollment in waitlistedEnrollments)
        {
            var learner = _store.Learners
                .FirstOrDefault(x => x.Id == waitingEnrollment.LearnerId);

            if (learner == null || !learner.HasCompletedPrerequisite)
                continue;

            waitingEnrollment.Status = EnrollmentStatus.Enrolled;
            break;
        }
    }
}