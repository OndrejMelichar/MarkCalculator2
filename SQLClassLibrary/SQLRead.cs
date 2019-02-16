using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Entities;
using SQLite;

namespace SQLClassLibrary
{
    public class SQLRead
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
        }

        public async Task<List<Subject>> GetSubjects()
        {
            AsyncTableQuery<Subject> query = databaseConnection.Table<Subject>();
            return await query.ToListAsync();
        }

        public async Task<List<Mark>> GetMarks()
        {
            AsyncTableQuery<Mark> query = databaseConnection.Table<Mark>();
            return await query.ToListAsync();
        }

        public async Task<int> GetSubjectId(Subject subject)
        {
            var query = databaseConnection.Table<Subject>().Where(v => v.Name.Equals(subject.Name));
            var data = await query.ToListAsync();
            return data[data.Count - 1].SubjectId;
        }

    }
}
