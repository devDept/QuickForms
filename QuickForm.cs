using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickForms
{
    public class Parameter<T>
    {
        public Action<T> Function;
        private T _value;

        public Parameter(T value, Action<T> function = null)
        {
            _value = value;
            Function = function ?? (val => {});
        }

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Function(_value);
            }
        }
    }

    public class QuickForm : Form
    {
        private Panel _mainPanel;

        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private readonly IContainer components = null;

        public QuickForm()
        {
            InitializeComponent();
        }

        public void Button(string text, Action function)
        {
            Button button = new Button();
            button.Text = text;

            button.Click += (ob, ea) =>
            {
                BeginInvoke(function);
            };

            AddSingleControl(button);
        }

        public Parameter<bool> CheckBox(string label, Action<bool> function = null)
        {
            CheckBox checkBox = new CheckBox();

            checkBox.Text = "";
            checkBox.UseVisualStyleBackColor = true;

            Parameter<bool> param = new Parameter<bool>(checkBox.Checked, function);

            checkBox.CheckedChanged += (ob, ea) => { param.Value = checkBox.Checked; };

            AddControl(label, checkBox, out _);

            return param;
        }

        public Parameter<string> TextBox(string label, Action<string> function = null)
        {
            Parameter<string> param = new Parameter<string>("", function);

            TextBox textbox = new TextBox();

            textbox.AllowDrop = true;
            textbox.LostFocus += (ob, ea) => param.Value = textbox.Text;

            AddControl(label, textbox, out _);

            return param;
        }

        // Es.
        // step = 0.001 => print 4 decimal digits
        // step = 0.06  => print 3 decimal digits
        private int DecimalsToPrint(double step) => (int)Math.Max(0, Math.Ceiling(-Math.Log10(step)) + 1);

        public Parameter<double> TrackBar(string label, double min, double max, double step, Action<double> function = null)
        {
            Parameter<double> param = new Parameter<double>(min, function);

            TrackBar trackBar = new TrackBar();

            trackBar.Text = label;
            trackBar.Minimum = 0;
            trackBar.Maximum = (int) Math.Ceiling((max - min) / step);
            trackBar.TickFrequency = 1;

            AddControl(label, trackBar, out Label labelControl);

            trackBar.ValueChanged += (ob, ea) =>
            {
                // don't go above max val
                param.Value = Math.Min(max, min + trackBar.Value * step);

                // Print the current value.
                // Number of decimal digits proportional to the step param.
                labelControl.Text = label + $@" [{param.Value.ToString("0." + new string('0', DecimalsToPrint(step)))}]";
            };

            return param;
        }

        public Parameter<T> ComboBox<T>(string label, IEnumerable<T> values, Action<T> function = null)
        {
            Parameter<T> param = new Parameter<T>(values.First(), function);

            ComboBox comboBox = new ComboBox();

            comboBox.Text = label;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            AddControl(label, comboBox, out Label _);

            foreach (T val in values)
                comboBox.Items.Add(val);

            comboBox.SelectedIndexChanged += (ob, ea) => param.Value = (T) comboBox.SelectedItem;

            return param;
        }

        private void AddSingleControl(Control control)
        {
            Panel panel = new Panel();

            SuspendLayout();

            control.Dock = DockStyle.Fill;

            panel.Dock = DockStyle.Top;
            panel.Padding = new Padding(0, 0, 0, 10);
            panel.Height = 50;

            _mainPanel.Controls.Add(panel);
            _mainPanel.Controls.SetChildIndex(panel, 0);

            panel.Controls.Add(control);

            ResumeLayout(false);
        }

        private void AddControl(string labelText, Control control, out Label label)
        {
            Panel panel = new Panel();
            label = new Label();

            SuspendLayout();

            control.Dock = DockStyle.Fill;

            label.Width = 150;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Text = labelText;
            label.Dock = DockStyle.Left;

            panel.Dock = DockStyle.Top;
            panel.Padding = new Padding(0, 0, 0, 10);
            panel.Height = 30;

            _mainPanel.Controls.Add(panel);
            _mainPanel.Controls.SetChildIndex(panel, 0);

            panel.Controls.Add(control);
            panel.Controls.Add(label);

            ResumeLayout(false);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();

            base.Dispose(disposing);
        }
        
        private void InitializeComponent()
        {
            SuspendLayout();

            Name = "QuickForm";
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Size = new Size(400, 300);

            _mainPanel = new Panel();
            _mainPanel.Padding = new Padding(10);
            _mainPanel.Font = new Font("Calibri", 9F, FontStyle.Regular, GraphicsUnit.Point);
            _mainPanel.Dock = DockStyle.Fill;

            Controls.Add(_mainPanel);

            ResumeLayout(false);
        }
    }
}
