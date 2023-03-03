using System.ComponentModel;

namespace Lab5WinterSemester.Desktop.Models;

public class Notifier : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged = delegate {  };

    protected void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}