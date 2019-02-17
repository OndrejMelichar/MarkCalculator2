﻿using Entities;
using Provider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarkCalculator2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SubjectDetailPage : ContentPage
	{
        private Subject subject;
        private StudentBook studentBook;

		public SubjectDetailPage(Subject subject, StudentBook studentBook)
		{
            /*InitializeComponent ();
            this.subject = subject;
            this.studentBook = studentBook;
            marksListView.ItemsSource = StudentBook.SubjectMarksObservable;
            navigationCenter.Text = this.subject.Name;
            navigationGrid.BackgroundColor = ThemeCollors.StringToColor(ThemeCollors.NavigationColor);*/

            InitializeComponent();
            this.subject = subject;
            this.studentBook = studentBook;

            Task.Run(async () => {
                await this.displayMarks();
            }).ConfigureAwait(true);

            navigationCenter.Text = this.subject.Name;
            navigationGrid.BackgroundColor = ThemeCollors.StringToColor(ThemeCollors.NavigationColor);
        }

        private async Task displayMarks()
        {
            List<Mark> marks = await this.studentBook.GetSubjectMarks(this.subject); //toto asi ne
            StudentBook.SubjectMarksObservable = this.marksToListViewCollection(marks);
            marksListView.ItemsSource = StudentBook.SubjectMarksObservable;
        }

        private ObservableCollection<MarkListViewItem> marksToListViewCollection(List<Mark> marks)
        {
            ObservableCollection<MarkListViewItem> collection = new ObservableCollection<MarkListViewItem>();

            foreach (Mark mark in marks)
            {
                collection.Add(new MarkListViewItem() { MarkValue = mark.Value, MarkWeight = mark.Weight });
            }

            return collection;
        }

        private async void backClicked(object sender, EventArgs e)
        {
            await this.Navigation.PopModalAsync();
        }

        private async void addMarkClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddMarkPage(this.subject, this.studentBook));
        }

        private void deleteMarkClicked(object sender, EventArgs e)
        {
            
        }
    }
}