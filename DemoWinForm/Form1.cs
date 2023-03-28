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

            var a = qf.AddCheckBox("Hello");

            qf.AddCheckBox("Hello");

            a.Value = true;

            qf.AddButton("Hello darkness", () => { a.Value = !a.Value; });
            qf.AddButton("Light", () =>
            {
                qf.SetTheme(Themes.Light);
            });
        }
    }
}