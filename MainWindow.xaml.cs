using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class MainWindow : Window
    {
        public ObservableCollection<Card> UserCards { get; set; } = new ObservableCollection<Card>();
        public ObservableCollection<Card> AvailableCards { get; } = new ObservableCollection<Card>();
        public ObservableCollection<Card> Answers { get; }
        public MainWindow()
        {
            InitializeComponent();

            //Characters
            AvailableCards.Add(new Card("Green", new BitmapImage(new Uri("Images/Cards/Characters/green.png", UriKind.Relative)), Card.Type.Character));
            AvailableCards.Add(new Card("Mustard", new BitmapImage(new Uri("Images/Cards/Characters/mustard.png", UriKind.Relative)), Card.Type.Character));
            AvailableCards.Add(new Card("Peacock", new BitmapImage(new Uri("Images/Cards/Characters/peacock.png", UriKind.Relative)), Card.Type.Character));
            AvailableCards.Add(new Card("Plum", new BitmapImage(new Uri("Images/Cards/Characters/plum.png", UriKind.Relative)), Card.Type.Character));
            AvailableCards.Add(new Card("Scarlet", new BitmapImage(new Uri("Images/Cards/Characters/scarlet.png", UriKind.Relative)), Card.Type.Character));
            AvailableCards.Add(new Card("White", new BitmapImage(new Uri("Images/Cards/Characters/white.png", UriKind.Relative)), Card.Type.Character));

            //Weapons
            AvailableCards.Add(new Card("Candlestick", new BitmapImage(new Uri("Images/Cards/Weapons/candlestick.png", UriKind.Relative)), Card.Type.Weapon));
            AvailableCards.Add(new Card("Knife", new BitmapImage(new Uri("Images/Cards/Weapons/knife.png", UriKind.Relative)), Card.Type.Weapon));
            AvailableCards.Add(new Card("Pipe", new BitmapImage(new Uri("Images/Cards/Weapons/pipe.png", UriKind.Relative)), Card.Type.Weapon));
            AvailableCards.Add(new Card("Revolver", new BitmapImage(new Uri("Images/Cards/Weapons/revolver.png", UriKind.Relative)), Card.Type.Weapon));
            AvailableCards.Add(new Card("Rope", new BitmapImage(new Uri("Images/Cards/Weapons/rope.png", UriKind.Relative)), Card.Type.Weapon));
            AvailableCards.Add(new Card("Wrench", new BitmapImage(new Uri("Images/Cards/Weapons/wrench.png", UriKind.Relative)), Card.Type.Weapon));

            //Locations
            AvailableCards.Add(new Card("Ballroom", new BitmapImage(new Uri("Images/Cards/Locations/ballroom.png", UriKind.Relative)), Card.Type.Location));
            AvailableCards.Add(new Card("Billard Room", new BitmapImage(new Uri("Images/Cards/Locations/Billard.png", UriKind.Relative)), Card.Type.Location));
            AvailableCards.Add(new Card("Conservatory", new BitmapImage(new Uri("Images/Cards/Locations/Conservatory.png", UriKind.Relative)), Card.Type.Location));
            AvailableCards.Add(new Card("Dining Room", new BitmapImage(new Uri("Images/Cards/Locations/Dining.png", UriKind.Relative)), Card.Type.Location));
            AvailableCards.Add(new Card("Hall", new BitmapImage(new Uri("Images/Cards/Locations/hall.png", UriKind.Relative)), Card.Type.Location));
            AvailableCards.Add(new Card("Kitchen", new BitmapImage(new Uri("Images/Cards/Locations/kitchen.png", UriKind.Relative)), Card.Type.Location));
            AvailableCards.Add(new Card("Library", new BitmapImage(new Uri("Images/Cards/Locations/library.png", UriKind.Relative)), Card.Type.Location));
            AvailableCards.Add(new Card("Lounge", new BitmapImage(new Uri("Images/Cards/Locations/lounge.png", UriKind.Relative)), Card.Type.Location));
            AvailableCards.Add(new Card("Study", new BitmapImage(new Uri("Images/Cards/Locations/study.png", UriKind.Relative)), Card.Type.Location));

            //Choses randomly the killer, the weapon, and the location
            var random = new Random();
            Answers = new ObservableCollection<Card>(AvailableCards.GroupBy(card => card.type).Select(group => group.ElementAt(random.Next(group.Count()))).ToList());

            DataContext = this;
        }
    }
}