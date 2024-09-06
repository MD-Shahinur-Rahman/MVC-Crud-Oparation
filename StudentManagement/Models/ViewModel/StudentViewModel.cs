using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models.ViewModel
{
    public class StudentViewModel
    {
        [ Display(Name = "Student Id")]
        public int StudentId { get; set; }
        [Required, Display(Name = "Student Name")]
      
        public string StudentName { get; set; }
        [Display(Name = "Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
   
        public int Mobile { get; set; }
        [Required, Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        public string Image { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public virtual Student Student { get; set; } = null!;
        public virtual Course Course { get; set; } = null!; 
        [Required, Display(Name = "Picture")]
        public IFormFile ImgUrl { get; set; } = null!;
        
    }
}
