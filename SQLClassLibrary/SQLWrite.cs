using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Entities;
using System.Threading.Tasks;

namespace SQLClassLibrary
{
    public class SQLWrite
    {
        private static SQLiteAsyncConnection databaseConnection;

        public void SetConnection(string databaseName)
        {
            string personalFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string fullPath = Path.Combine(personalFolderPath, databaseName);
            databaseConnection = new SQLiteAsyncConnection(fullPath);
        }

        public async Task LinkTables()
        {
            await databaseConnection.CreateTableAsync<Mark>();
            await databaseConnection.CreateTableAsync<Subject>();
            await databaseConnection.CreateTableAsync<Binding>();
        }
    }
}
