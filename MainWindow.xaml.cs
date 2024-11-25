using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Path = System.Windows.Shapes.Path;

namespace Clue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Card> UserCards { get; set; } = new ObservableCollection<Card>();
        public ObservableCollection<Card> UserCharacters { get; set; } = new ObservableCollection<Card>();
        public ObservableCollection<Card> UserLocations { get; set; } = new ObservableCollection<Card>();
        public ObservableCollection<Card> UserWeapons { get; set; } = new ObservableCollection<Card>();
        public ObservableCollection<Card> AvailableCharacters { get; } = new ObservableCollection<Card>();
        public ObservableCollection<Card> AvailableLocations { get; } = new ObservableCollection<Card>();
        public ObservableCollection<Card> AvailableWeapons { get; } = new ObservableCollection<Card>();
        public ObservableCollection<Card> Guess { get; set; } = new ObservableCollection<Card>();
        public ObservableCollection<Card> Answers { get; } = new ObservableCollection<Card>();

        public const int CharactersSize = 6;
        public const int WeaponsSize = 6;
        public const int LocationsSize = 9;

        public MainWindow()
        {
            InitializeComponent();

            //Characters
            AvailableCharacters.Add(new Card("Green", new BitmapImage(new Uri("Images/Cards/Characters/green.png", UriKind.Relative)), Card.Type.Character));
            AvailableCharacters.Add(new Card("Mustard", new BitmapImage(new Uri("Images/Cards/Characters/mustard.png", UriKind.Relative)), Card.Type.Character));
            AvailableCharacters.Add(new Card("Peacock", new BitmapImage(new Uri("Images/Cards/Characters/peacock.png", UriKind.Relative)), Card.Type.Character));
            AvailableCharacters.Add(new Card("Plum", new BitmapImage(new Uri("Images/Cards/Characters/plum.png", UriKind.Relative)), Card.Type.Character));
            AvailableCharacters.Add(new Card("Scarlet", new BitmapImage(new Uri("Images/Cards/Characters/scarlet.png", UriKind.Relative)), Card.Type.Character));
            AvailableCharacters.Add(new Card("White", new BitmapImage(new Uri("Images/Cards/Characters/white.png", UriKind.Relative)), Card.Type.Character));

            //Weapons
            AvailableWeapons.Add(new Card("Candlestick", new BitmapImage(new Uri("Images/Cards/Weapons/candlestick.png", UriKind.Relative)), Card.Type.Weapon));
            AvailableWeapons.Add(new Card("Knife", new BitmapImage(new Uri("Images/Cards/Weapons/knife.png", UriKind.Relative)), Card.Type.Weapon));
            AvailableWeapons.Add(new Card("Pipe", new BitmapImage(new Uri("Images/Cards/Weapons/pipe.png", UriKind.Relative)), Card.Type.Weapon));
            AvailableWeapons.Add(new Card("Revolver", new BitmapImage(new Uri("Images/Cards/Weapons/revolver.png", UriKind.Relative)), Card.Type.Weapon));
            AvailableWeapons.Add(new Card("Rope", new BitmapImage(new Uri("Images/Cards/Weapons/rope.png", UriKind.Relative)), Card.Type.Weapon));
            AvailableWeapons.Add(new Card("Wrench", new BitmapImage(new Uri("Images/Cards/Weapons/wrench.png", UriKind.Relative)), Card.Type.Weapon));

            //Locations
            AvailableLocations.Add(new Card("Ballroom", new BitmapImage(new Uri("Images/Cards/Locations/ballroom.png", UriKind.Relative)), Card.Type.Location));
            AvailableLocations.Add(new Card("Billard Room", new BitmapImage(new Uri("Images/Cards/Locations/Billard.png", UriKind.Relative)), Card.Type.Location));
            AvailableLocations.Add(new Card("Conservatory", new BitmapImage(new Uri("Images/Cards/Locations/Conservatory.png", UriKind.Relative)), Card.Type.Location));
            AvailableLocations.Add(new Card("Dining Room", new BitmapImage(new Uri("Images/Cards/Locations/Dining.png", UriKind.Relative)), Card.Type.Location));
            AvailableLocations.Add(new Card("Hall", new BitmapImage(new Uri("Images/Cards/Locations/hall.png", UriKind.Relative)), Card.Type.Location));
            AvailableLocations.Add(new Card("Kitchen", new BitmapImage(new Uri("Images/Cards/Locations/kitchen.png", UriKind.Relative)), Card.Type.Location));
            AvailableLocations.Add(new Card("Library", new BitmapImage(new Uri("Images/Cards/Locations/library.png", UriKind.Relative)), Card.Type.Location));
            AvailableLocations.Add(new Card("Lounge", new BitmapImage(new Uri("Images/Cards/Locations/lounge.png", UriKind.Relative)), Card.Type.Location));
            AvailableLocations.Add(new Card("Study", new BitmapImage(new Uri("Images/Cards/Locations/study.png", UriKind.Relative)), Card.Type.Location));

            //Choses randomly the killer, the weapon, and the location
            var random = new Random();

            Answers.Add(AvailableCharacters.ElementAt(random.Next(AvailableCharacters.Count)));
            Answers.Add(AvailableWeapons.ElementAt(random.Next(AvailableWeapons.Count)));
            Answers.Add(AvailableLocations.ElementAt(random.Next(AvailableLocations.Count)));

            for (int i = UserCharacters.Count; i < CharactersSize; i++)
            {
                UserCharacters.Add(new Card("Empty", Card.CardsBack, Card.Type.None));
            }

            for (int i = UserLocations.Count; i < LocationsSize; i++)
            {
                UserLocations.Add(new Card("Empty", Card.CardsBack, Card.Type.None));
            }

            for (int i = UserWeapons.Count; i < WeaponsSize; i++)
            {
                UserWeapons.Add(new Card("Empty", Card.CardsBack, Card.Type.None));
            }

            DataContext = this;
        }

        public void AddCard(Card card)
        {
            // shouldn't be needed, but here just in case to make sur no duplicate can be added
            if (UserCards.Contains(card))
                return;

            UserCards.Add(card);

            switch (card.type)
            {
                case Card.Type.Character:
                    UserCharacters.Clear();
                    foreach (Card OwnedCard in UserCards)
                    {
                        if (OwnedCard.type.Equals(card.type))
                            UserCharacters.Add(OwnedCard);
                    }
                    for (int i = UserCharacters.Count; i < CharactersSize; i++)
                    {
                        UserCharacters.Add(new Card("Empty", Card.CardsBack, Card.Type.None));
                    }
                    break;
                case Card.Type.Location:
                    UserLocations.Clear();
                    foreach (Card OwnedCard in UserCards)
                    {
                        if (OwnedCard.type.Equals(card.type))
                            UserLocations.Add(OwnedCard);
                    }
                    for (int i = UserLocations.Count; i < LocationsSize; i++)
                    {
                        UserLocations.Add(new Card("Empty", Card.CardsBack, Card.Type.None));
                    }
                    break;
                case Card.Type.Weapon:
                    UserWeapons.Clear();
                    foreach (Card OwnedCard in UserCards)
                    {
                        if (OwnedCard.type.Equals(card.type))
                            UserWeapons.Add(OwnedCard);
                    }
                    for (int i = UserWeapons.Count; i < WeaponsSize; i++)
                    {
                        UserWeapons.Add(new Card("Empty", Card.CardsBack, Card.Type.None));
                    }
                    break;
            }
        }

        private void OpenInventory(object sender, MouseButtonEventArgs e)
        {
            Inventory.Visibility = Visibility.Visible;
            buttons.Visibility = Visibility.Hidden;
        }

        private void CloseInventory(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as Grid;

            if (e.OriginalSource == grid)
            {
                Inventory.Visibility = Visibility.Hidden;
                buttons.Visibility = Visibility.Visible;
            }
        }
        private void OpenGuess(object sender, MouseButtonEventArgs e)
        {
            GuessPannel.Visibility = Visibility.Visible;
            buttons.Visibility = Visibility.Hidden;
        }

        private void CloseGuess(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as Grid;

            if (e.OriginalSource == grid)
            {
                GuessPannel.Visibility = Visibility.Hidden;
                buttons.Visibility = Visibility.Visible;
            }
        }

        private void ClickGuessCard(object sender, MouseButtonEventArgs e)
        {
            var image = (Image) sender;
            var border = (Border)image.Parent;
            border.BorderBrush = Brushes.White;
            
            var gcard = image.DataContext as Card;

            var toRemove = Guess.FirstOrDefault(card => card.type == gcard.type);
            if (toRemove != null)
            {
                Guess.Remove(toRemove);
            }

            Guess.Add(gcard);

            switch (gcard.type)
            {
                case Card.Type.Character:
                    foreach (var item in characters.Items)
                    {
                        var element = characters.ItemContainerGenerator.ContainerFromItem(item);
                        if (((FrameworkElement)element).DataContext != gcard)
                        {
                            Border b = (Border)VisualTreeHelper.GetChild(element, 0);
                            b.BorderBrush = Brushes.Black;
                        }
                    }
                    break;
                case Card.Type.Weapon:
                    foreach (var item in weapons.Items)
                    {
                        var element = weapons.ItemContainerGenerator.ContainerFromItem(item);
                        if (((FrameworkElement)element).DataContext != gcard)
                        {
                            Border b = (Border)VisualTreeHelper.GetChild(element, 0);
                            b.BorderBrush = Brushes.Black;
                        }
                    }
                    break;
                case Card.Type.Location:
                    foreach (var item in locations.Items)
                    {
                        var element = locations.ItemContainerGenerator.ContainerFromItem(item);
                        if (((FrameworkElement)element).DataContext != gcard)
                        {
                            Border b = (Border)VisualTreeHelper.GetChild(element, 0);
                            b.BorderBrush = Brushes.Black;
                        }
                    }
                    break;
            }
        }

        private void ConfirmGuess(object sender, MouseButtonEventArgs e)
        {
            foreach (var card in Guess)
            {
                if (!Answers.Contains(card))
                {
                    MessageBox.Show("You Lost");
                    this.Close();
                    return;
                }
            }
            MessageBox.Show("You Won");
        }

        private void WorldMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(((Path) sender).Name);
        }
    }
}