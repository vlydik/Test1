using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test1.Models
{
    public class Task
    {
        public Task(int idTask, string name, string description, DateTime deadLine) 
        {
            IdTask = idTask;
            Name = name;
            Description = description;
            DeadLine = deadLine;
            
        }
        public int IdTask { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine { get; set; }

        public List<Task> Tasks{ get; set; }

    }
}
