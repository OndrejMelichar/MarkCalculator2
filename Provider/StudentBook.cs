using Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Provider
{
    public class StudentBook
    {
        public static ObservableCollection<SubjectListViewItem> SubjectsObservable = new ObservableCollection<SubjectListViewItem>();
        public static ObservableCollection<MarkListViewItem> SubjectMarksObservable = new ObservableCollection<MarkListViewItem>();
        private DataHelper dataHelper;

        public StudentBook()
        {
            this.dataHelper = new DataHelper();
        }

        public async Task<List<Subject>> GetSubjects()
        {
            return await this.dataHelper.GetSubjects();
        }
        
        public async Task AddSubject(Subject subject)
        {
            await this.dataHelper.AddSubject(subject);
        }

        public async Task AddMark(Mark mark, Subject subject)
        {
            await this.dataHelper.AddMark(mark, subject);
            await this.updateAverage(subject);
        }

        private async Task updateAverage(Subject subject)
        {
            List<Mark> marks = await this.GetSubjectMarks(subject);
            float average = this.GetMarksAverage(marks);
            int subjectId = this.dataHelper.GetSubjectListId(subject);
            SubjectListViewItem oldItem = StudentBook.SubjectsObservable[subjectId];
            StudentBook.SubjectsObservable.Insert(subjectId, new SubjectListViewItem() { SubjectId = oldItem.SubjectId, SubjectName = oldItem.SubjectName, Average = average.ToString("0.00") });
            StudentBook.SubjectsObservable.RemoveAt(subjectId + 1);
        }

        public async Task DeleteSubject(Subject subject)
        {
            List<Mark> subjectMarks = await this.GetSubjectMarks(subject);
            float averange = this.GetMarksAverage(subjectMarks);
            await this.dataHelper.DeleteSubject(subject, averange);
        }

        public async Task DeleteMark(Mark mark)
        {
            await this.dataHelper.DeleteMark(mark);
        }

        public async Task<List<Mark>> GetSubjectMarks(Subject subject, bool firstLoad = false)
        {
            List<Mark> subjectMarks = await this.dataHelper.GetSubjectMarks(subject);

            if (firstLoad)
            {
                await this.updateAverage(subject);
            }
            
            return subjectMarks;
        }

        public async Task<Status> GetSubjectStatus(Subject subject)
        {
            float status = this.GetMarksAverage(await this.GetSubjectMarks(subject));
            return this.GetStatusByAverage(status, true);
        }

        public async Task<Status> GetTotalStatus()
        {
            float totalStatus = 0f;
            List<Subject> subjects = await this.GetSubjects();

            if (subjects.Count != 0)
            {
                float statusSum = 0f;

                foreach (Subject subject in subjects)
                {
                    statusSum += this.GetMarksAverage(await this.GetSubjectMarks(subject));
                }

                totalStatus = statusSum / subjects.Count;
            }

            return this.GetStatusByAverage(totalStatus);
        }

        private Status GetStatusByAverage(float average, bool subjectStatus = false)
        {
            if (!subjectStatus && average <= 1.5f)
            {
                return Status.Distinction;
            }
            else if (average > 4.5f)
            {
                return Status.Failed;
            }

            return Status.Passed;
        }

        public float GetMarksAverage(List<Mark> marks)
        {
            float marksWeightsSum = 0;
            int weightsSum = 0;

            foreach (Mark mark in marks)
            {
                marksWeightsSum += mark.Value * mark.Weight;
                weightsSum += mark.Weight;
            }

            if (marks.Count != 0)
            {
                return marksWeightsSum / weightsSum;
            }

            return 0f;
        }

        public async Task<bool> SubjectNameExists(string name)
        {
            List<Subject> subjects = await this.GetSubjects();

            foreach (Subject subject in subjects)
            {
                if (subject.Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<int> GetSubjectId(Subject subject)
        {
            return await this.dataHelper.GetSubjectId(subject);
        }

        public string NormalizeSubjectName(string name)
        {
            return name.First().ToString().ToUpper() + name.Substring(1).ToLower();
        }
    }



    public enum Status
    {
        Distinction, Passed, Failed
    }
}
