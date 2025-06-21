namespace dotnet_core_MVC.DTO
{
    public class EmployeeCourseDto
    {
        public int EmployeeCourseId { get; set; }
        public int EmployeeId { get; set; }
        public int CourseId { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Status { get; set; }
    }
}
