using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace L04
{
    public class StudentRepository : IStudentRepository 
    {
        private string _connectionString;
        private CloudTableClient _tableClient;
        private CloudTable _studentsTable;
        public StudentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("AzureStorageAccountConnectionString"); // acces appsettings.json
            Task.Run(async () => { await InitializeTable(); })
            .GetAwaiter()
            .GetResult(); // va crea tabelul de studenti daca nu exista deja
        }
        public async Task CreateNewStudent(StudentEntity student)
        {
            //var insert = TableOperation.Insert(student); // inserare student
            
            //await _studentsTable.ExecuteAsync(insert);

            var jsonStudent = JsonConvert.SerializeObject(student);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(jsonStudent);
            var base64String = System.Convert.ToBase64String(plainTextBytes);

            QueueClient queueClient = new QueueClient(
                _connectionString,
                "students-queue"
            );
            queueClient.CreateIfNotExists();

            await queueClient.SendMessageAsync(base64String);
        }

        public async Task DeleteStudent(string partitionKey, string rowKey)
        {
            var getStudent = TableOperation.Retrieve<StudentEntity>(partitionKey, rowKey);
            var studentResult = await _studentsTable.ExecuteAsync(getStudent);
            var deleteStudent = TableOperation.Delete((StudentEntity)studentResult.Result);
            
            await _studentsTable.ExecuteAsync(deleteStudent);
        }

        public async Task EditStudent(StudentEntity student)
        {
           student.ETag = "*";
           var editStudent = TableOperation.Merge(student);
           
           await _studentsTable.ExecuteAsync(editStudent);
        }

        public async Task<List<StudentEntity>> GetAllStudents()
        {
            List<StudentEntity> students = new List<StudentEntity>();

            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();

            TableContinuationToken token = null; 
            do{
                TableQuerySegment<StudentEntity> resultSegment = await _studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
            
                students.AddRange(resultSegment.Results); // extragere sublista cu rezultate partiale si le adaugam la lista principala

            }while(token != null);

            return students;
        }

        private async Task InitializeTable()
        {
            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();

            _studentsTable = _tableClient.GetTableReference("studenti");

            await _studentsTable.CreateIfNotExistsAsync();
        }
    }
}