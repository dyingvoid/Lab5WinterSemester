using System.Windows;
using System.Windows.Controls;

namespace Lab5WinterSemester.Desktop.UserControls;

public class MainColumnDefinition : ColumnDefinition
{
    private bool _isVisible;
    private GridLength _width;

    public static readonly DependencyProperty IsVisibleProperty;

    static MainColumnDefinition()
    {
        IsVisibleProperty = DependencyProperty.Register("IsVisible",
            typeof(bool?),
            typeof(MainColumnDefinition));
    }

    public MainColumnDefinition()
    {
        _isVisible = true;
    }

    public bool? IsVisible
    {
        get => (bool?)GetValue(IsVisibleProperty);
        set => SetValue(IsVisibleProperty, value);
    }

    public void ChangeVisibility()
    {
        if (_isVisible)
        {
            Hide();
            return;
        }
        
        Show();
    }
    
    public void Hide()
    {
        if (!_isVisible) return;
        _width = Width;
        Width = new GridLength(0);
        _isVisible = false;
    }

    public void Show()
    {
        if(_isVisible) return;
        Width = _width;
        _isVisible = true;
    }
}