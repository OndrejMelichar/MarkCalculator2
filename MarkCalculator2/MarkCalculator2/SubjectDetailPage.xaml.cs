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
	public partial class SubjectDetailPage : ContentPage
	{
		public SubjectDetailPage ()
		{
			InitializeComponent ();

            navigationCenter.Text = "předmět";

        }

        private void backClicked(object sender, EventArgs e)
        {

        }

        private void addMarkClicked(object sender, EventArgs e)
        {

        }

        private void deleteMarkClicked(object sender, EventArgs e)
        {

        }
    }
}