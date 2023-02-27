using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;


namespace Lab5WinterSemester.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<GridLength> _columnWidths;
        private List<ToggleButton> _buttons;

        public MainWindow()
        {
            InitializeComponent();

            _columnWidths = new List<GridLength>();
            RememberWidths();

            ConfigureButtonEvents();
        }

        private void ConfigureButtonEvents()
        {
            _buttons = new List<ToggleButton>()
            {
                menuBar.TogBtnExplorer,
                menuBar.TogBtnMetaData,
                menuBar.TogBtnData
            };
            
            foreach (var btn in _buttons)
            {
                btn.Checked += ShowColumnEvent;
                btn.Unchecked += HideColumnEvent;
            }
        }

        private void ShowColumnEvent(object sender, RoutedEventArgs e)
        {
            var buttonIndex = _buttons.FindIndex(button => button == (ToggleButton)sender);
            ShowColumn(buttonIndex + 1);
        }

        private void HideColumnEvent(object sender, RoutedEventArgs e)
        {
            var buttonIndex = _buttons.FindIndex(button => button == (ToggleButton)sender);
            HideColumn(buttonIndex + 1);
        }

        private void HideColumn(int index)
        {
            Grid.ColumnDefinitions[index].Width = new GridLength(0);
        }

        private void ShowColumn(int index)
        {
            Grid.ColumnDefinitions[index].Width = _columnWidths[index];
        }

        private void RememberWidths()
        {
            foreach (var columnDefinition in Grid.ColumnDefinitions)
            {
                _columnWidths.Add(columnDefinition.Width);
            }
        }

    }
}