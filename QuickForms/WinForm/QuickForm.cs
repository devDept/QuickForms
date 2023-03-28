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

        public QuickOptions Options
        {
            get => _quickControl.Options;
            set => _quickControl.Options = value;
        }

        public Parameter<bool> AddCheckBox(string? label = null, Action<bool>? function = null)
        {
            return _quickControl.AddCheckBox(label, function);
        }

        public Parameter<string> AddTextBox(string? label = null, Action<string>? function = null)
        {
            return _quickControl.AddTextBox(label, function);
        }

        public Parameter<double> AddTrackBar(string? label, double min, double max, double? step = null,
            Action<double>? function = null)
        {
            return _quickControl.AddTrackBar(label, min, max, step, function);
        }

        public Parameter<T> AddComboBox<T>(string? label, IEnumerable<T> values, Action<T>? function = null)
        {
            return _quickControl.AddComboBox(label, values, function);
        }
        
        public void AddButton(string? text, Action function)
        {
            _quickControl.AddButton(text, function);
        }

        public IQuickUI AddCategory(string? title = null)
        {
            return _quickControl.AddCategory(title);
        }
        
        public Parameter<Color> AddColorPicker(string? label, Color? color, Action<Color>? function)
        {
            return _quickControl.AddColorPicker(label, color, function);
        }

        public IQuickUI[] Split(int n = 2)
        {
            return _quickControl.Split(n);
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
