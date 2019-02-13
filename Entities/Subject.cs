using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Entities
{
    public class Subject
    {
        [PrimaryKey, AutoIncrement]
        public int SubjectId { get; set; }
        public string Name { get; set; }
    }
}
