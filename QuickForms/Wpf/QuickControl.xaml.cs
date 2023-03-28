using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using QuickForms.Core;

namespace QuickForms.Wpf
{
    [System.ComponentModel.ToolboxItem(false)]
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

        public QuickOptions Options
        {
            get => QuickUI.Options;
            set => QuickUI.Options = value;
        }

        public Parameter<bool> AddCheckBox(string? label = null, Action<bool>? function = null)
        {
            return QuickUI.AddCheckBox(label, function);
        }

        public Parameter<string> AddTextBox(string? label = null, Action<string>? function = null)
        {
            return QuickUI.AddTextBox(label, function);
        }

        public Parameter<double> AddTrackBar(string? label, double min, double max, double? step = null,
            Action<double>? function = null)
        {
            return QuickUI.AddTrackBar(label, min, max, step, function);
        }

        public Parameter<T> AddComboBox<T>(string? label, IEnumerable<T> values, Action<T>? function = null)
        {
            return QuickUI.AddComboBox(label, values, function);
        }
        
        public void AddButton(string? text, Action function)
        {
            QuickUI.AddButton(text, function);
        }

        public IQuickUI AddCategory(string? title = null)
        {
            return QuickUI.AddCategory(title);
        }
        
        public Parameter<Color> AddColorPicker(string? label = null, Color? color = null, Action<Color>? function = null)
        {
            return QuickUI.AddColorPicker(label, color, function);
        }

        public IQuickUI[] Split(int n = 2)
        {
            return QuickUI.Split(n);
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
