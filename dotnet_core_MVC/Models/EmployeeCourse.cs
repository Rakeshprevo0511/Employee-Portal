using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_core_MVC.Models
{
    public class EmployeeCourse
    {
        [Key]
        public int EmployeeCourseId { get; set; }
        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }

        [Required]
        public DateTime CompletionDate { get; set; }
        [Required]
        public string Status { get; set; } // Completed, In Progress, etc.
    }
}
