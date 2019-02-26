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
            navigationGrid.BackgroundColor = ThemeCollors.StringToColor(ThemeCollors.DefaultNavigationColor);
        }

        private async Task<bool> newSubjectNameCheck(string name)
        {
            bool subjectExists = await this.studentBook.SubjectNameExists(name);

            if (!subjectExists && name.Length != 0 && name.Length <= 28)
            {
                return true;
            }

            return false;
        }

        private async void addSubjectButtonClicked(object sender, EventArgs e)
        {
            string newSubjectName = newSubjectNameEntry.Text;

            if (!string.IsNullOrEmpty(newSubjectName) && !string.IsNullOrEmpty(newSubjectName.Trim()))
            {
                newSubjectName = newSubjectName.Trim();
                newSubjectName = this.studentBook.NormalizeSubjectName(newSubjectName);
                bool checkResult = await this.newSubjectNameCheck(newSubjectName);

                if (checkResult)
                {
                    await this.studentBook.AddSubject(new Subject() { Name = newSubjectName });
                    await this.Navigation.PopModalAsync();
                }
            }
        }

        private async void backClicked(object sender, EventArgs e)
        {
            await this.Navigation.PopModalAsync();
        }
    }
}