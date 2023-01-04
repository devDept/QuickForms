using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
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

        IQuickUI Category(string title = null);

        IQuickUI[] Split(int columns=2);

        IQuickUI Label(string text, double percentage = 0.3);
    }

    public class QuickPanel : RoundedPanel, IQuickUI
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
            
            trackBar.Minimum = 0;
            trackBar.Maximum = (int) Math.Ceiling((max - min) / step);
            trackBar.TickFrequency = 1;
            trackBar.BackColor = BackgroundColor;
            
            AddControl(label, trackBar, out Label labelControl);

            Parameter<double> param = new Parameter<double>(
                () => Math.Min(max, min + trackBar.Value * step),
                val => trackBar.Value = (int)((val - min) / step),
                function);

            trackBar.ValueChanged += (ob, ea) =>
            {
                param.Trigger();

                // labelControl is null if the specified label string is null
                if (labelControl != null)
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

            control.AutoSize = false;
            control.Height = height;
            control.Dock = DockStyle.Fill;

            panel.Controls.Add(control);

            AddPanel(panel, height, spacing);

            ResumeLayout();
        }

        private void AddControl(string labelText, Control control, out Label label, int height = SMALL_H)
        {
            if (labelText == null)
            {
                label = null;
                AddSingleControl(control, height);
                return;
            }

            Panel panel = new Panel();
            
            SuspendLayout();

            control.Dock = DockStyle.Top;
            control.AutoSize = false;
            control.Height = height;

            label = new Label
            {
                Text = labelText,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Left,
                Tag = new ControlLabel()
            };

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

        public IQuickUI Category(string title = null)
        {
            // for a nested panel we use by default a slightly
            // darker background than the current one
            // todo: we may go over 255
            const int darkenBy = 4;
            const int darkenBorderBy = darkenBy * 5;

            Color back = Color.FromArgb(BackgroundColor.R - darkenBy, BackgroundColor.G - darkenBy, BackgroundColor.B - darkenBy);

            Color border = Color.FromArgb(BackgroundColor.R - darkenBorderBy, BackgroundColor.G - darkenBorderBy, BackgroundColor.B - darkenBorderBy);
            
            QuickPanel panel = new QuickPanel
            {
                BackgroundColor = back,
                BorderColor = border,
                Padding = new Padding(SPACING),
                BorderThickness = 1,
                Title = title
            };

            SuspendLayout();

            Panel wrap = new Panel();

            wrap.Controls.Add(panel);
            wrap.AutoSize = true;

            AddPanel(wrap);

            ResumeLayout();

            return panel;
        }

        public IQuickUI[] Split(int columns=2)
        {
            QuickPanel[] quickUIs = new QuickPanel[columns];

            for (int i = 0; i < quickUIs.Length; i++)
            {
                quickUIs[i] = new QuickPanel
                {
                    BackgroundColor = Color.Transparent,
                    Padding = Padding.Empty,
                    BorderThickness = 0
                };
            }
            
            SuspendLayout();

            TableLayoutPanel wrap = new TableLayoutPanel();

            for (int i = 0; i < quickUIs.Length; i++)
            {
                wrap.Controls.Add(quickUIs[i], i, 0);
                wrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columns));
            }

            wrap.AutoSize = true;

            AddPanel(wrap);

            ResumeLayout();
            
            return quickUIs;
        }

        public IQuickUI Label(string text, double percentage = 0.3)
        {
            QuickPanel qp = new QuickPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = Padding.Empty
            };

            SuspendLayout();

            Label label = new Label
            {
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Left,
                Tag = new ControlLabel(percentage)
            };

            Panel wrap = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,

                // without this the whole wrap panel would collapse
                // if the quick panel does not contain any controls
                MinimumSize = new Size(0, label.Height)
            };

            Controls.Add(wrap);

            wrap.Controls.Add(qp);
            wrap.Controls.Add(label);

            AddPanel(wrap);

            ResumeLayout();

            return qp;
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
                    if (label.Tag is ControlLabel cont)
                        label.Width = (int) (Width * cont.Percentage);
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

        public IQuickUI Category(string title = null)
        {
            return _panel.Category(title);
        }

        public IQuickUI[] Split(int columns = 2)
        {
            return _panel.Split(columns);
        }

        public IQuickUI Label(string text, double percentage = 0.3)
        {
            return _panel.Label(text, percentage);
        }
    }

    // group of checkboxes
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

    // used to check whether a label should be resized, see QuickPanel.OnResize
    public class ControlLabel
    {
        public double Percentage { get; set; }

        public ControlLabel(double percentage = 0.3)
        {
            if(percentage >= 1 || percentage <= 0)
                throw new ArgumentException("Percentage must be between 0 and 1.");

            Percentage = percentage;
        }
    }

    /// <summary>
    /// Panel with customizable thickness, border color, and background color.
    /// Partially made by ChatGPT.
    /// </summary>
    public class RoundedPanel : Panel
    {
        private const int TITLE_DARKEN_FAC = 3;

        // background, border
        private Color _backgroundColor = Color.FromArgb(240, 240, 240);
        private Color _borderColor = Color.FromArgb(230, 230, 230);
        private int _borderThickness;

        // padding
        private Padding _padding = new Padding(10);

        // border thickness
        
        // title
        private string _title;
        private int _titleSize = 30;

        // corner radius
        // todo: not working (outer border is too thick due to anti aliasing)
        private int _cornerRadius;

        public new Padding Padding
        {
            get => _padding;
            set
            {
                // we store the user-provided padding
                _padding = value;

                Padding copy = _padding;

                if (_title != null) copy.Top += _titleSize;

                // we set the actual padding after adding the title size
                base.Padding = copy;
            }
        }

        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        public int BorderThickness
        {
            get { return _borderThickness; }
            set
            {
                _borderThickness = value;
                Invalidate();
            }
        }

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                Invalidate();
            }
        }

        public int CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                _cornerRadius = value;
                Invalidate();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                Padding = _padding; // update padding
                Invalidate();
            }
        }

        public int TitleSize
        {
            get => _titleSize;
            set
            {
                _titleSize = value;
                Padding = _padding; // update padding
                Invalidate();
            }
        }

        public RoundedPanel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            DoubleBuffered = true;
            BackColor = Color.Transparent; // we use BackgroundColor instead
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            Rectangle clip = ClientRectangle;

            // client rectangle is one pixel bigger than expected
            // https://stackoverflow.com/questions/5280838/one-pixel-out-onpaint-systemdrawingrectangle-rectangle-this-clientrec
            clip.Width -= 1;
            clip.Height -= 1;

            // draw the background
            using (SolidBrush brush = new SolidBrush(_backgroundColor))
            {
                g.FillPath(brush, RoundedRectangle.Create(clip, _cornerRadius));
            }

            // draw the title if present
            if (Title != null)
            {
                Color titleColor = Color.FromArgb(_backgroundColor.R - TITLE_DARKEN_FAC, _backgroundColor.G - TITLE_DARKEN_FAC, _backgroundColor.B - TITLE_DARKEN_FAC);

                using (SolidBrush brush = new SolidBrush(titleColor))
                {
                    Rectangle titleRect = new Rectangle(clip.Left, clip.Top, clip.Width, _titleSize);
                    g.FillPath(brush, RoundedRectangle.Create(titleRect, new Padding(_cornerRadius, 0, _cornerRadius, 0)));
                }

                using (SolidBrush brush = new SolidBrush(ForeColor))
                {
                    StringFormat format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

                    Font titleFont = new Font(Font.FontFamily, Font.Size * 1.1f, FontStyle.Regular);

                    g.DrawString(Title, titleFont, brush, clip.Left + clip.Width / 2, clip.Top + _titleSize / 2, format);
                }

                using (Pen pen = new Pen(_borderColor, _borderThickness))
                {
                    g.DrawLine(pen, clip.Left + BorderThickness, clip.Top + _titleSize, clip.Right - BorderThickness, clip.Top + _titleSize);
                }
            }

            if (BorderThickness > 0)
            {
                // daw the border
                using (Pen pen = new Pen(_borderColor, _borderThickness))
                {
                    if (_cornerRadius == 0)
                    {
                        // this is not affected by anti aliasing
                        g.DrawRectangle(pen, clip);
                    }
                    else
                    {
                        g.DrawPath(pen, RoundedRectangle.Create(clip, _cornerRadius, _borderThickness / 2));
                    }
                }
            }

            g.Flush();
        }
    }

    public static class RoundedRectangle
    {
        /// <summary>
        /// Create a rounded rectangle path.
        /// </summary>
        public static GraphicsPath Create(Rectangle rectangle, Padding radius, int offset = 0)
        {
            GraphicsPath path = new GraphicsPath();

            int
                l = rectangle.Left + offset,
                t = rectangle.Top + offset,
                w = rectangle.Width - offset * 2,
                h = rectangle.Height - offset * 2;

            if (radius.Left > 0) path.AddArc(l, t, radius.Left * 2, radius.Left * 2, 180, 90); // topleft

            path.AddLine(l + radius.Left, t, l + w - radius.Right, t); // top

            if (radius.Right > 0) path.AddArc(l + w - radius.Right * 2, t, radius.Right * 2, radius.Right * 2, 270, 90); // topright

            path.AddLine(l + w, t + radius.Right, l + w, t + h - radius.Bottom); // right

            if (radius.Bottom > 0) path.AddArc(l + w - radius.Bottom * 2, t + h - radius.Bottom * 2, radius.Bottom * 2, radius.Bottom * 2, 0, 90); // bottomright

            path.AddLine(l + w - radius.Bottom, t + h, l + radius.Top, t + h); // bottom

            if (radius.Top > 0) path.AddArc(l, t + h - l + radius.Top * 2, radius.Top * 2, radius.Top * 2, 90, 90); // bottomleft

            path.AddLine(l, t + h - radius.Top * 2, l, t + radius.Left); // left

            path.CloseFigure();

            return path;
        }

        public static GraphicsPath Create(Rectangle rectangle, int radius, int offset = 0)
            => Create(rectangle, new Padding(radius), offset);
    }
}