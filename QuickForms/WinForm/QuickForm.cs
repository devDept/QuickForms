using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QuickForms.Core;
using QuickForms.Properties;
using QuickForms.Wpf;

namespace QuickForms.WinForm
{
    public class QuickForm : Form, IQuickUI
    {
        private QuickControl _quickControl;

        public QuickForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            Name = "QuickForm";
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            
            // we need grow only if we want to resize the window by dragging
            // https://stackoverflow.com/questions/12287523/cant-resize-the-form-by-dragging-its-borders
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            MinimumSize = new Size(400, 0);
            Icon = Resources.Icon;

            QuickControl control = new QuickControl();
            control.Dock = DockStyle.Fill;
            control.Padding = 10;
            _quickControl = control;

            Controls.Add(control);

            ResumeLayout(true);
        }

        public new double Padding
        {
            get => _quickControl.Padding;
            set => _quickControl.Padding = value;
        }

        public Parameter<bool> CheckBox(string label, Action<bool>? function = null)
        {
            return _quickControl.CheckBox(label, function);
        }

        public Parameter<string> TextBox(string label, Action<string>? function = null)
        {
            return _quickControl.TextBox(label, function);
        }

        public Parameter<double> TrackBar(string label, double min, double max, double step, Action<double>? function = null)
        {
            return _quickControl.TrackBar(label, min, max, step, function);
        }

        public Parameter<T> ComboBox<T>(string label, IEnumerable<T> values, Action<T>? function = null)
        {
            return _quickControl.ComboBox(label, values, function);
        }

        public Parameter<T> RadioButtons<T>(IDictionary<T, string> values, Action<T>? function = null)
        {
            return _quickControl.RadioButtons(values, function);
        }

        public void Button(string text, Action function)
        {
            _quickControl.Button(text, function);
        }

        public IQuickUI Category(string? title = null)
        {
            return _quickControl.Category(title);
        }

        public IQuickUI[] Split(int columns = 2)
        {
            return _quickControl.Split(columns);
        }

        public IQuickUI Label(string text, double percentage = 0.3)
        {
            return _quickControl.Label(text, percentage);
        }

        public Parameter<Color> ColorPicker(Color color, Action<Color>? function)
        {
            return _quickControl.ColorPicker(color, function);
        }

        public void Clear()
        {
            _quickControl.Clear();
        }

        public void SetTheme(Themes theme)
        {
            _quickControl.SetTheme(theme);
        }
    }
}
