using CourseRegistration.Models;
using CourseRegistration.Services;
using CourseRegistration.Storage;

namespace CourseRegistration.Tests;

public class EnrollmentServiceTests
{
    private static (InMemoryCourseStore Store, EnrollmentService Service)
        CreateSystem(int capacity, params Learner[] learners)
    {
        var course = new Course
        {
            Id = 1,
            Name = "C# Programming",
            Capacity = capacity
        };

        var store = new InMemoryCourseStore(course);

        store.Learners.AddRange(learners);

        var service = new EnrollmentService(store);

        return (store, service);
    }

    private static Learner CreateLearner(int id, bool prerequisite = true)
    {
        return new Learner
        {
            Id = id,
            Name = $"Learner {id}",
            HasCompletedPrerequisite = prerequisite
        };
    }

    [Fact]
    public void Enroll_WhenSeatAvailable_ShouldEnrollLearner()
    {
        var (store, service) = CreateSystem(
            2, CreateLearner(1));

        var result = service.Enroll(1);

        Assert.True(result.Success);
        Assert.Equal(EnrollmentStatus.Enrolled,
            store.Enrollments[0].Status);
    }

    [Fact]
    public void Enroll_WhenCourseFull_ShouldAddToWaitlist()
    {
        var (store, service) = CreateSystem(
            1, CreateLearner(1), CreateLearner(2));

        service.Enroll(1);
        var result = service.Enroll(2);

        Assert.True(result.Success);
        Assert.Equal(EnrollmentStatus.Waitlisted,
            store.Enrollments[1].Status);
    }

    [Fact]
    public void Enroll_DuplicateRequest_ShouldBeRejected()
    {
        var (store, service) = CreateSystem(
            2, CreateLearner(1));

        service.Enroll(1);
        var result = service.Enroll(1);

        Assert.False(result.Success);
        Assert.Single(store.Enrollments);
    }

    [Fact]
    public void Drop_WhenEnrolledLearnerDrops_ShouldPromoteNextLearner()
    {
        var (store, service) = CreateSystem(
            1, CreateLearner(1), CreateLearner(2), CreateLearner(3));

        service.Enroll(1);
        service.Enroll(2);
        service.Enroll(3);

        var result = service.Drop(1);

        Assert.True(result.Success);

        Assert.Equal(EnrollmentStatus.Dropped,
            store.Enrollments[0].Status);

        Assert.Equal(EnrollmentStatus.Enrolled,
            store.Enrollments[1].Status);

        Assert.Equal(EnrollmentStatus.Waitlisted,
            store.Enrollments[2].Status);
    }

    [Fact]
    public void Enroll_WhenPrerequisiteNotCompleted_ShouldReject()
    {
        var (store, service) = CreateSystem(
            2, CreateLearner(1, false));

        var result = service.Enroll(1);

        Assert.False(result.Success);
        Assert.Empty(store.Enrollments);
    }

    [Fact]
    public void Drop_InvalidOrRepeatedRequest_ShouldBeRejected()
    {
        var (store, service) = CreateSystem(
            2, CreateLearner(1));

        var invalidResult = service.Drop(99);

        service.Enroll(1);
        service.Drop(1);

        var repeatedResult = service.Drop(1);

        Assert.False(invalidResult.Success);
        Assert.False(repeatedResult.Success);

        Assert.Equal(EnrollmentStatus.Dropped,
            store.Enrollments[0].Status);
    }

    [Fact]
    public void Drop_WhenFirstWaitlistedLearnerIsIneligible_ShouldSkipAndPromoteNext()
    {
        var learner1 = CreateLearner(1);
        var learner2 = CreateLearner(2);
        var learner3 = CreateLearner(3);

        var (store, service) = CreateSystem(
            1, learner1, learner2, learner3);

        service.Enroll(1);
        service.Enroll(2);
        service.Enroll(3);

        // Learner 2 becomes ineligible before promotion.
        learner2.HasCompletedPrerequisite = false;

        service.Drop(1);

        Assert.Equal(EnrollmentStatus.Waitlisted,
            store.Enrollments[1].Status);

        Assert.Equal(EnrollmentStatus.Enrolled,
            store.Enrollments[2].Status);
    }
    [Fact]
    public void Enroll_WhenLearnerDoesNotExist_ShouldReject()
    {
        var (store, service) = CreateSystem(2);

        var result = service.Enroll(99);

        Assert.False(result.Success);
        Assert.Empty(store.Enrollments);
    }

    [Fact]
    public void Drop_WhenWaitlistedLearnerDrops_ShouldCancelRequest()
    {
        var (store, service) = CreateSystem(
            1, CreateLearner(1), CreateLearner(2));

        service.Enroll(1);
        service.Enroll(2);

        var result = service.Drop(2);

        Assert.True(result.Success);
        Assert.Equal(
            EnrollmentStatus.Dropped,
            store.Enrollments[1].Status);

        Assert.Equal(
            EnrollmentStatus.Enrolled,
            store.Enrollments[0].Status);
    }

    [Fact]
    public void Enroll_AfterDropping_ShouldRejectDuplicateRequest()
    {
        var (store, service) = CreateSystem(
            2, CreateLearner(1));

        service.Enroll(1);
        service.Drop(1);

        var result = service.Enroll(1);

        Assert.False(result.Success);
        Assert.Single(store.Enrollments);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateStore_WithInvalidCapacity_ShouldThrow(int capacity)
    {
        var course = new Course
        {
            Id = 1,
            Name = "C# Programming",
            Capacity = capacity
        };

        Assert.Throws<ArgumentOutOfRangeException>(
            () => new InMemoryCourseStore(course));
    }
}