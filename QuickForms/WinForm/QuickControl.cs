using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.Integration;
using QuickForms.Core;
using QuickForms.Wpf;

namespace QuickForms.WinForm
{
    public class QuickControl : ElementHost, IQuickUI
    {
        private readonly Wpf.QuickControl _quickControlWpf;

        public QuickControl()
        {
            _quickControlWpf = new Wpf.QuickControl();

            Child = _quickControlWpf;
        }

        public new double Padding
        {
            get => _quickControlWpf.Padding;
            set => _quickControlWpf.Padding = value;
        }

        public Parameter<bool> CheckBox(string label, Action<bool>? function = null)
        {
            return _quickControlWpf.CheckBox(label, function);
        }

        public Parameter<string> TextBox(string label, Action<string>? function = null)
        {
            return _quickControlWpf.TextBox(label, function);
        }

        public Parameter<double> TrackBar(string label, double min, double max, double? step = null, Action<double>? function = null)
        {
            return _quickControlWpf.TrackBar(label, min, max, step, function);
        }

        public Parameter<T> ComboBox<T>(string label, IEnumerable<T> values, Action<T>? function = null)
        {
            return _quickControlWpf.ComboBox(label, values, function);
        }

        public Parameter<T> RadioButtons<T>(IDictionary<T, string> values, Action<T>? function = null)
        {
            return _quickControlWpf.RadioButtons(values, function);
        }

        public void Button(string text, Action function)
        {
            _quickControlWpf.Button(text, function);
        }

        public IQuickUI Category(string? title = null)
        {
            return _quickControlWpf.Category(title);
        }

        public IQuickUI[] Split(int columns = 2)
        {
            return _quickControlWpf.Split(columns);
        }

        public IQuickUI Label(string text, double percentage = 0.3)
        {
            return _quickControlWpf.Label(text, percentage);
        }

        public Parameter<Color> ColorPicker(Color color, Action<Color>? function)
        {
            return _quickControlWpf.ColorPicker(color, function);
        }

        public void Clear()
        {
            _quickControlWpf.Clear();
        }

        public void SetTheme(Themes theme)
        {
            _quickControlWpf.SetTheme(theme);
        }
    }
}
