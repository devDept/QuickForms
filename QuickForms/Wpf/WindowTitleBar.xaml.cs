using System.Windows;
using System.Windows.Controls;
using QuickForms.Core;

namespace QuickForms.Wpf
{
    /// <summary>
    /// User control used to customize the form title bar.
    /// </summary>
    internal partial class WindowTitleBar : UserControl
    {
        public static readonly RoutedEvent CloseEvent = EventManager.RegisterRoutedEvent(
            name: "Close",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(WindowTitleBar));
        
        public event RoutedEventHandler Close
        {
            add => AddHandler(CloseEvent, value);
            remove => RemoveHandler(CloseEvent, value);
        }

        public static readonly RoutedEvent MinimizeEvent = EventManager.RegisterRoutedEvent(
            name: "Minimize",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(WindowTitleBar));

        public event RoutedEventHandler Minimize
        {
            add => AddHandler(MinimizeEvent, value);
            remove => RemoveHandler(MinimizeEvent, value);
        }

        public static readonly RoutedEvent MaximizeEvent = EventManager.RegisterRoutedEvent(
            name: "Maximize",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(WindowTitleBar));

        public event RoutedEventHandler Maximize
        {
            add => AddHandler(MaximizeEvent, value);
            remove => RemoveHandler(MaximizeEvent, value);
        }

        public WindowTitleBar()
        {
            InitializeComponent();
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs routedEventArgs = new(routedEvent: CloseEvent);
            RaiseEvent(routedEventArgs);
        }

        private void Minimize_OnClick(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs routedEventArgs = new(routedEvent: MinimizeEvent);
            RaiseEvent(routedEventArgs);
        }

        private void Maximize_OnClick(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs routedEventArgs = new(routedEvent: MaximizeEvent);
            RaiseEvent(routedEventArgs);
        }

        public void SetTheme(Themes theme)
        {
            Resources = ThemeHelper.GetGeneric(theme);
        }
    }
}
