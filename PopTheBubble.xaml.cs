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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Clue
{
    /// <summary>
    /// Interaction logic for PopTheBubble.xaml
    /// </summary>
    public partial class PopTheBubble : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer(); // create a new instance of the dispatcher timer called gameTimer

        List<Ellipse> removeThis = new List<Ellipse>(); // list of ellipses to be removed

        // necessary game variables
        int spawnRate = 60; // default spawn rate of circles
        int currentRate; // current rate for spawning circles
        int health = 350; // player health at the start
        int posX; // x position of the circles
        int posY; // y position of the circles
        int score = 0; // current score for the game

        double growthRate = 0.6; // default growth rate for each circle

        Random rand = new Random(); // random number generator

        bool gameCompleted = false; // prevents further play once the player reaches 25 points

        // colour for the circles
        Brush brush;

        public PopTheBubble()
        {
            InitializeComponent();

            // initialize game settings
            gameTimer.Tick += GameLoop; // set the game loop
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); // timer ticks every 20 ms
            gameTimer.Start(); // start the timer

            currentRate = spawnRate; // set the current rate to the default spawn rate
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (gameCompleted) return; // Stop the game loop if the game is completed

            // game loop logic
            txtScore.Content = "Score: " + score;

            currentRate -= 2;

            if (currentRate < 1)
            {
                currentRate = spawnRate;

                posX = rand.Next(15, 700);
                posY = rand.Next(50, 350);

                // Generate a random shade of blue
                brush = new SolidColorBrush(Color.FromRgb(0, 0, (byte)rand.Next(100, 255)));

                Ellipse circle = new Ellipse
                {
                    Tag = "circle",
                    Height = 10,
                    Width = 10,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Fill = brush
                };

                Canvas.SetLeft(circle, posX);
                Canvas.SetTop(circle, posY);
                MyCanvas.Children.Add(circle);
            }

            foreach (var x in MyCanvas.Children.OfType<Ellipse>())
            {
                x.Height += growthRate;
                x.Width += growthRate;
                x.RenderTransformOrigin = new Point(0.5, 0.5);

                if (x.Width > 70)
                {
                    removeThis.Add(x);
                    health -= 15;
                }
            }

            if (health > 1)
            {
                healthBar.Width = health;
            }
            else
            {
                GameOverFunction();
            }

            foreach (Ellipse i in removeThis)
            {
                MyCanvas.Children.Remove(i);
            }

            if (score > 5)
            {
                spawnRate = 25;
            }

            if (score > 20)
            {
                spawnRate = 15;
                growthRate = 1.5;
            }
        }

        private void ClickOnCanvas(object sender, MouseButtonEventArgs e)
        {
            if (gameCompleted) return; // Stop interaction if the game is completed

            if (e.OriginalSource is Ellipse)
            {
                Ellipse circle = (Ellipse)e.OriginalSource;
                MyCanvas.Children.Remove(circle);
                score++;
            }
        }

        private void GameOverFunction()
        {
            gameTimer.Stop();

            if (score >= 25)
            {
                gameCompleted = true; // Lock the game from further play
                MessageBox.Show("Congratulations! You scored " + score + " points! Press \"OK\" to clam your cards!", "Game Over");
                this.Close();
            }
            else
            {
                MessageBox.Show("Game Over! You scored " + score + " points. Try again.", "Game Over");

                // Reset game values if the score is below 25
                ResetGame();
            }
        }

        private void ResetGame()
        {
            foreach (var y in MyCanvas.Children.OfType<Ellipse>())
            {
                removeThis.Add(y);
            }

            foreach (Ellipse i in removeThis)
            {
                MyCanvas.Children.Remove(i);
            }

            growthRate = .6;
            spawnRate = 60;
            score = 0;
            currentRate = 5;
            health = 350;
            removeThis.Clear();
            gameTimer.Start();
        }
    }
}
