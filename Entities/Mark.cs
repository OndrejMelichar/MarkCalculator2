using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Entities
{
    public class Mark
    {
        [PrimaryKey, AutoIncrement]
        public int MarkId { get; set; }
        public float Value { get; set; }
        public int Weight { get; set; }
    }
}
