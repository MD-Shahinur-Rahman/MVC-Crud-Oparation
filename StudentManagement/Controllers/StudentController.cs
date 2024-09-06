using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.Models.ViewModel;
using System.Diagnostics.Contracts;

namespace StudentManagement.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public StudentController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        
        public IActionResult Index()
        {
            var students = _db.Students.Include(e => e.Course)
                .Select(s => new StudentViewModel
                {
                    StudentId = s.StudentId,
                    StudentName = s.StudentName,
                    Email = s.Email,
                    Mobile = s.Mobile,
                    DateOfBirth = s.DateOfBirth,
                    CourseName = s.Course.CourseName, // Assuming Course has a CourseName property
                    Image = s.Image,
                }).ToList();

            return View(students);
        }
   
        private string GetuploadFile(StudentViewModel vm)
        {
            string uniqueFile = null;
            if (vm.ImgUrl != null)
            {
                string uploadfolder = Path.Combine(_env.WebRootPath, "Images");
                uniqueFile = Guid.NewGuid().ToString() + Path.GetExtension(vm.ImgUrl.FileName);
                string filePath = Path.Combine(uploadfolder, uniqueFile);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    vm.ImgUrl.CopyTo(fileStream);
                }
            }
            return uniqueFile;
        }
            [HttpGet]
        public IActionResult Create()
        {

            ViewBag.course = new SelectList(_db.Courses, "CourseId", "CourseName");

            return View();
            
        }

        [HttpPost]
        public IActionResult Create(StudentViewModel SVM)
        {
            
                var student = new Student
                {
                    StudentName = SVM.StudentName,
                    Email = SVM.Email,
                    Mobile = SVM.Mobile,
                    DateOfBirth = SVM.DateOfBirth,
                    Image = SVM.ImgUrl != null ? GetuploadFile(SVM) : null,
                    CourseId = SVM.CourseId,
                };
                _db.Students.Add(student);
                _db.SaveChanges();

                  
          
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = _db.Students
                .Include(s => s.Course)
                .FirstOrDefault(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            ViewBag.course = new SelectList(_db.Courses, "CourseId", "CourseName");

            var viewModel = new StudentViewModel
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                Email = student.Email,
                Mobile = student.Mobile,
                DateOfBirth = student.DateOfBirth,
                Image = student.Image,
                CourseId = student.CourseId,
                CourseName = student.Course?.CourseName,
                ImgUrl = null
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(StudentViewModel svm)
        {
            if (!ModelState.IsValid)
            {
                // Reload the course data if the model state is invalid
                svm.CourseName = _db.Courses.FirstOrDefault(c => c.CourseId == svm.CourseId)?.CourseName;
                return View(svm);
            }

            var student = _db.Students
                .FirstOrDefault(s => s.StudentId == svm.StudentId);

            if (student == null)
            {
                return NotFound();
            }

            // Update student properties
            student.StudentName = svm.StudentName;
            student.Email = svm.Email;
            student.Mobile = svm.Mobile;
            student.DateOfBirth = svm.DateOfBirth;
            student.CourseId = svm.CourseId;

            // Handle image update
            if (svm.ImgUrl != null)
            {
                // Delete old image file if exists
                if (!string.IsNullOrEmpty(student.Image))
                {
                    var oldFilePath = Path.Combine(_env.WebRootPath, "Images", student.Image);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Save new image
                student.Image = GetuploadFile(svm);
            }

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public  IActionResult Delete(int id)
        {
            var student = _db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            _db.Students.Remove(student);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
    }

}
