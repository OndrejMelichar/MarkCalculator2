using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Entities;
using SQLClassLibrary;

namespace Provider
{
    public class DataHelper
    {
        private string databaseName;

        private ObservableCollection<Subject> subjects = new ObservableCollection<Subject>();
        private List<Mark> marks = new List<Mark>();
        private Dictionary<Subject, ObservableCollection<Mark>> MarksBySubjects = new Dictionary<Subject, ObservableCollection<Mark>>();

        public DataHelper(string databaseName)
        {
            this.databaseName = databaseName;
        }

        public async Task<ObservableCollection<Subject>> GetSubjects()
        {
            if (this.subjects.Count == 0)
            {
                return await this.getSubjectsFromDatabase();
            }

            return this.subjects;
        }

        public async Task<Dictionary<Subject, ObservableCollection<Mark>>> GetMarksBySubjects()
        {
            if (this.MarksBySubjects.Count == 0)
            {
                return await this.getMarksBySubjectsFromDatabase();
            }

            return this.MarksBySubjects;
        }

        public async Task<ObservableCollection<Mark>> GetSubjectMarks(Subject subject)
        {
            ObservableCollection<Mark> subjectMarks = new ObservableCollection<Mark>();
            List<Mark> allMarks = await this.getMarks();
            SQLReadFactory readFactory = new SQLReadFactory();
            SQLRead SQLRead = await readFactory.GetInstance(this.databaseName);
            List<Binding> SQLBindings = await SQLRead.GetBindings();

            foreach (Binding binding in SQLBindings)
            {
                if (binding.SubjectId == subject.SubjectId)
                {
                    subjectMarks.Add(this.getMarkById(binding.MarkId, allMarks));
                }
            }

            return subjectMarks;
        }

        private async Task<List<Mark>> getMarks()
        {
            if (this.marks.Count == 0)
            {
                return await this.getMarksFromDatabase();
            }

            return this.marks;
        }

        private async Task<ObservableCollection<Subject>> getSubjectsFromDatabase()
        {
            SQLReadFactory readFactory = new SQLReadFactory();
            SQLRead SQLRead = await readFactory.GetInstance(this.databaseName);
            ObservableCollection<Subject> SQLSubjects = await SQLRead.GetSubjects();
            return SQLSubjects;
        }

        private async Task<List<Mark>> getMarksFromDatabase()
        {
            SQLReadFactory readFactory = new SQLReadFactory();
            SQLRead SQLRead = await readFactory.GetInstance(this.databaseName);
            List<Mark> SQLMarks = await SQLRead.GetMarks();
            return SQLMarks;
        }

        private async Task<Dictionary<Subject, ObservableCollection<Mark>>> getMarksBySubjectsFromDatabase()
        {
            ObservableCollection<Subject> SQLSubjects = await this.GetSubjects();
            Dictionary<Subject, ObservableCollection<Mark>> dictionary = new Dictionary<Subject, ObservableCollection<Mark>>();

            foreach (Subject subject in SQLSubjects)
            {
                dictionary.Add(subject, await this.GetSubjectMarks(subject));
            }

            return dictionary;
        }

        private Mark getMarkById(int id, List<Mark> allMarks)
        {
            foreach (Mark mark in allMarks)
            {
                if (mark.MarkId == id)
                {
                    return mark;
                }
            }

            return null;
        }

    }
}
