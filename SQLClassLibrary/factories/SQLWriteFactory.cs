using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SQLClassLibrary
{
    public class SQLWriteFactory
    {
        private SQLWrite instance;

        public async Task<SQLWrite> GetInstance(string databaseName)
        {
            if (this.instance == null)
            {
                this.instance = new SQLWrite();
                this.instance.SetConnection(databaseName);
                await this.instance.LinkTables();
            }

            return this.instance;
        }
    }
}
