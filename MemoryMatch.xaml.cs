using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Clue // Ensure namespace matches XAML's x:Class
{
    public partial class MemoryMatch : UserControl
    {
        private List<string> imagePaths;
        private Button firstClicked = null;
        private Button secondClicked = null;
        private DispatcherTimer countdownTimer;
        private int timeLeft = 100; // Countdown time in seconds
        private bool canClick = true; // To prevent fast clicking

        public MemoryMatch()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Create a list of image paths (8 pairs - example paths)
            imagePaths = new List<string>
            {
                "pack://application:,,,/Images/Countries/CanadaPhoto.png", "pack://application:,,,/Images/Countries/FREEPALESTINE.png",
                "pack://application:,,,/Images/Countries/NorwayPhoto.png", "pack://application:,,,/Images/Countries/PakistaniPhoto.png",
                "pack://application:,,,/Images/Countries/PhilippinesPhoto.png", "pack://application:,,,/Images/Countries/PortugalPhoto.png",
                "pack://application:,,,/Images/Countries/UKPhoto.png", "pack://application:,,,/Images/Countries/USAPhoto.png",

                "pack://application:,,,/Images/Countries/CanadaPhoto.png", "pack://application:,,,/Images/Countries/FREEPALESTINE.png",
                "pack://application:,,,/Images/Countries/NorwayPhoto.png", "pack://application:,,,/Images/Countries/PakistaniPhoto.png",
                "pack://application:,,,/Images/Countries/PhilippinesPhoto.png", "pack://application:,,,/Images/Countries/PortugalPhoto.png",
                "pack://application:,,,/Images/Countries/UKPhoto.png", "pack://application:,,,/Images/Countries/USAPhoto.png"
            };

            // Shuffle the image paths
            var random = new Random();
            imagePaths = imagePaths.OrderBy(x => random.Next()).ToList();

            // Add buttons to the grid
            CardGrid.Children.Clear();
            for (int i = 0; i < 16; i++)
            {
                var button = new Button
                {
                    Background = Brushes.LightGray,
                    Tag = imagePaths[i] // Store the image path in the button's Tag property
                };

                button.Focusable = false;
                button.Click += Button_Click;
                CardGrid.Children.Add(button);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!canClick) // Prevent clicking while processing
                return;

            var clickedButton = sender as Button;

            if (clickedButton == null || clickedButton.Content != null)
                return;

            // Reveal the image
            string imagePath = clickedButton.Tag.ToString();
            try
            {
                var image = new Image
                {
                    Source = new BitmapImage(new Uri(imagePath)),
                    Stretch = Stretch.Uniform
                };
                clickedButton.Content = image;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Handle first and second clicks
            if (firstClicked == null)
            {
                firstClicked = clickedButton;
                return;
            }

            secondClicked = clickedButton;

            // Disable clicking during evaluation
            canClick = false;

            // Check for a match
            if (firstClicked.Tag.ToString() == secondClicked.Tag.ToString())
            {
                firstClicked = null;
                secondClicked = null;
                canClick = true; // Re-enable clicking
                CheckForWinner();
            }
            else
            {
                // Use a timer to delay hiding the cards again
                var timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(0.75)
                };
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    firstClicked.Content = null;
                    secondClicked.Content = null;
                    firstClicked = null;
                    secondClicked = null;
                    canClick = true; // Re-enable clicking
                };
                timer.Start();
            }
        }

        private void CheckForWinner()
        {
            foreach (Button button in CardGrid.Children)
            {
                if (button.Content == null)
                    return;
            }

            countdownTimer.Stop(); // Stop the countdown if the player wins
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.GameWin();
            }
            ResetGame();
        }

        private void ResetGame()
        {
            CardGrid.Children.Clear();
            InitializeGame();
            timeLeft = 100; // Reset the timer
            TimerText.Text = "Time Left: 100";
            StartCountdown();
        }

        private void StartCountdown()
        {
            // Initialize the countdown timer
            countdownTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            countdownTimer.Tick += (s, e) =>
            {
                timeLeft--;

                // Update the timer display
                TimerText.Text = $"Time Left: {timeLeft}";

                // Check if time has run out
                if (timeLeft <= 0)
                {
                    countdownTimer.Stop();
                    MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                    if (mainWindow != null)
                    {
                        mainWindow.GameLose();
                    }
                    //ResetGame();
                }
            };

            countdownTimer.Start();
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            countdownTimer.Stop();
        }
    }
}
