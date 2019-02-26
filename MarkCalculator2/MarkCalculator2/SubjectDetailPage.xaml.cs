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
            InitializeComponent();
            this.subject = subject;
            this.studentBook = studentBook;

            Task.Run(async () => {
                await this.displayMarks();
            }).ConfigureAwait(true);

            navigationCenter.Text = this.normalizeSubjectName(this.subject.Name);
            navigationGrid.BackgroundColor = ThemeCollors.StringToColor(ThemeCollors.DefaultNavigationColor);
            marksListView.ItemsSource = StudentBook.SubjectMarksObservable;
        }

        private string normalizeSubjectName(string name)
        {
            if (name.Length >= 15)
            {
                name = name.Substring(0, 12);
                name += "...";
            }

            return name;
        }

        private async Task displayMarks()
        {
            StudentBook.SubjectMarksObservable.Clear();
            List<Mark> marks = await this.studentBook.GetSubjectMarks(this.subject);

            foreach (Mark mark in marks)
            {
                StudentBook.SubjectMarksObservable.Add(new MarkListViewItem() { MarkId = mark.MarkId, SubjectId = mark.SubjectId, MarkValue = mark.Value, MarkWeight = mark.Weight });
            }
        }

        private async void backClicked(object sender, EventArgs e)
        {
            await this.Navigation.PopModalAsync();
        }

        private async void addMarkClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddMarkPage(this.subject, this.studentBook));
        }

        private async void OnSwiped(object sender, SwipedEventArgs e)
        {
            BoxView boxView = (BoxView)sender;
            ViewCell viewCell = (ViewCell)boxView.Parent.Parent;
            MarkListViewItem markListViewItem = (MarkListViewItem)viewCell.BindingContext;
            await this.studentBook.DeleteMark(new Mark() { MarkId = markListViewItem.MarkId, SubjectId = markListViewItem.SubjectId });
        }
    }
}