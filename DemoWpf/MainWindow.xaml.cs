using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuickForms.Wpf;

namespace DemoWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            QuickForm qf = new QuickForm();
            qf.Show();

            var a = qf.CheckBox("Hello");

            qf.CheckBox("Hello");

            a.Value = true;

            qf.Button("Hello darkness", () => { a.Value = !a.Value; });
            qf.Button("Hello darkness", () => { MessageBox.Show("Pipo"); });

            qf.TrackBar("Hello", 0, 10, 1);
        }
    }
}
