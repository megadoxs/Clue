using NHunspell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Clue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Wordle : Window
    {
        private List<string> answers = new List<string>() { "ocean", "whale", "glass", "water", "train", "earth", "river", "plane" };
        private Hunspell dictionary = new Hunspell("en_us.aff", "en_us.dic");
        private List<char> word = new List<char>();
        private string answer;
        private int tries = 0;
        public Wordle()
        {
            InitializeComponent();
            Random random = new Random();
            answer = answers[random.Next(0, 8)];

            Guess.Width = answer.Length * 70;
            Guess.Height = 420;

            for (int i = 0; i < 6; i++)
            {
                Guess.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < answer.Length; i++)
            {
                Guess.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < answer.Length; col++)
                {
                    Border border = new Border();
                    border.SetValue(Grid.RowProperty, row);
                    border.SetValue(Grid.ColumnProperty, col);
                    border.Style = (Style)FindResource("GridItemStyle");

                    TextBlock textBlock = new TextBlock();
                    textBlock.FontSize = 50;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
                    textBlock.TextAlignment = TextAlignment.Center;

                    border.Child = textBlock;
                    Guess.Children.Add(border);
                }
            }
        }



        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.A && e.Key <= Key.Z && word.Count() < answer.Length)
            {
                foreach (Border item in Guess.Children)
                {
                    if (Grid.GetColumn(item) == word.Count() && Grid.GetRow(item) == tries)
                        ((TextBlock)item.Child).Text = e.Key.ToString().ToUpper();
                }
                word.Add(char.Parse(e.Key.ToString().ToLower()));
            }
            else if (e.Key == Key.Back && word.Count() > 0)
            {
                foreach (Border item in Guess.Children)
                {
                    if (Grid.GetColumn(item) == word.Count() - 1 && Grid.GetRow(item) == tries)
                        ((TextBlock)item.Child).Text = "";
                }
                word.RemoveAt(word.Count() - 1);
            }
            else if (e.Key == Key.Enter && word.Count() == answer.Length && dictionary.Spell(new string(word.ToArray())))
            {
                for (int i = 0; i < answer.Length; i++)
                {
                    foreach (Border item in Guess.Children)
                    {
                        if (Grid.GetColumn(item) == i && Grid.GetRow(item) == tries && answer[i] == word[i])
                            ((TextBlock)item.Child).Background = Brushes.Green;
                        else if (Grid.GetColumn(item) == i && Grid.GetRow(item) == tries && answer.Contains(word[i]))
                            ((TextBlock)item.Child).Background = Brushes.Yellow;
                        else if (Grid.GetColumn(item) == i && Grid.GetRow(item) == tries && !answer.Contains(word[i]))
                            ((TextBlock)item.Child).Background = Brushes.Red;
                    }
                }
                tries++;
                if (answer.Equals(new string(word.ToArray())))
                {
                    MessageBox.Show("You Won");
                }
                else if (tries == 6)
                {
                    MessageBox.Show("You Lost");
                }
                word.Clear();
            }
        }
    }
}
