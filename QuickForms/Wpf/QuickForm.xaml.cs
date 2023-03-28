using System;
using System.Collections.Generic;
using System.Drawing;
using QuickForms.Core;

namespace QuickForms.Wpf
{
    public partial class QuickForm : IQuickUI
    {
        public QuickForm()
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

        public Parameter<Color> AddColorPicker(string? label, Color? color, Action<Color>? function)
        {
            return QuickUI.AddColorPicker(label, color, function);
        }

        public void Clear()
        {
            QuickUI.Clear();
        }

        public IQuickUI[] Split(int n = 2)
        {
            return QuickUI.Split(n);
        }

        public void SetTheme(Themes theme)
        {
            Resources = Theme.GetThemeDictionary(theme);

            QuickUI.SetTheme(theme);
        }
    }
}
