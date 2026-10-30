using CourseRegistration.Models;
using CourseRegistration.Services;
using CourseRegistration.Storage;

// Create course
var course = new Course
{
    Id = 1,
    Name = "C# Programming",
    Capacity = 2
};

// Create learners
var store = new InMemoryCourseStore(course);

store.Learners.AddRange(new[]
{
    new Learner { Id = 1, Name = "Rahul", HasCompletedPrerequisite = true },
    new Learner { Id = 2, Name = "Priya", HasCompletedPrerequisite = true },
    new Learner { Id = 3, Name = "Arun", HasCompletedPrerequisite = true },
    new Learner { Id = 4, Name = "Sneha", HasCompletedPrerequisite = true }
});

// Create service
var service = new EnrollmentService(store);

// Enroll learners
Console.WriteLine(service.Enroll(1).Message);
Console.WriteLine(service.Enroll(2).Message);
Console.WriteLine(service.Enroll(3).Message);
Console.WriteLine(service.Enroll(4).Message);

// Drop one learner
Console.WriteLine("\nRahul drops the course:");
Console.WriteLine(service.Drop(1).Message);

// Display enrollment status
Console.WriteLine("\nFinal Enrollment Status:");

foreach (var enrollment in store.Enrollments)
{
    var learner = store.Learners
        .First(x => x.Id == enrollment.LearnerId);

    Console.WriteLine($"{learner.Name}: {enrollment.Status}");
}