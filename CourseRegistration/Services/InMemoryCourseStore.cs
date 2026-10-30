using CourseRegistration.Models;

namespace CourseRegistration.Storage;

public class InMemoryCourseStore
{
    public Course Course { get; }

    public List<Learner> Learners { get; } = new();

    public List<Enrollment> Enrollments { get; } = new();

    private long _nextEnrollmentId = 1;

    public InMemoryCourseStore(Course course)
    {
        if (course == null)
            throw new ArgumentNullException(nameof(course));

        if (course.Capacity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(course.Capacity),
                "Course capacity must be greater than zero.");

        Course = course;
    }
    public long GetNextEnrollmentId()
    {
        return _nextEnrollmentId++;
    }
}