using System.Collections.Generic;
using System.Threading.Tasks;
using L04;
public interface IStudentRepository // interfata ce defineste metodele
{
    Task<List<StudentEntity>> GetAllStudents();

    Task CreateNewStudent(StudentEntity student);
    Task DeleteStudent(string partitionKey, string rowKey);
    Task EditStudent(StudentEntity student);
}