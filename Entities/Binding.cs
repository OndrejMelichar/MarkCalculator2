using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Entities
{
    public class Binding
    {
        [PrimaryKey, AutoIncrement]
        public int BindingId { get; set; }
        public int SubjectId { get; set; }
        public int MarkId { get; set; }
    }
}
