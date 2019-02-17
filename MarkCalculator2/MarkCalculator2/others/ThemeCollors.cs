using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MarkCalculator2
{
    public class ThemeCollors
    {
        public static string NavigationColor = "#1171d7";

        public static Color StringToColor(string stringColor)
        {
            ColorTypeConverter converter = new ColorTypeConverter();
            Color color = (Color)converter.ConvertFromInvariantString(stringColor);
            return color;
        }
    }
}
