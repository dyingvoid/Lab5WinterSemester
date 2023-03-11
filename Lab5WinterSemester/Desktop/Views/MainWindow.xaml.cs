using System.Data;
using System.Linq;
using System.Windows;
using Lab5WinterSemester.Core.TableClasses;
using Lab5WinterSemester.Desktop.Models;
using Lab5WinterSemester.Desktop.UserControls;
using Lab5WinterSemester.Desktop.ViewModels;

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
        }
        
        public static readonly DependencyProperty DataViewProperty = DependencyProperty.Register(
            nameof(DataView),
            typeof(DataView),
            typeof(MainWindow));
        public DataView DataView
        {
            get => (DataView)GetValue(DataViewProperty);
            set => SetValue(DataViewProperty, value);
        }

        private void ChangeDefinitionVisibility(object sender, RoutedEventArgs e)
        {
            var indexOfButton = MenuBarGrid.Children.IndexOf((UIElement)e.OriginalSource);
            var definition = MainGrid.ColumnDefinitions
                .Cast<MainColumnDefinition>().GetEnumerator();
            
            for (int i = 0; i <= indexOfButton; ++i)
                definition.MoveNext();
            
            definition.Current.ChangeVisibility();

            definition.Dispose();
        }

        private void ConvertTableToDataView(object sender, RoutedEventArgs e)
        {
            var dataTable = new DataTable();
            
            var table = Explorer.SelectedItem as ITable;
            if (table == null) return ;

            foreach (var (key, value) in table.Elements)
            {
                dataTable.Columns.Add(key, typeof(object));
            }
        
            foreach (var (name, list) in table.Elements)
            {
                var row = dataTable.NewRow();
                for (var i = 0; i < list.Count; ++i)
                {
                    row[i] = list[i];
                }

                dataTable.Rows.Add(row);
            }

            DataView = dataTable.DefaultView;
        }
    }
}