using QuickForms.Wpf;

namespace DemoWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            QuickForms.WinForm.QuickForm qf = new QuickForms.WinForm.QuickForm();
            qf.Show();

            var a = qf.CheckBox("Hello");

            qf.CheckBox("Hello");

            a.Value = true;

            qf.Button("Hello darkness", () => { a.Value = !a.Value; });
            qf.Button("Light", () =>
            {
                qf.SetTheme(Themes.Light);
            });
        }
    }
}