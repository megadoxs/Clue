using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Clue
{
    public partial class SlidingPuzzle : UserControl
    {
        private Button[,] buttons = new Button[3, 3]; // 3x3 array for the buttons
        private int emptyRow = 2, emptyCol = 2; // Position of the empty slot
        private int moves = 0; // Counter for moves
        private BitmapImage sourceImage; // The full image

        public SlidingPuzzle()
        {
            InitializeComponent();
            sourceImage = new BitmapImage(new Uri("pack://application:,,,/Images/PuzzleBackground.png")); // Ensure the image is in the project and set as 'Content'
            InitializePuzzle();
        }

        private void InitializePuzzle()
        {
            PuzzleGrid.Children.Clear(); // Clear the grid in case of reset
            moves = 0;
            UpdateMovesText();

            // Generate numbers 1 to 8 and shuffle them
            var numbers = Enumerable.Range(1, 8).OrderBy(_ => Guid.NewGuid()).ToList();
            numbers.Add(0); // Add 0 for the empty slot

            int index = 0;

            // Create the buttons and add them to the grid
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    int value = numbers[index++];
                    Button button = new Button
                    {
                        Tag = value, // Store the value in the Tag for easy reference
                        Background = value == 0 ? null : CreateImageBrush(value, 3) // Set image or leave empty
                    };

                    button.Style = (Style)this.FindResource("CustomButtonStyle");
                    button.Click += Button_Click; // Add the click event
                    buttons[row, col] = button;
                    PuzzleGrid.Children.Add(button);

                    if (value == 0) // Track the empty slot's position
                    {
                        emptyRow = row;
                        emptyCol = col;
                    }
                }
            }
        }

        private ImageBrush CreateImageBrush(int value, int gridSize)
        {
            int pieceSize = (int)sourceImage.PixelWidth / gridSize; //gives an error
            int row = (value - 1) / gridSize;
            int col = (value - 1) % gridSize;

            CroppedBitmap croppedBitmap = new CroppedBitmap(
                sourceImage,
                new Int32Rect(col * pieceSize, row * pieceSize, pieceSize, pieceSize));

            return new ImageBrush(croppedBitmap);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            // Find the clicked button's position in the grid
            int clickedRow = -1, clickedCol = -1;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (buttons[row, col] == clickedButton)
                    {
                        clickedRow = row;
                        clickedCol = col;
                        break;
                    }
                }
            }

            // Check if the clicked button is adjacent to the empty slot
            if (Math.Abs(clickedRow - emptyRow) + Math.Abs(clickedCol - emptyCol) == 1)
            {
                // Swap the clicked button with the empty slot
                buttons[emptyRow, emptyCol].Background = clickedButton.Background;
                buttons[emptyRow, emptyCol].Tag = clickedButton.Tag;

                clickedButton.Background = null;
                clickedButton.Tag = 0;

                emptyRow = clickedRow;
                emptyCol = clickedCol;

                // Increment move count and update display
                moves++;
                UpdateMovesText();

                CheckWinCondition();
            }
        }

        private void UpdateMovesText()
        {
            MovesTextBlock.Text = $"Moves: {moves}";
        }

        private void CheckWinCondition()
        {
            int expectedValue = 1;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    int currentValue = (int)buttons[row, col].Tag;

                    // Ignore the empty slot
                    if (currentValue == 0 && row == 2 && col == 2)
                        continue;

                    if (currentValue != expectedValue)
                        return;

                    expectedValue++;
                }
            }

            //MessageBox.Show("Congratulations! You solved the puzzle! Press \"OK\" to claim your cards!", "Victory!!!");
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.GameWin();
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            InitializePuzzle(); // Reset the board
        }

        private void GiveUpButton_Click(object sender, RoutedEventArgs e)
        {
            int value = 1;

            // Fill the grid in solved order
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    buttons[row, col].Background = value < 9 ? CreateImageBrush(value, 3) : null;
                    buttons[row, col].Tag = value < 9 ? value : 0;
                    value++;
                }
            }

            // Set the empty slot
            emptyRow = 2;
            emptyCol = 2;

            MessageBox.Show("Lol not happening, get good.");
        }
    }
}
