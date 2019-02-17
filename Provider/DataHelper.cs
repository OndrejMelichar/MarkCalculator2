using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities;
using SQLClassLibrary;

namespace Provider
{
    public class DataHelper
    {
        public static string DatabaseName = "database.db";

        private List<Subject> subjects = new List<Subject>();
        private List<Mark> marks = new List<Mark>();
        private Dictionary<Subject, List<Mark>> MarksBySubjects = new Dictionary<Subject, List<Mark>>();

        public async Task AddSubject(Subject subject)
        {
            SQLWriteFactory writeFactory = new SQLWriteFactory();
            SQLWrite SQLWrite = await writeFactory.GetInstance(DataHelper.DatabaseName);
            await SQLWrite.AddSubject(subject);
            int subjectId = await this.GetSubjectId(subject);
            Subject formedSubject = new Subject() { Name = subject.Name, SubjectId = subjectId };
            this.subjects.Add(formedSubject);
            this.MarksBySubjects.Add(formedSubject, new List<Mark>());
            StudentBook.SubjectsObservable.Add(new SubjectListViewItem() { SubjectName = subject.Name, SubjectId = subject.SubjectId, Average = 0f } );
        }

        public async Task AddMark(Mark mark, Subject subject, float average)
        {
            mark.SubjectId = subject.SubjectId;
            SQLWriteFactory writeFactory = new SQLWriteFactory();
            SQLWrite SQLWrite = await writeFactory.GetInstance(DataHelper.DatabaseName);
            await SQLWrite.AddMark(mark);
            this.marks.Add(mark);
            StudentBook.SubjectMarksObservable.Add(new MarkListViewItem() { MarkValue = mark.Value, MarkWeight = mark.Weight });
            int subjectListId = this.GetSubjectListId(subject);
            StudentBook.SubjectsObservable[subjectListId] = new SubjectListViewItem() { SubjectName = subject.Name, SubjectId = subject.SubjectId, Average = average };
        }

        public async Task<List<Subject>> GetSubjects()
        {
            if (this.subjects.Count == 0)
            {
                return await this.getSubjectsFromDatabase();
            }

            return this.subjects;
        }

        public async Task<Dictionary<Subject, List<Mark>>> GetMarksBySubjects()
        {
            if (this.MarksBySubjects.Count == 0)
            {
                return await this.getMarksBySubjectsFromDatabase();
            }

            return this.MarksBySubjects;
        }

        public async Task<List<Mark>> GetSubjectMarks(Subject subjectPattern)
        {
            List<Mark> subjectMarks = new List<Mark>();
            List<Mark> allMarks = await this.getMarks();

            foreach (Mark mark in allMarks)
            {
                if (mark.SubjectId == subjectPattern.SubjectId)
                {
                    subjectMarks.Add(mark);
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

        private async Task<List<Subject>> getSubjectsFromDatabase()
        {
            SQLReadFactory readFactory = new SQLReadFactory();
            SQLRead SQLRead = await readFactory.GetInstance(DataHelper.DatabaseName);
            List<Subject> SQLSubjects = await SQLRead.GetSubjects();
            return SQLSubjects;
        }

        private async Task<List<Mark>> getMarksFromDatabase()
        {
            SQLReadFactory readFactory = new SQLReadFactory();
            SQLRead SQLRead = await readFactory.GetInstance(DataHelper.DatabaseName);
            List<Mark> SQLMarks = await SQLRead.GetMarks();
            return SQLMarks;
        }

        private async Task<Dictionary<Subject, List<Mark>>> getMarksBySubjectsFromDatabase()
        {
            List<Subject> SQLSubjects = await this.GetSubjects();
            Dictionary<Subject, List<Mark>> dictionary = new Dictionary<Subject, List<Mark>>();

            foreach (Subject subject in SQLSubjects)
            {
                dictionary.Add(subject, await this.GetSubjectMarks(subject));
            }

            return dictionary;
        }

        public async Task<int> GetSubjectId(Subject subject)
        {
            SQLReadFactory readFactory = new SQLReadFactory();
            SQLRead SQLRead = await readFactory.GetInstance(DataHelper.DatabaseName);
            int subjectId = await SQLRead.GetSubjectId(subject);
            return subjectId;
        }

        public int GetSubjectListId(Subject searchedSubject)
        {
            for (int i = 0; i < this.subjects.Count; i++)
            {
                Subject subject = this.subjects[i];

                if (subject.Name == searchedSubject.Name && subject.SubjectId == searchedSubject.SubjectId)
                {
                    return i;
                }
            }

            return -1;
        }

    }
}
