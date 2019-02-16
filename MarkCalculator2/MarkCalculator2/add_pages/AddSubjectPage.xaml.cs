using Entities;
using Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarkCalculator2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddSubjectPage : ContentPage
	{
        private StudentBook studentBook;

        public AddSubjectPage(StudentBook studentBook)
		{
			InitializeComponent();
            this.studentBook = studentBook;
            this.setTheme();
        }

        private void setTheme()
        {
            navigationGrid.BackgroundColor = ThemeCollors.StringToColor(ThemeCollors.NavigationColor);
        }

        private string normalizeName(string name)
        {
            return name.First().ToString().ToUpper() + name.Substring(1).ToLower();
        }

        private async Task<bool> newSubjectNameCheck(string name)
        {
            bool subjectExists = await this.studentBook.SubjectNameExists(name);

            if (!subjectExists && name.Length <= 28)
            {
                return true;
            }

            return false;
        }

        private async void addSubjectButtonClicked(object sender, EventArgs e)
        {
            string newSubjectName = this.normalizeName(newSubjectNameEntry.Text);
            bool checkResult = await this.newSubjectNameCheck(newSubjectName);

            if (checkResult)
            {
                await this.studentBook.AddSubject(new Subject() { Name = newSubjectName });
                await this.Navigation.PopModalAsync();
            }
        }

        private async void backClicked(object sender, EventArgs e)
        {
            await this.Navigation.PopModalAsync();
        }
    }
}