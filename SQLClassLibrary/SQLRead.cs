using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            await databaseConnection.CreateTableAsync<Binding>();
        }

        public async Task<ObservableCollection<Subject>> GetSubjects()
        {
            AsyncTableQuery<Subject> query = databaseConnection.Table<Subject>();
            List<Subject> queryList = await query.ToListAsync();
            return new ObservableCollection<Subject>(queryList);
        }

        public async Task<List<Mark>> GetMarks()
        {
            AsyncTableQuery<Mark> query = databaseConnection.Table<Mark>();
            return await query.ToListAsync();
        }

        public async Task<List<Binding>> GetBindings()
        {
            AsyncTableQuery<Binding> query = databaseConnection.Table<Binding>();
            return await query.ToListAsync();
        }

        public async Task<int> GetLastSameMarkId(Mark mark)
        {
            var data = await databaseConnection.QueryAsync<Mark>("select * from Mark where Value = ? AND Weight = ?", mark.Value, mark.Weight);
            data.Reverse();
            return data[0].MarkId;
        }

        public async Task<int> GetSubjectId(Subject subject)
        {
            var query = databaseConnection.Table<Subject>().Where(v => v.Name.Equals(subject.Name));
            var data = await query.ToListAsync();
            return data[0].SubjectId;
        }

    }
}
