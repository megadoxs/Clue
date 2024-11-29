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

namespace Clue
{
    /// <summary>
    /// Interaction logic for CrackTheLock.xaml
    /// </summary>
    public partial class CrackTheLock : UserControl
    {
        private readonly int[] _lockCombination = { 3, 0, 1 };
        private int _hintIndex = 2; // Track which digit to hint next
        public CrackTheLock()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            // Parse player input
            if (!int.TryParse(Num1.Text, out int guess1) ||
                !int.TryParse(Num2.Text, out int guess2) ||
                !int.TryParse(Num3.Text, out int guess3))
            {
                FeedbackLabel.Text = "Please enter valid numbers!";
                return;
            }

            // Check if the guess is correct
            int[] playerGuess = { guess1, guess2, guess3 };
            if (IsCorrect(playerGuess))
            {
                FeedbackLabel.Text = "🎉 Correct! You unlocked the lock!";
                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.GameWin();
                }
            }
            else
            {
                FeedbackLabel.Text = "❌ Incorrect! Try again!";
            }
        }

        private void Hint_Click(object sender, RoutedEventArgs e)
        {
            // Provide hint logic
            if (_hintIndex < _lockCombination.Length)
            {
                FeedbackLabel.Text = $"Hint: This number references the date that marks the beginning of Lent in \"Ash Wednesday Feast\" By Olga Tokarczuk";
                _hintIndex++;
            }
            else
            {
                FeedbackLabel.Text = "No more hints available!";
            }
        }

        private bool IsCorrect(int[] guess)
        {
            // Compare guess with the lock combination
            for (int i = 0; i < _lockCombination.Length; i++)
            {
                if (guess[i] != _lockCombination[i])
                    return false;
            }
            return true;
        }
    }
}
