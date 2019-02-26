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

        public async Task AddSubject(Subject subject, bool saveToDB = true)
        {
            if (saveToDB)
            {
                SQLWriteFactory writeFactory = new SQLWriteFactory();
                SQLWrite SQLWrite = await writeFactory.GetInstance(DataHelper.DatabaseName);
                await SQLWrite.AddSubject(subject);
                int subjectId = await this.GetSubjectId(subject);
                subject.SubjectId = subjectId;
            }

            this.subjects.Add(subject);
            StudentBook.SubjectsObservable.Add(new SubjectListViewItem() { SubjectName = subject.Name, SubjectId = subject.SubjectId, Average = "0,00" } );
        }

        public async Task AddMark(Mark mark, Subject subject, bool saveToDB = true)
        {
            if (saveToDB)
            {
                mark.SubjectId = subject.SubjectId;
                SQLWriteFactory writeFactory = new SQLWriteFactory();
                SQLWrite SQLWrite = await writeFactory.GetInstance(DataHelper.DatabaseName);
                await SQLWrite.AddMark(mark);
                int markId = await this.GetMarkId(mark, subject);
                mark.MarkId = markId;
            }
            
            this.marks.Add(mark);
            int subjectListId = this.GetSubjectListId(subject);
            StudentBook.SubjectMarksObservable.Add(new MarkListViewItem() { MarkValue = mark.Value, MarkWeight = mark.Weight, MarkId = mark.MarkId });
            StudentBook.SubjectsObservable[subjectListId] = new SubjectListViewItem() { SubjectName = subject.Name, SubjectId = subject.SubjectId, Average = "0,00" };
        }

        public async Task DeleteSubject(Subject subject, float averange)
        {
            List<Mark> subjectMarks = await this.GetSubjectMarks(subject);

            foreach (Mark mark in subjectMarks)
            {
                await this.DeleteMark(mark);
            }

            int subjectListId = this.GetSubjectListId(subject);
            SQLWriteFactory writeFactory = new SQLWriteFactory();
            SQLWrite SQLWrite = await writeFactory.GetInstance(DataHelper.DatabaseName);
            await SQLWrite.DeleteSubject(subject);
            this.subjects.RemoveAt(subjectListId);
            StudentBook.SubjectsObservable.RemoveAt(subjectListId);
        }

        public async Task DeleteMark(Mark mark, float average = 0f)
        {
            int markObservableListId = this.GetMarkObservableListId(mark);
            int markListId = this.GetMarkListId(mark);
            SQLWriteFactory writeFactory = new SQLWriteFactory();
            SQLWrite SQLWrite = await writeFactory.GetInstance(DataHelper.DatabaseName);
            await SQLWrite.DeleteMark(mark);
            this.marks.RemoveAt(markListId);
            Subject subject = this.GetSubjectById(mark.SubjectId);
            int subjectListId = this.GetSubjectListId(subject);
            StudentBook.SubjectMarksObservable.RemoveAt(markObservableListId);
            StudentBook.SubjectsObservable[subjectListId] = new SubjectListViewItem() { SubjectName = subject.Name, SubjectId = subject.SubjectId, Average = average.ToString("0.00") };
        }

        public async Task<List<Subject>> GetSubjects()
        {
            if (this.subjects.Count == 0)
            {
                List<Subject> allSubjects = await this.getSubjectsFromDatabase();

                foreach (Subject subject in allSubjects)
                {
                    await this.AddSubject(subject, false);
                }
                
                return allSubjects;
            }

            return this.subjects;
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
                List<Mark> allMarks = await this.getMarksFromDatabase();

                foreach (Mark mark in allMarks)
                {
                    Subject subject = this.GetSubjectById(mark.SubjectId);
                    await this.AddMark(mark, subject, false);
                }
                
                return allMarks;
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

        public Subject GetSubjectById(int subjectId)
        {
            foreach (Subject subject in this.subjects)
            {
                if (subject.SubjectId == subjectId)
                {
                    return subject;
                }
            }

            return null;
        }

        public async Task<int> GetSubjectId(Subject subject)
        {
            SQLReadFactory readFactory = new SQLReadFactory();
            SQLRead SQLRead = await readFactory.GetInstance(DataHelper.DatabaseName);
            int subjectId = await SQLRead.GetSubjectId(subject);
            return subjectId;
        }

        public async Task<int> GetMarkId(Mark mark, Subject subject)
        {
            SQLReadFactory readFactory = new SQLReadFactory();
            SQLRead SQLRead = await readFactory.GetInstance(DataHelper.DatabaseName);
            int markId = await SQLRead.GetMarkId(mark, subject);
            return markId;
        }

        public int GetSubjectListId(Subject searchedSubject)
        {
            for (int i = 0; i < this.subjects.Count; i++)
            {
                if (this.subjects[i].SubjectId == searchedSubject.SubjectId)
                {
                    return i;
                }
            }

            return -1;
        }

        public int GetMarkListId(Mark searchedMark)
        {
            for (int i = 0; i < this.marks.Count; i++)
            {
                if (this.marks[i].MarkId == searchedMark.MarkId)
                {
                    return i;
                }
            }

            return -1;
        }

        public int GetMarkObservableListId(Mark searchedMark)
        {
            for (int i = 0; i < StudentBook.SubjectMarksObservable.Count; i++)
            {
                if (StudentBook.SubjectMarksObservable[i].MarkId == searchedMark.MarkId)
                {
                    return i;
                }
            }

            return -1;
        }

    }
}
