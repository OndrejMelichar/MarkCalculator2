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
            }).Wait();
        }

        private async Task displaySubjects()
        {
            List<Subject> subjects = await this.studentBook.GetSubjects();
            ObservableCollection<SubjectListViewItem> listViewCollection = await this.subjectsToListViewCollection(subjects);
            subjectsListView.ItemsSource = listViewCollection;
        }

        private async Task<ObservableCollection<SubjectListViewItem>> subjectsToListViewCollection(List<Subject> subjects)
        {
            ObservableCollection<SubjectListViewItem> collection = new ObservableCollection<SubjectListViewItem>();

            foreach (Subject subject in subjects)
            {
                List<Mark> subjectMarks = await this.studentBook.GetSubjectMarks(subject);
                float subjectAverage = this.studentBook.GetMarksAverage(subjectMarks);
                collection.Add(new SubjectListViewItem() { SubjectName = subject.Name, Average = subjectAverage } );
            }

            return collection;
        }
    }
}
