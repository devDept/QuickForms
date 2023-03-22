using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using QuickForms.Wpf.Elements.ColorPicker.Models;

namespace QuickForms.Wpf.Elements.ColorPicker
{
    public partial class ColorPicker
    {
        public static readonly DependencyProperty ActiveColorProperty = DependencyProperty.RegisterAttached("ActiveColor", typeof(Color), typeof(ColorPicker),
            new PropertyMetadata(Color.FromArgb(255, 255, 0, 0)));

        public Color ActiveColor
        {
            get => (Color) GetValue(ActiveColorProperty);
            set => SetValue(ActiveColorProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(CornerRadius), typeof(ColorPicker), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly RoutedEvent ColorChangedEvent = EventManager.RegisterRoutedEvent(nameof(ColorChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ColorPicker));

        public event RoutedEventHandler ColorChanged
        {
            add => AddHandler(ColorChangedEvent, value);
            remove => RemoveHandler(ColorChangedEvent, value);
        }

        public ColorPicker()
        {
            InitializeComponent();
        }

        private void Btn_OnClick(object sender, RoutedEventArgs e)
        {
            ((Button) sender).ContextMenu.IsOpen = true;
            ((Button) sender).ContextMenu.PlacementTarget = (Button) sender;
        }

        private void Sliders_OnColorChanged(object sender, RoutedEventArgs e)
        {
            if (e is ColorRoutedEventArgs cre)
            {
                RaiseEvent(new ColorRoutedEventArgs(ColorChangedEvent, cre.Color));
            }
        }
    }
}
