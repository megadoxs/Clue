using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Clue.FindWaldo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<int> found = new List<int>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FishFound(object sender, MouseButtonEventArgs e)
        {
            var path = sender as Path;
            if (path.Visibility == Visibility.Visible)
            {
                path.Visibility = Visibility.Hidden;
                found.Add(int.Parse(path.Tag.ToString()));
                switch (found.Last())
                {
                    case 1:
                        fish1.Visibility = Visibility.Hidden;
                        break;
                    case 2:
                        fish2.Visibility = Visibility.Hidden;
                        break;
                    case 3:
                        fish3.Visibility = Visibility.Hidden;
                        break;
                    case 4:
                        fish4.Visibility = Visibility.Hidden;
                        break;
                    case 5:
                        fish5.Visibility = Visibility.Hidden;
                        break;
                }
                if (found.Count() == 5)
                {
                    MessageBox.Show("You Won");
                }
            }
        }
    }
}
