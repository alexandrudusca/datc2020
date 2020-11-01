using System;
using L04;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DATC.Students
{
    public static class StudentsQueueTrigger
    {
        [FunctionName("StudentsQueueTrigger")]
        [return: Table("studenti")]
        public static StudentEntity Run([QueueTrigger("students-queue", Connection = "storageaccountdatc292b5_STORAGE")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var student = JsonConvert.DeserializeObject<StudentEntity>(myQueueItem);

            return student;
        }
    }
}
