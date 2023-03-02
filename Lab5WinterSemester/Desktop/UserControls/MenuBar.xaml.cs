using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Win32;

namespace Lab5WinterSemester.Desktop.UserControls;

public partial class MenuBar : UserControl
{
    public MenuBar()
    {
        InitializeComponent();
        
        TogBtnExplorer.IsChecked = true;
        TogBtnMetaData.IsChecked = true;
        TogBtnData.IsChecked = true;
    }
}