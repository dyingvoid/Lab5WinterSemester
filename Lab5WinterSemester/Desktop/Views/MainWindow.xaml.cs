﻿using System.Collections.Generic;
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
    }
}