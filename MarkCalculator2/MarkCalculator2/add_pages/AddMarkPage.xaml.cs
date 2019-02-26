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
	public partial class AddMarkPage : ContentPage
	{
        private Subject subject;
        private StudentBook studentBook;

        public AddMarkPage(Subject subject, StudentBook studentBook)
        {
            InitializeComponent();
            this.subject = subject;
            this.studentBook = studentBook;
            navigationGrid.BackgroundColor = ThemeCollors.StringToColor(ThemeCollors.DefaultNavigationColor);
        }

        private bool newMarkValueCheck(float mark)
        {
            if (mark == 1f || mark == 1.5f || mark == 2f || mark == 2.5f || mark == 3f || mark == 3.5f || mark == 4f || mark == 4.5f || mark == 5f)
            {
                return true;
            }

            return false;
        }

        private bool newMarkWeightChect(float weight)
        {
            if (weight >= 1 && weight <= 100)
            {
                return true;
            }

            return false;
        }

        private async void newMarkButtonClicked(object sender, EventArgs e)
        {
            string stringNewMarkValue = newMarkValueEntry.Text;
            string stringNewMarkWeight = newMarkWeightEntry.Text;

            if (!string.IsNullOrEmpty(stringNewMarkValue) && !string.IsNullOrEmpty(stringNewMarkWeight))
            {
                float newMarkValue;
                int newMarkWeight;
                bool parse1 = float.TryParse(stringNewMarkValue, out newMarkValue);
                bool parse2 = int.TryParse(stringNewMarkWeight, out newMarkWeight);

                if (parse1 && parse2 && this.newMarkValueCheck(newMarkValue) && this.newMarkWeightChect(newMarkWeight))
                {
                    await this.studentBook.AddMark(new Mark() { Value = newMarkValue, Weight = newMarkWeight }, this.subject);
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