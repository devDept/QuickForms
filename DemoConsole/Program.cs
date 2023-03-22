using System.Drawing;
using System.Windows;
using QuickForms.Core;
using QuickForms.Wpf;

[STAThread]
static void App()
{
    Application app = new Application();

    QuickForm qf = new QuickForm();

    qf.Show();

    var a = qf.CheckBox("Hello");

    qf.CheckBox("Hello");

    a.Value = true;

    qf.Button("Hello!", () => { a.Value = !a.Value; });
    qf.Button("Hello!", () => { qf.Button("Hello bro!", () => {}); });

    qf.TrackBar("Hello", 1, 9, 1, d =>
    {
        Console.WriteLine(d);
    });

    var cb = qf.ComboBox("Label", new string[]
    {
        "Hello",
        "World"
    }, s =>
    {
        Console.WriteLine(s);
    });

    cb.Value = "Reeas";

    qf.TextBox("Hello", t =>
    {
        Console.WriteLine(t);
    });

    qf.ColorPicker(Color.Lime, (col) =>
    {
        Console.WriteLine(col.GetHue());
    });

    var qp = qf.Category("Cats & Dogs");

    qp.Button("Hola", () => {});

    qp = qp.Category("Nasty nested category with very long title");

    var copy = qp;

    qp.Button("Delete", () =>
    {
        copy.Clear();
    });

    qp = qp.Category();

    qp.CheckBox("Yes or no?");

    Themes theme = Themes.Dark;

    qf.Button("Change theme", () =>
    {
        Themes newTheme = theme == Themes.Dark ? Themes.Light : Themes.Dark;
        qf.ChangeTheme(newTheme);
        theme = newTheme;
    });

    app.Run(qf);
}

Thread thread = new Thread(App);
thread.SetApartmentState(ApartmentState.STA);
thread.Start();