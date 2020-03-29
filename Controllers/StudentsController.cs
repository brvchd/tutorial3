using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tut3.Models;
using tut3.DAL;
using System.Data;
using System.Data.SqlClient;

namespace tut3.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        
        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            var students = new List<Student>();
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18963;Integrated Security=True"))
            {
                using(var com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = "Select FirstName, LastName, BirthDate, Name, Semester From Enrollment, Student, Studies Where Enrollment.IdStudy = Studies.IdStudy AND Enrollment.IdEnrollment = Student.IdEnrollment; ";

                    con.Open();
                    var dr = com.ExecuteReader();
                    while(dr.Read())
                    {
                        var st = new Student();
                        st.FirstName = dr["FirstName"].ToString();
                        st.LastName = dr["LastName"].ToString();
                        st.BirthDate = dr["BirthDate"].ToString();
                        st.enrollment = new Enrollment 
                        { 
                            IdSemester = (int)dr["Semester"], 
                            study = new Studies { Name = dr["Name"].ToString()} 
                        };
                        

                        students.Add(st);
                    }
                }
            }
            return Ok(students);

        }

      [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            var enroll = new Enrollment();
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18963;Integrated Security=True"))
            {
                using (var com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = "select Semester from Student, Enrollment, Studies Where Student.IndexNumber=@id AND Enrollment.IdStudy = Studies.IdStudy AND Enrollment.IdEnrollment = Student.IdEnrollment";
                    com.Parameters.AddWithValue("id", id);
                    con.Open();
                    var dr = com.ExecuteReader();
                    while(dr.Read())
                    {
                        enroll.IdSemester = (int)dr["Semester"];
                    }
                   
                }
            }
            return Ok(enroll);

        }
        

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            //... add to database
            //... generating index number
            student.IndexNumber = $"s{new Random().Next(1,2000)}";
            return Ok(student);

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            return Ok("Deleted Completed");
        }

        [HttpPut("{id}")]
        public IActionResult PutStudent(int id)
        {
            return Ok("Update completed");
        }
    }
}