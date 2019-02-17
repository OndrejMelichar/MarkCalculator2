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
            navigationGrid.BackgroundColor = ThemeCollors.StringToColor(ThemeCollors.NavigationColor);
            subjectsListView.ItemsSource = StudentBook.SubjectsObservable;
        }

        private async void subjectTapped(object sender, EventArgs e)
        {
            ViewCell viewCell = ((ViewCell)sender);
            Grid grid = viewCell.FindByName<Grid>("ViewCellGrid");
            SubjectListViewItem subjectListViewItem = (SubjectListViewItem)viewCell.BindingContext;
            grid.BackgroundColor = Color.White;
            Subject subject = new Subject() { Name = subjectListViewItem.SubjectName, SubjectId = subjectListViewItem.SubjectId };
            await Navigation.PushModalAsync(new SubjectDetailPage(subject, this.studentBook));
        }

        private async void addSubjectClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddSubjectPage(this.studentBook));
        }
    }
}
