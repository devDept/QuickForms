using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using QuickForms.Core;

namespace QuickForms.Wpf
{
    public partial class QuickControl : IQuickUI
    {
        public QuickControl()
        {
            InitializeComponent();
        }

        public new double Padding
        {
            get => QuickUI.Padding;
            set => QuickUI.Padding = value;
        }

        public Parameter<bool> CheckBox(string label, Action<bool>? function = null)
        {
            return QuickUI.CheckBox(label, function);
        }

        public Parameter<string> TextBox(string label, Action<string>? function = null)
        {
            return QuickUI.TextBox(label, function);
        }

        public Parameter<double> TrackBar(string label, double min, double max, double? step = null, Action<double>? function = null)
        {
            return QuickUI.TrackBar(label, min, max, step, function);
        }

        public Parameter<T> ComboBox<T>(string label, IEnumerable<T> values, Action<T>? function = null)
        {
            return QuickUI.ComboBox(label, values, function);
        }

        public Parameter<T> RadioButtons<T>(IDictionary<T, string> values, Action<T>? function = null)
        {
            return QuickUI.RadioButtons(values, function);
        }

        public void Button(string text, Action function)
        {
            QuickUI.Button(text, function);
        }

        public IQuickUI Category(string? title = null)
        {
            return QuickUI.Category(title);
        }

        public IQuickUI[] Split(int columns = 2)
        {
            return QuickUI.Split(columns);
        }

        public IQuickUI Label(string text, double percentage = 0.3)
        {
            return QuickUI.Label(text, percentage);
        }

        public Parameter<Color> ColorPicker(Color color, Action<Color>? function)
        {
            return QuickUI.ColorPicker(color, function);
        }

        public void Clear()
        {
            QuickUI.Clear();
        }
        
        public void SetTheme(Themes theme)
        {
            Resources = Theme.GetGeneric(theme);
        }
    }

    internal static class Theme
    {
        public static ResourceDictionary GetThemeDictionary(Themes theme)
        {
            string uri;

            if (theme == Themes.Dark)
            {
                uri = "pack://application:,,,/QuickForms;component/Wpf/Themes/DarkTheme.xaml";
            }
            else if (theme == Themes.Light)
            {
                uri = "pack://application:,,,/QuickForms;component/Wpf/Themes/LightTheme.xaml";
            }
            else
            {
                throw new ArgumentException("Theme not yet supported.");
            }

            return new ResourceDictionary { Source = new Uri(uri) };
        }

        public static ResourceDictionary GetGeneric(Themes theme)
        {
            Uri generic = new Uri("pack://application:,,,/QuickForms;component/Wpf/Themes/Generic.xaml");

            var td = GetThemeDictionary(theme);

            var gd = new ResourceDictionary { Source = generic };
            gd.MergedDictionaries.Clear();
            gd.MergedDictionaries.Add(td);

            return gd;
        }
    }

    public enum Themes
    {
        Dark,
        Light
    }
}
