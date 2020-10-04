using System.Collections.Generic;
namespace L02
{
    public static class StudentRepo
    {
        public static List<Student> Students = new List<Student>()
        {
            new Student() { Id = 1, Name = "Popescu Ion", Faculty = "ETC", StudyYear = 3, StudyType = "Taxa" },
            new Student() { Id = 2, Name = "Craciun Sorin", Faculty = "AC", StudyYear = 4, StudyType = "Fara taxa" },
            new Student() { Id = 3, Name = "Belciuc  Vitalie", Faculty = "AC", StudyYear = 1, StudyType = "Fara taxa" }
        };
        
    }
}