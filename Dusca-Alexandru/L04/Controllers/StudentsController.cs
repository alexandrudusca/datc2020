using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace L04.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private IStudentRepository _studentRepository;
        public StudentsController(IStudentRepository studentRepository) 
        {
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<StudentEntity>> Get()
        {
           return await _studentRepository.GetAllStudents();
        }

        [HttpPost]
        public async Task<string> Post([FromBody] StudentEntity student)
        {
        
            return await _studentRepository.CreateNewStudent(student);
    
        }

        [HttpDelete]
        public async Task<string> Delete([FromBody] StudentEntity student)
        {
            return await _studentRepository.DeleteStudent(student);
        }

        [HttpPut]
        public async Task<string> Put([FromBody] StudentEntity student)
        {
            return await _studentRepository.EditStudent(student);
        }

    }
}
