using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace L02.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return StudentRepo.Students;
        }

        [HttpGet("{id}")]
        public Student Get(int id)
        {
            Student stud = new Student() { Id = 0, Name = "----", Faculty = "----", StudyYear = 0, StudyType = "----" }; // exemplu daca id-ul cautat nu exista
            foreach(var idCautat in StudentRepo.Students) // parcurgere recursiva
            {
                if(idCautat.Id == id) // daca obiectul a fost gasit in lista
                    stud = idCautat; // va returna obiectul suprascris, in caz contrar obiectul exemplu va ramane nemodificat
            }
            return stud;
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            int status = 0;
            foreach(var idCautat in StudentRepo.Students.ToList()) 
            {
                if(idCautat.Id == id) // daca obiectul a fost gasit in lista
                {
                    StudentRepo.Students.Remove(idCautat); // obiectul va fi sters din lista curenta
                    status = 1; // status de stergere
                }
            }
            if(status == 0) // daca obiectul nu a fost sters
                return "Studentul nu exista in lista!";
            else // in caz contrar
                return "Studentul a fost sters din lista cu succes!";
        }

        [HttpPost]
        public string Post([FromBody] Student student)
        {
            try
            {
                foreach(var idCautat in StudentRepo.Students.ToList()) // parcurgere recursiva
                {
                    if(idCautat.Id == student.Id) // daca obiectul deja exista in lista
                        throw new Exception("Exista student cu acest ID!"); // raspunde cu eroare
                }
                StudentRepo.Students.Add(student); // altfel va adauga studentul nou in lista curenta
                return "Studentul a fost adaugat cu succes!";
            }catch(System.Exception e) // daca a aparut o eroare la adaugare
            {
                return "Eroare la adaugare: " + e.Message;
            }
        }

        [HttpPut]
        public string Put([FromBody] Student student)
        {
            int status = 0;
            try
            {
                foreach(var idCautat in StudentRepo.Students.ToList()) // parcurgere recursiva
                {
                    if(idCautat.Id == student.Id) // daca obiectul a fost gasit in lista va fi inlocuit cu date noi
                    {
                        StudentRepo.Students.Remove(idCautat);
                        StudentRepo.Students.Add(student);
                        status = 1;
                    }
                }
                if(status == 1) 
                    return "Datele studentului au fost modificate cu succes!";
                else
                    throw new Exception("Nu exista student cu acest ID!");
            }catch(System.Exception e) // daca a aparut o eroare la modificare
            {
                return "Eroare la modificare: " + e.Message;
            }
        }
    }
}
