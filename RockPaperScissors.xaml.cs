﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace Clue
{
    public partial class RockPaperScissors : UserControl
    {
        public RockPaperScissors()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            Button button = sender as Button;
            string playerChoice = button.Tag.ToString();

            
            string computerChoice = GetComputerChoice();

            
            string result = DetermineWinner(playerChoice, computerChoice);

            
            PlayerChoiceText.Text = $"Your Choice: {playerChoice}";
            ComputerChoiceText.Text = $"Computer's Choice: {computerChoice}";
            ResultText.Text = $"Result: {result}";
            if (result == "You Win!")
            {
                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.GameWin();
                }
            }
            else
            {
                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.GameLose();
                }
            }

        }

        private string GetComputerChoice()
        {
            Random random = new Random();
            int choice = random.Next(3); 

            switch (choice)
            {
                case 0: return "Rock";
                case 1: return "Paper";
                case 2: return "Scissors";
                default: return "Rock";
            }
        }

        private string DetermineWinner(string player, string computer)
        {
            if (player == computer)
                return "It's a Draw!";
            else if ((player == "Rock" && computer == "Scissors") ||
                     (player == "Paper" && computer == "Rock") ||
                     (player == "Scissors" && computer == "Paper"))
                return "You Win!";
            else
                return "You Lose!";
        }
    }
}
