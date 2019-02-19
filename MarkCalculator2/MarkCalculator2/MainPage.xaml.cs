﻿using Entities;
using Provider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MarkCalculator2
{
    public partial class MainPage : ContentPage
    {
        private StudentBook studentBook;

        public MainPage()
        {
            InitializeComponent();

            this.studentBook = new StudentBook();

            Task.Run(async () => {
                await this.displaySubjects();
            }).ConfigureAwait(true);

            navigationGrid.BackgroundColor = ThemeCollors.StringToColor(ThemeCollors.DefaultNavigationColor);
            subjectsListView.ItemsSource = StudentBook.SubjectsObservable;
        }

        private async Task displaySubjects()
        {
            //await this.deleteAll();

            List<Subject> subjects = await this.studentBook.GetSubjects();
            StudentBook.SubjectsObservable = await this.subjectsToListViewCollection(subjects);
            //
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

        private async void subjectTapped(object sender, EventArgs e)
        {
            ViewCell viewCell = ((ViewCell)sender);
            Grid grid = viewCell.FindByName<Grid>("ViewCellGrid");
            SubjectListViewItem subjectListViewItem = (SubjectListViewItem)viewCell.BindingContext;
            grid.BackgroundColor = ThemeCollors.StringToColor(ThemeCollors.BackgroundClassic);
            Subject subject = new Subject() { Name = subjectListViewItem.SubjectName, SubjectId = subjectListViewItem.SubjectId };
            await Navigation.PushModalAsync(new SubjectDetailPage(subject, this.studentBook));
        }

        private async void addSubjectClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddSubjectPage(this.studentBook));
        }
    }
}
