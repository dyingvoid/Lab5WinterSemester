using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using Lab5WinterSemester.Desktop.UserControls;


namespace Lab5WinterSemester.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ConfigureToggleButtons();
        }

        private void ConfigureToggleButtons()
        {
            menuBar.AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(ChangeDefinitionVisibility));
            menuBar.AddHandler(ToggleButton.UncheckedEvent, new RoutedEventHandler(ChangeDefinitionVisibility));
        }

        private void ChangeDefinitionVisibility(object sender, RoutedEventArgs e)
        {
            var indexOfButton = menuBar.MenuBarGrid.Children.IndexOf((UIElement)e.OriginalSource);
            var definition = MainGrid.ColumnDefinitions
                .Cast<MainColumnDefinition>().GetEnumerator();
            
            for (int i = 0; i <= indexOfButton; ++i)
                definition.MoveNext();
            
            definition.Current.ChangeVisibility();

            definition.Dispose();
        }
    }
}