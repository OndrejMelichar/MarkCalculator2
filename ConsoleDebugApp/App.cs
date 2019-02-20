using Entities;
using Provider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDebugApp
{
    class App
    {
        private StudentBook studentBook;

        public void Start()
        {
            this.studentBook = new StudentBook();

            Task.Run(async () => {
                await this.startAsync();
            }).ConfigureAwait(true);
        }

        private async Task startAsync()
        {
            //await this.deleteAll();

            await this.displaySubjects();

            Subject s1 = new Subject() { Name = "politologie" };
            Subject s2 = new Subject() { Name = "sociologie" };
            Subject s3 = new Subject() { Name = "makroekonomie" };
            Mark m1 = new Mark() { Value = 1, Weight = 30 };
            Mark m2 = new Mark() { Value = 2, Weight = 30 };
            Mark m3 = new Mark() { Value = 3, Weight = 30 };
            Mark m4 = new Mark() { Value = 4, Weight = 30 };
            Mark m5 = new Mark() { Value = 5, Weight = 30 };
            /*await this.studentBook.AddSubject(s1);
            await this.studentBook.AddSubject(s2);
            await this.studentBook.AddMark(m1, s1);
            await this.studentBook.AddMark(m2, s2);
            await this.studentBook.AddMark(m3, s1);
            await this.studentBook.AddSubject(s3);*/

            foreach (SubjectListViewItem subjectItem in StudentBook.SubjectsObservable)
            {
                Console.Write("> " + subjectItem.SubjectName + " (" + subjectItem.Average + ") [");
                List<Mark> subjectMarks = await this.studentBook.GetSubjectMarks(new Subject() { Name = subjectItem.SubjectName, SubjectId = subjectItem.SubjectId });

                foreach (Mark mark in subjectMarks)
                {
                    Console.Write(mark.Value + " (" + mark.Weight + "); ");
                }

                Console.Write("];\n");
            }
        }

        private async Task displaySubjects()
        {
            List<Subject> subjects = await this.studentBook.GetSubjects();
            StudentBook.SubjectsObservable = await this.subjectsToListViewCollection(subjects);
        }

        private async Task deleteAll()
        {
            List<Subject> subjectsx = await this.studentBook.GetSubjects();

            foreach (Subject subjectx in subjectsx)
            {
                await this.studentBook.DeleteSubject(subjectx);
            }
        }

        private async Task<ObservableCollection<SubjectListViewItem>> subjectsToListViewCollection(List<Subject> subjects)
        {
            ObservableCollection<SubjectListViewItem> collection = new ObservableCollection<SubjectListViewItem>();

            foreach (Subject subject in subjects)
            {
                List<Mark> marks = await this.studentBook.GetSubjectMarks(subject);
                float average = this.studentBook.GetMarksAverage(marks);
                collection.Add(new SubjectListViewItem() { SubjectId = subject.SubjectId, SubjectName = subject.Name, Average = average });
            }

            return collection;
        }
    }
}
