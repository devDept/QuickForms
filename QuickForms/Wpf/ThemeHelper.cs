using System;
using System.Windows;

namespace QuickForms.Wpf
{
    internal static class ThemeHelper
    {
        public static ResourceDictionary GetThemeDictionary(Core.Themes theme)
        {
            string uri;

            if (theme == Core.Themes.Dark)
            {
                uri = "pack://application:,,,/QuickForms;component/Wpf/Themes/DarkTheme.xaml";
            }
            else if (theme == Core.Themes.Light)
            {
                uri = "pack://application:,,,/QuickForms;component/Wpf/Themes/LightTheme.xaml";
            }
            else
            {
                throw new ArgumentException("Theme not yet supported.");
            }

            return new ResourceDictionary { Source = new Uri(uri) };
        }

        public static ResourceDictionary GetGeneric(Core.Themes theme)
        {
            Uri generic = new Uri("pack://application:,,,/QuickForms;component/Wpf/Themes/Generic.xaml");

            var td = GetThemeDictionary(theme);

            var gd = new ResourceDictionary { Source = generic };
            gd.MergedDictionaries.Clear();
            gd.MergedDictionaries.Add(td);

            return gd;
        }
    }
}
