using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private static Student[] students =
        {
            new Student
            {
                StudentId = 1,
                StudentName = "Thabo Mokoena",
                Course = "Software Development",
                Age = 20
            },
            new Student
            {
                StudentId = 2,
                StudentName = "Lerato Nkosi",
                Course = "Information Technology",
                Age = 21
            },
            new Student
            {
                StudentId = 3,
                StudentName = "Kagiso Molefe",
                Course = "Computer Science",
                Age = 19
            }
        };

        // GET: api/students
        [HttpGet]
        public ActionResult<Student[]> GetStudents()
        {
            return Ok(students);
        }

        // GET: api/students/1
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            Student student = students.FirstOrDefault(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // POST: api/students
        [HttpPost]
        public ActionResult<Student> AddStudent(Student student)
        {
            int newId = students.Length + 1;

            student.StudentId = newId;

            Student[] newStudents = new Student[students.Length + 1];

            for (int i = 0; i < students.Length; i++)
            {
                newStudents[i] = students[i];
            }

            newStudents[newStudents.Length - 1] = student;

            students = newStudents;

            return CreatedAtAction(
                nameof(GetStudent),
                new { id = student.StudentId },
                student
            );
        }

        // PUT: api/students/1
        [HttpPut("{id}")]
        public ActionResult<Student> UpdateStudent(int id, Student updatedStudent)
        {
            Student student = students.FirstOrDefault(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            student.StudentName = updatedStudent.StudentName;
            student.Course = updatedStudent.Course;
            student.Age = updatedStudent.Age;

            return Ok(student);
        }

        //DELETE: api/students/1
        [HttpDelete("{id}")]
        public ActionResult DeleteStudent(int id)
        {
            Student student = students.FirstOrDefault(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            Student[] newStudent = students
                .Where(s => s.StudentId != id)
                .ToArray();
            students = newStudent;

            return NoContent();
        }

    }
} 