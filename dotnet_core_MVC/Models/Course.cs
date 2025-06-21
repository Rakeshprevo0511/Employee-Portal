namespace dotnet_core_MVC.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }

        public ICollection<EmployeeCourse> EmployeeCourses { get; set; }
    }
}
