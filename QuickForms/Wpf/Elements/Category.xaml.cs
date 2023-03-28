using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuickForms.Wpf.Elements
{
    internal partial class Category : UserControl
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(CornerRadius), typeof(Category), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty TitleVisibilityProperty = DependencyProperty.Register(
            nameof(TitleVisibility), typeof(Visibility), typeof(Category), new PropertyMetadata(default(Visibility)));

        public Visibility TitleVisibility
        {
            get => (Visibility) GetValue(TitleVisibilityProperty);
            set => SetValue(TitleVisibilityProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(Category), new PropertyMetadata(default(string)));

        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleBrushProperty = DependencyProperty.Register(
            nameof(TitleBrush), typeof(Brush), typeof(Category), new PropertyMetadata(default(Brush)));

        public Brush TitleBrush
        {
            get => (Brush) GetValue(TitleBrushProperty);
            set => SetValue(TitleBrushProperty, value);
        }

        public static readonly DependencyProperty MinTitleHeightProperty = DependencyProperty.Register(
            nameof(MinTitleHeight), typeof(double), typeof(Category), new PropertyMetadata(default(double)));

        public double MinTitleHeight
        {
            get => (double) GetValue(MinTitleHeightProperty);
            set => SetValue(MinTitleHeightProperty, value);
        }

        public Category()
        {
            InitializeComponent();
        }
    }
}
