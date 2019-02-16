using Entities;
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
            this.setTheme();

            Task.Run(async () => {
                await this.studentBook.AddSubject(new Subject() { Name = "Matematika" });
                await this.studentBook.AddSubject(new Subject() { Name = "Český jazyk" });
                await this.studentBook.AddSubject(new Subject() { Name = "Základy společenských věd" });
                await this.displaySubjects();
                await this.studentBook.AddSubject(new Subject() { Name = "Tělesná výchova" });
            }).Wait();
        }

        private void setTheme()
        {
            navigationGrid.BackgroundColor = ThemeCollors.StringToColor(ThemeCollors.NavigationColor);
        }

        private async Task displaySubjects()
        {
            List<Subject> subjects = await this.studentBook.GetSubjects();
            StudentBook.SubjectsObservable = await this.subjectsToListViewCollection(subjects);
            subjectsListView.ItemsSource = StudentBook.SubjectsObservable;
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

        /*private async void subjectTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushModalAsync(new SubjectDetailPage());
        }*/

        private async void subjectTapped(object sender, EventArgs e)
        {
            Grid grid = ((ViewCell)sender).FindByName<Grid>("ViewCellGrid");
            Label label1 = grid.FindByName<Label>("ViewCellLabel");
            grid.BackgroundColor = Color.White;
            label1.TextDecorations = TextDecorations.Underline;
            await Navigation.PushModalAsync(new SubjectDetailPage());
        }

        private async void addSubjectClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddSubjectPage(this.studentBook));
        }
    }
}
