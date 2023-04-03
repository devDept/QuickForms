using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using QuickForms.Core;
using QuickForms.Wpf;
using Color = System.Drawing.Color;
using Size = System.Drawing.Size;

namespace QuickForms.WinForm
{
    public class QuickForm : ResizeableForm, IQuickUI
    {
        private QuickControl _quickControl;
        private WindowTitleBar _titleBar;

        private Panel _wrapper;

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
            MinimumSize = new Size(400, 0);

            // we need this padding or the user will not
            // be able to resize the form
            base.Padding = new Padding(1);
            
            ElementHost titleBar = new ElementHost();
            WindowTitleBar wtb = new WindowTitleBar();

            _titleBar = wtb;

            // close window
            wtb.Close += (_, _) => Close();

            // minimize window
            wtb.Minimize += (_, _) => WindowState = FormWindowState.Minimized;

            // maximize window
            wtb.Maximize += (_, _) =>
            {
                WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
            };

            // drag window
            wtb.MouseDown += (ob, ea) =>
            {
                if (ea.ChangedButton == MouseButton.Left)
                    Drag();
            };
            
            titleBar.Child = wtb;
            titleBar.Dock = DockStyle.Top;
            titleBar.Height = 29;

            QuickControl control = new QuickControl();
            control.Dock = DockStyle.Fill;
            control.Padding = 10;
            _quickControl = control;

            // this panel creates the inner 1-pixel border
            _wrapper = new Panel();
            _wrapper.Padding = new Padding(1);
            _wrapper.Dock = DockStyle.Fill;

            _wrapper.Controls.Add(control);
            _wrapper.Controls.Add(titleBar);

            Controls.Add(_wrapper);

            SetBorderColor();

            ResumeLayout(true);
        }

        protected override void OnResizeBegin(EventArgs e)
        {
            // during resize we suspend the layout to prevent the
            // wpf controls from blinking
            SuspendLayout();
            base.OnResizeBegin(e);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            ResumeLayout();
            base.OnResizeEnd(e);
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
            _titleBar.SetTheme(theme);

            SetBorderColor();
        }

        // sets the colors of the inner and outer border
        private void SetBorderColor()
        {
            // get the window's darker background color
            var c = ((SolidColorBrush)_titleBar.Background).Color;
            var backColor = Color.FromArgb(c.R, c.G, c.B);

            // this sets the inner border
            _wrapper.BackColor = backColor;

            int th = 127;

            // this will yield a darker or lighter color
            // depending on the theme
            Color color = Color.FromArgb(
                Closer(backColor.R, th),
                Closer(backColor.G, th),
                Closer(backColor.B, th)
            );

            // this sets the outer border
            BackColor = color;
        }

        // returns an integer closer to the threshold by the 
        // given factor with respect to the original value
        private int Closer(int val, int th, double fac = 2.0)
        {
            int d = val - th;
            double newD = Math.Sign(d) * Math.Abs(val - th) / fac;
            return th + (int) Math.Round(newD);
        }
    }

    // resizeable and draggable form
    public class ResizeableForm : Form
    {
        public static int Gap = 2;

        public ResizeableForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private const int
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17;

        Rectangle Top => new(0, 0, ClientSize.Width, Gap);
        Rectangle Left => new(0, 0, Gap, ClientSize.Height);
        Rectangle Bottom => new(0, ClientSize.Height - Gap, ClientSize.Width, Gap);
        Rectangle Right => new(ClientSize.Width - Gap, 0, Gap, ClientSize.Height);

        Rectangle TopLeft => new(0, 0, Gap, Gap);
        Rectangle TopRight => new(ClientSize.Width - Gap, 0, Gap, Gap);
        Rectangle BottomLeft => new(0, ClientSize.Height - Gap, Gap, Gap);
        Rectangle BottomRight => new(ClientSize.Width - Gap, ClientSize.Height - Gap, Gap, Gap);

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x84) // WM_NCHITTEST
            {
                var cursor = PointToClient(System.Windows.Forms.Cursor.Position);

                if (TopLeft.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeft.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;

                else if (Top.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                else if (Left.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (Right.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (Bottom.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
            }
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        protected void Drag()
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
    }
}
