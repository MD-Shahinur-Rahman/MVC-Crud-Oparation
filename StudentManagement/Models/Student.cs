
namespace StudentManagement.Models
{
    public class Student
    {
        internal readonly IFormFile ImgUrl;

        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public int Mobile { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Image { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
