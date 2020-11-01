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
            try
            {
                await _studentRepository.CreateNewStudent(student);
                return "Studentul a fost adaugat cu succes!";
            }catch(System.Exception e)
            {
                return "Eroare la adaugare: " + e.Message;
            }
    
        }

        [HttpDelete("{_partitionKey}/{_rowKey}")]
        public async Task<string> Delete(string _partitionKey, string _rowKey)
        {
            try
            {
                await _studentRepository.DeleteStudent(_partitionKey, _rowKey);
                return "Studentul a fost sters din lista cu succes!";
            }catch(System.Exception e)
            {
                return "Studentul nu exista in lista: " + e.Message;
            }
        }

        [HttpPut]
        public async Task<string> Put([FromBody] StudentEntity student)
        {
            try
            {
                await _studentRepository.EditStudent(student);
                return "Datele studentul au fost editate cu succes";
            }catch(System.Exception e)
            {
                return "Eroare la modificare: " + e.Message;
            }
        }

    }
}
