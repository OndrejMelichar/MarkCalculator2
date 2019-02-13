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

        public async void AddMark(Mark mark, Subject subject, string databaseName)
        {
            await databaseConnection.InsertAsync(mark);
            SQLReadFactory readFactory = new SQLReadFactory();
            SQLRead SQLRead = await readFactory.GetInstance(databaseName);
            int newMarkId = await SQLRead.GetLastSameMarkId(mark);
            int subjectId = await SQLRead.GetSubjectId(subject);

            await databaseConnection.InsertAsync(new Binding() { SubjectId = subjectId, MarkId = newMarkId });
        }

        public async void AddSubject(Subject subject)
        {
            await databaseConnection.InsertAsync(subject);
        }
        
    }
}
