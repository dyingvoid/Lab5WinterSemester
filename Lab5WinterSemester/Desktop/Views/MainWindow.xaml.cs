using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using Lab5WinterSemester.Desktop.Models;
using Lab5WinterSemester.Desktop.UserControls;
using Lab5WinterSemester.Desktop.ViewModels;
using Microsoft.Win32;

namespace Lab5WinterSemester.Desktop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IMainModel _mainModel;
        
        public MainWindow()
        {
            InitializeComponent();

            _mainModel = new MainModel();
            DataContext = new MainWindowViewModel(_mainModel);
            
            ConfigureButtons();
        }
        

        private void ConfigureButtons()
        {
            menuBar.BtnDbOpen.Click += OpenFile;
            menuBar.AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(ChangeDefinitionVisibility));
            menuBar.AddHandler(ToggleButton.UncheckedEvent, new RoutedEventHandler(ChangeDefinitionVisibility));
        }
        
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            FileInfo dataBaseSchemaFile;
            if (fileDialog.ShowDialog() == true)
            {
                dataBaseSchemaFile = new FileInfo(fileDialog.FileName);
            }
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