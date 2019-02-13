using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SQLClassLibrary
{
    public class SQLReadFactory
    {
        private SQLRead instance;

        public async Task<SQLRead> GetInstance(string databaseName)
        {
            if (this.instance == null)
            {
                this.instance = new SQLRead();
                this.instance.SetConnection(databaseName);
                await this.instance.LinkTables();
            }

            return this.instance;
        }
    }
}
