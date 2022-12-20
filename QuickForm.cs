using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuickForms
{
    /// <summary>
    /// Represents a property of a control.
    /// </summary>
    /// <typeparam name="T">Type of the parameter.</typeparam>
    public class Parameter<T>
    {
        /// <summary>
        /// Invoked after the value was changed via the GUI control.
        /// </summary>
        public Action<T> Function { get; set; }

        /// <summary>
        /// Sets the property in the GUI control.
        /// </summary>
        private readonly Action<T> _setter;

        /// <summary>
        /// Gets the property in the GUI control.
        /// </summary>
        private readonly Func<T> _getter;

        public Parameter(Func<T> getter, Action<T> setter, Action<T> function = null)
        {
            _getter = getter;
            _setter = setter;

            Function = function ?? (val => { });
        }

        /// <summary>
        /// Gets or sets the current value. Setting the value will update the GUI control
        /// but it will not trigger the callback <see cref="Function"/>.
        /// </summary>
        public T Value
        {
            get => _getter();
            set => _setter(value);
        }

        /// <summary>
        /// Triggers the user callback.
        /// </summary>
        internal void Trigger() => Function(Value);
    }

    /// <summary>
    /// Interface for quick user interfaces.
    /// </summary>
    public interface IQuickUI
    {
        Parameter<bool> CheckBox(string label, Action<bool> function = null);

        Parameter<string> TextBox(string label, Action<string> function = null);

        Parameter<double> TrackBar(string label, double min, double max, double step, Action<double> function = null);

        Parameter<T> ComboBox<T>(string label, IEnumerable<T> values, Action<T> function = null);

        Parameter<T> RadioButtons<T>(IDictionary<T, string> values, Action<T> function = null);

        void Button(string text, Action function);

        IQuickUI Category();
    }

    public class QuickPanel : Panel, IQuickUI
    {
        /// <summary>
        /// Default height for controls with labels (e.g., track bars, checkboxes, etc.).
        /// </summary>
        private const int SMALL_H = 20;

        /// <summary>
        /// Default height for buttons in a group (e.g., radio buttons).
        /// </summary>
        private const int MEDIUM_H = 30;

        /// <summary>
        /// Default height for single buttons.
        /// </summary>
        private const int LARGE_H = 40;

        /// <summary>
        /// Vertical spacing between different controls.
        /// </summary>
        private const int SPACING = 10;

        public QuickPanel()
        {
            InitializeComponent();
        }

        public void Button(string text, Action function)
        {
            Button button = new Button();
            button.BackColor = Color.White;
            button.Text = text;

            button.Click += (ob, ea) => { BeginInvoke(function); };

            AddSingleControl(button);
        }

        public Parameter<bool> CheckBox(string label, Action<bool> function = null)
        {
            CheckBox checkBox = new CheckBox();

            checkBox.Text = "";
            checkBox.UseVisualStyleBackColor = true;

            Parameter<bool> param = new Parameter<bool>(
                () => checkBox.Checked,
                val => checkBox.Checked = val,
                function
            );

            checkBox.CheckedChanged += (ob, ea) => param.Trigger();

            AddControl(label, checkBox, out _);

            return param;
        }

        public Parameter<string> TextBox(string label, Action<string> function = null)
        {
            TextBox textbox = new TextBox();

            Parameter<string> param = new Parameter<string>(
                () => textbox.Text,
                text => textbox.Text = text,
                function
            );

            textbox.AllowDrop = true;
            textbox.LostFocus += (ob, ea) => param.Trigger();

            AddControl(label, textbox, out _);

            return param;
        }

        // Es.
        // step = 0.001 => print 4 decimal digits
        // step = 0.06  => print 3 decimal digits
        private int DecimalsToPrint(double step) => (int)Math.Max(0, Math.Ceiling(-Math.Log10(step)) + 1);

        public Parameter<double> TrackBar(string label, double min, double max, double step, Action<double> function = null)
        {
            TrackBar trackBar = new TrackBar();

            trackBar.Text = label;
            trackBar.Minimum = 0;
            trackBar.Maximum = (int)Math.Ceiling((max - min) / step);
            trackBar.TickFrequency = 1;

            AddControl(label, trackBar, out Label labelControl);

            Parameter<double> param = new Parameter<double>(
                () => Math.Min(max, min + trackBar.Value * step),
                val => trackBar.Value = (int)((val - min) / step),
                function);

            trackBar.ValueChanged += (ob, ea) =>
            {
                param.Trigger();

                // Print the current value.
                // Number of decimal digits proportional to the step param.
                labelControl.Text = label + $@" [{param.Value.ToString("0." + new string('0', DecimalsToPrint(step)))}]";
            };

            return param;
        }

        public Parameter<T> ComboBox<T>(string label, IEnumerable<T> values, Action<T> function = null)
        {
            ComboBox comboBox = new ComboBox();

            comboBox.Text = label;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            // we specify the height because the combobox is not able to resize
            // vertically, it can only be resized by changing the font size
            AddControl(label, comboBox, out Label _, comboBox.Height + 1);

            foreach (T val in values)
                comboBox.Items.Add(val);

            Parameter<T> param = new Parameter<T>(
                () => (T)comboBox.SelectedItem,
                val => comboBox.SelectedItem = val,
                function
            );

            comboBox.SelectedIndexChanged += (ob, ea) => param.Trigger();

            return param;
        }

        public Parameter<T> RadioButtons<T>(IDictionary<T, string> dict, Action<T> function = null)
        {
            CheckBoxGroup<T> cbg = new CheckBoxGroup<T>(dict);

            Parameter<T> param = new Parameter<T>(cbg.GetValue, cbg.SetValue, function);

            for (var i = 0; i < cbg.CheckBoxes.Length; i++)
            {
                var btn = cbg.CheckBoxes[i];
                btn.Appearance = Appearance.Button;
                btn.BackColor = Color.White;
                AddSingleControl(btn, MEDIUM_H, SPACING / (i > 0 ? 3 : 1));
            }

            cbg.OnChange = function;

            return param;
        }

        private void AddSingleControl(Control control, int height = LARGE_H, int spacing = SPACING)
        {
            Panel panel = new Panel();

            SuspendLayout();

            control.Dock = DockStyle.Fill;

            panel.Controls.Add(control);

            AddPanel(panel, height, spacing);

            ResumeLayout();
        }

        private void AddControl(string labelText, Control control, out Label label, int height = SMALL_H)
        {
            Panel panel = new Panel();

            label = new Label();

            SuspendLayout();

            control.Dock = DockStyle.Fill;
            control.AutoSize = false;

            label.Width = 150;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Text = labelText;
            label.Dock = DockStyle.Left;
            label.Tag = LabelType.ControlLabel;

            AddPanel(panel, height);

            panel.Controls.Add(control);
            panel.Controls.Add(label);

            ResumeLayout();
        }

        private void AddPanel(Panel panel, int? height = null, int spacing = SPACING)
        {
            Padding padding = Controls.Count > 0 ? new Padding(0, spacing, 0, 0) : Padding.Empty;

            panel.Dock = DockStyle.Top;
            panel.Padding = padding;

            if (height.HasValue)
                panel.Height = height.Value + padding.Top;

            Controls.Add(panel);
            Controls.SetChildIndex(panel, 0);
        }

        public IQuickUI Category()
        {
            // for a nested panel we use by default a slightly
            // darker background than the current one
            // todo: we may go over 255
            const int darkenBy = 5;

            QuickPanel panel = new QuickPanel
            {
                BackColor = Color.FromArgb(BackColor.R - darkenBy, BackColor.G - darkenBy, BackColor.B - darkenBy),
                Padding = new Padding(10)
            };

            SuspendLayout();

            Panel wrap = new Panel();

            wrap.Controls.Add(panel);
            wrap.AutoSize = true;
            wrap.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            AddPanel(wrap);

            ResumeLayout();

            return panel;
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            Font = new Font("Calibri", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            ResumeLayout();
        }

        protected override void OnResize(EventArgs eventargs)
        {
            // we resize the labels so that they always take up one third of the width

            foreach (Panel panel in Controls.OfType<Panel>())
            {
                foreach (Label label in panel.Controls.OfType<Label>())
                {
                    if (label.Tag is LabelType labelType && labelType == LabelType.ControlLabel)
                        label.Width = Width / 3;
                }
            }

            base.OnResize(eventargs);
        }
    }

    public class QuickForm : Form, IQuickUI
    {
        private QuickPanel _panel;

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

            _panel = new QuickPanel();
            _panel.Padding = new Padding(10);
            _panel.Dock = DockStyle.Fill;
            _panel.AutoSize = true;
            _panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            Controls.Add(_panel);

            ResumeLayout(true);
        }

        public Parameter<bool> CheckBox(string label, Action<bool> function = null)
        {
            return _panel.CheckBox(label, function);
        }

        public Parameter<string> TextBox(string label, Action<string> function = null)
        {
            return _panel.TextBox(label, function);
        }

        public Parameter<double> TrackBar(string label, double min, double max, double step, Action<double> function = null)
        {
            return _panel.TrackBar(label, min, max, step, function);
        }

        public Parameter<T> ComboBox<T>(string label, IEnumerable<T> values, Action<T> function = null)
        {
            return _panel.ComboBox(label, values, function);
        }

        public Parameter<T> RadioButtons<T>(IDictionary<T, string> values, Action<T> function = null)
        {
            return _panel.RadioButtons(values, function);
        }

        public void Button(string text, Action function)
        {
            _panel.Button(text, function);
        }

        public IQuickUI Category()
        {
            return _panel.Category();
        }
    }

    internal class CheckBoxGroup<T>
    {
        public CheckBox[] CheckBoxes;

        public Action<T> OnChange;

        private T _value;

        public CheckBoxGroup(IDictionary<T, string> dict)
        {
            CheckBoxes = new CheckBox[dict.Count];

            int i = 0;

            foreach (var entry in dict)
            {
                CheckBox cb = new CheckBox { Text = entry.Value, Tag = entry.Key };
                cb.CheckedChanged += OnCheck;
                CheckBoxes[i++] = cb;
            }
        }

        public void OnCheck(object ob, EventArgs ea)
        {
            CheckBox cb = (CheckBox)ob;

            if (cb.Checked) SetValue((T)cb.Tag);
            else SetValue(default);

            OnChange?.Invoke(_value);
        }

        public T GetValue() => _value;

        public void SetValue(T value)
        {
            foreach (CheckBox cb in CheckBoxes)
            {
                // we remove the event listener to prevent OnCheck from
                // being invoked multiple times
                cb.CheckedChanged -= OnCheck;
                cb.Checked = cb.Tag is T t && t.Equals(value);
                cb.CheckedChanged += OnCheck;
            }

            _value = value;
        }
    }

    // used to check
    internal enum LabelType
    {
        ControlLabel
    }
}