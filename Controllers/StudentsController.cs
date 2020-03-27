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
                    com.CommandText = "select * from Student";

                    con.Open();
                    var dr = com.ExecuteReader();
                    while(dr.Read())
                    {
                        var st = new Student();
                        st.FirstName = dr["FirstName"].ToString();
                        st.LastName = dr["LastName"].ToString();
                        st.IndexNumber = dr["IndexNumber"].ToString();
                        st.BirthDate = dr["BirthDate"].ToString();
                        st.IdEnrollment = (int)(dr["IdEnrollment"]);
                        students.Add(st);
                    }
                }
            }
            return Ok(students);

        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Kowalski");
            } else if (id == 2)
            {
                return Ok("Malewski");
            }

            return NotFound("Student wasn't found");
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