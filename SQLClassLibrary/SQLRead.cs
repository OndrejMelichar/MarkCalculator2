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

        /*public async Task<Mark> GetMarkByBinding(int bindingId)
        {
            AsyncTableQuery<Mark> query = databaseConnection.Table<Mark>().Where(v => v.MarkId.Equals(bindingId));
            List<Mark> list = await query.ToListAsync();
            return list[0];
        }

        public async Task<int> GetSubjectId(Subject subject)
        {
            var query = databaseConnection.Table<Subject>().Where(v => v.Name.Equals(subject.Name));
            var data = await query.ToListAsync();

            if (data.Count == 1)
            {
                return data[0].SubjectId;
            }

            // TODO chyba ve čtení databáze (? - vzít první, pokud vůbec existuje)
            return 0;
        }

        public async Task<List<int>> GetMarkIds(int subjectId)
        {
            var query = databaseConnection.Table<Binding>().Where(v => v.SubjectId.Equals(subjectId));
            var data = await query.ToListAsync();

            List<int> ids = new List<int>();

            foreach (Binding value in data)
            {
                ids.Add(value.MarkId);
            }

            return ids;
        }*/

        /*public async Task<int> GetLastMarkId(Mark mark)
        {
            var data = await databaseConnection.QueryAsync<Mark>("select * from Mark where Value = ? AND Weight = ?", mark.Value, mark.Weight);

            if (data.Count != 0)
            {
                data.Reverse();
                return data[0].MarkId;
            }

            return 0;
        }*/

    }
}
