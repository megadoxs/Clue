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

namespace Clue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SimonSays : UserControl
    {
        private bool displaying;
        private int slevel = 0;
        private int[] sequence = new int[4]; // 0 = red, 1 = blue, 2 = green, 3 = yellow
        private List<int> userSequence = new List<int>();

        public SimonSays()
        {
            InitializeComponent();
        }

        private async void display_Sequence()
        {
            level.Text = "Level: " + ++slevel;

            displaying = true;

            for (int i = 0; i < slevel; i++)
            {
                await Task.Delay(1000);
                switch (sequence[i])
                {
                    case 0:
                        red.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        break;

                    case 1:
                        blue.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                        break;

                    case 2:
                        green.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                        break;

                    case 3:
                        yellow.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 0));
                        break;
                }
                await Task.Delay(1000);
                red.Fill = new SolidColorBrush(Color.FromRgb(178, 34, 34));
                blue.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 139));
                green.Fill = new SolidColorBrush(Color.FromRgb(0, 100, 0));
                yellow.Fill = new SolidColorBrush(Color.FromRgb(255, 215, 0));
            }

            displaying = false;
        }

        private void SequanceButton_Click(object sender, RoutedEventArgs e)
        {
            if (displaying)
                return;

            Button button = (Button) sender;
            int x = Grid.GetColumn(button);
            int y = Grid.GetRow(button);

            if (x == 1 && y == 0)
                userSequence.Add(0);
            else if (x == 0 && y == 1)
                userSequence.Add(1);
            else if (x == 2 && y == 1)
                userSequence.Add(2);
            else if (x == 1 && y == 2)
                userSequence.Add(3);

            if (userSequence[userSequence.Count() - 1] != sequence[userSequence.Count() - 1])
            {
                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.GameLose();
                }
            }
            else if (userSequence.Count() != sequence.Length && userSequence.Count() == slevel)
            {
                userSequence.Clear();
                display_Sequence();
            }
            else if (userSequence.Count() == sequence.Length)
            {
                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.GameWin();
                }
            }
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            userSequence.Clear();
            slevel = 0;
            Random rand = new Random();
            for (int i = 0; i < sequence.Length; i++)
            {
                sequence[i] = rand.Next(0, 4);
            }
            display_Sequence();
        }
    }
}
