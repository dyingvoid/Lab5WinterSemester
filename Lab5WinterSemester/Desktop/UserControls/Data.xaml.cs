using System.Collections.Generic;
using System.Windows.Controls;

namespace Lab5WinterSemester.Desktop.UserControls;

public partial class Data : UserControl
{
    public Data()
    {
        InitializeComponent();
        List<Phone> phonesList = new List<Phone>
        {
            new Phone { Title="iPhone 6S", Company="Apple", Price=54990 },
            new Phone {Title="Lumia 950", Company="Microsoft", Price=39990 },
            new Phone {Title="Nexus 5X", Company="Google", Price=29990 },
            new Phone { Title="iPhone 6S", Company="Apple", Price=54990 },
            new Phone {Title="Lumia 950", Company="Microsoft", Price=39990 },
            new Phone {Title="Nexus 5X", Company="Google", Price=29990 },
            new Phone { Title="iPhone 6S", Company="Apple", Price=54990 },
            new Phone {Title="Lumia 950", Company="Microsoft", Price=39990 },
            new Phone {Title="Nexus 5X", Company="Google", Price=29990 },
            new Phone { Title="iPhone 6S", Company="Apple", Price=54990 },
            new Phone {Title="Lumia 950", Company="Microsoft", Price=39990 },
            new Phone {Title="Nexus 5X", Company="Google", Price=29990 },
            new Phone { Title="iPhone 6S", Company="Apple", Price=54990 },
            new Phone {Title="Lumia 950", Company="Microsoft", Price=39990 },
            new Phone {Title="Nexus 5X", Company="Google", Price=29990 },
            new Phone { Title="iPhone 6S", Company="Apple", Price=54990 },
            new Phone {Title="Lumia 950", Company="Microsoft", Price=39990 },
            new Phone {Title="Nexus 5X", Company="Google", Price=29990 },
            new Phone { Title="iPhone 6S", Company="Apple", Price=54990 },
            new Phone {Title="Lumia 950", Company="Microsoft", Price=39990 },
            new Phone {Title="Nexus 5X", Company="Google", Price=29990 }
        };
        Table.ItemsSource = phonesList;
    }
    
    public class Phone
    {
        public string Title { get; set; }
        public string Company { get; set; }
        public int Price { get; set; }
    }
}