using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

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