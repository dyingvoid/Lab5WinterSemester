using System.Data;
using System.Windows;
using Lab5WinterSemester.Core.TableClasses;
using Lab5WinterSemester.Desktop.Models;
using Lab5WinterSemester.Desktop.ViewModels;

namespace Lab5WinterSemester.Desktop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IMainModel mainModel = new MainModel();
            DataContext = new MainWindowViewModel(mainModel);
        }
        
        public static readonly DependencyProperty DataViewProperty = DependencyProperty.Register(
            nameof(DataView),
            typeof(DataView),
            typeof(MainWindow)
            );
        
        public DataView DataView
        {
            get => (DataView)GetValue(DataViewProperty);
            set => SetValue(DataViewProperty, value);
        }

        private void ConvertTableToDataView(object sender, RoutedEventArgs e)
        {
            var dataTable = new DataTable();
            
            var table = Explorer.SelectedItem as ITable;
            if (table == null) return;

            foreach (var (key, value) in table.Elements)
            {
                dataTable.Columns.Add(key, typeof(object));
            }

            for (var i = 0; i < table.Shape.Item2; i++)
            {
                var row = dataTable.NewRow();

                var counter = 0;
                foreach (var (columnName, column) in table.Elements)
                {
                    row[counter] = column[i];
                    counter++;
                }

                dataTable.Rows.Add(row);
            }

            DataView = dataTable.DefaultView;
        }
    }
}