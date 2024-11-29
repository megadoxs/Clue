using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public partial class CrossWord : UserControl
    {
        private List<KeyValuePair<string, Orientation>> list = new List<KeyValuePair<string, Orientation>>() {
            new KeyValuePair<string, Orientation>("barrier", Orientation.Horizontal),
            new KeyValuePair<string, Orientation>("immigration", Orientation.Vertical),
            new KeyValuePair<string, Orientation>("corruption", Orientation.Vertical),
            new KeyValuePair<string, Orientation>("destination", Orientation.Horizontal),
            new KeyValuePair<string, Orientation>("experiences", Orientation.Vertical),
            new KeyValuePair<string, Orientation>("adventure", Orientation.Horizontal),
            new KeyValuePair<string, Orientation>("tranquility", Orientation.Horizontal),
            new KeyValuePair<string, Orientation>("ocean", Orientation.Vertical),
            new KeyValuePair<string, Orientation>("introspection", Orientation.Horizontal),
            new KeyValuePair<string, Orientation>("whaling", Orientation.Horizontal)
        };
        private List<int> x = new List<int>() { 10, 14, 16, 7, 8, 10, 0, 13, 16, 11 };
        private List<int> y = new List<int>() { 0, 0, 6, 7, 7, 10, 12, 12, 13, 15};

        private List<List<char>> words = new List<List<char>>();

        private TextBlock focus = new TextBlock();
        private List<int> correct = new List<int>();
        public CrossWord()
        {
            InitializeComponent();
            int[] x = new int[this.x.Count];
            this.x.CopyTo(x);
            int[] y = new int[this.y.Count];
            this.y.CopyTo(y);

            CrossWordsGrid.Width = 50*29;
            CrossWordsGrid.Height = 50*18;

            for (int i = 0; i < 18; i++)
            {
                CrossWordsGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < 29; i++)
            {
                CrossWordsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for(int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Key.Length; j++)
                {
                    bool t = false;
                    foreach (var _ in CrossWordsGrid.Children)
                    {
                        if (_ is Border item && Grid.GetColumn(item) == x[i] && Grid.GetRow(item) == y[i])
                        {
                            ((TextBlock)item.Child).Tag = $"{((TextBlock)item.Child).Tag},{i}";
                            t = true ;
                            break;
                        }
                    }

                    if (j == 0)
                    {
                        TextBlock number = new TextBlock();
                        number.Text = $"{i + 1}";
                        number.SetValue(Grid.RowProperty, y[i]);
                        number.SetValue(Grid.ColumnProperty, x[i]);
                        number.Margin = new Thickness(5, 2, 0, 0);
                        number.FontSize = 15;
                        CrossWordsGrid.Children.Add(number);
                    }

                    if (t)
                    {
                        if (list[i].Value == Orientation.Horizontal)
                            x[i]++;
                        else
                            y[i]++;
                        continue;
                    }

                    Border border = new Border();
                    border.SetValue(Grid.RowProperty, y[i]);
                    border.SetValue(Grid.ColumnProperty, x[i]);
                    border.SetValue(Border.BorderBrushProperty, Brushes.Black);
                    border.SetValue(Border.BorderThicknessProperty, new Thickness(1));

                    TextBlock textBlock = new TextBlock();
                    textBlock.FontSize = 50;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
                    textBlock.TextAlignment = TextAlignment.Center;
                    textBlock.Tag = i.ToString();
                    textBlock.MouseLeftButtonDown += TextBlock_MouseLeftButtonDown;

                    border.Child = textBlock;
                    CrossWordsGrid.Children.Add(border);

                    if (list[i].Value == Orientation.Horizontal)
                        x[i]++;
                    else
                        y[i]++;
                }

                words.Add(new List<char>());
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            string[] parts = focus.Tag.ToString().Split(',');
            int tag = int.Parse(parts[0]);

            int x = this.x[tag];
            int y = this.y[tag];
            int gap = words[tag].Count;
            switch (list[tag].Value)
            {
                case Orientation.Horizontal:
                    x += gap;
                    break;

                case Orientation.Vertical:
                    y += gap;
                    break;
            }

            if (e.Key >= Key.A && e.Key <= Key.Z && words[tag].Count < list[tag].Key.Length)
            {
                foreach (var _ in CrossWordsGrid.Children)
                {
                    if (_ is Border item && Grid.GetColumn(item) == x && Grid.GetRow(item) == y && ((TextBlock)item.Child).Text == "")
                    {
                        words[tag].Add(char.Parse(e.Key.ToString().ToLower()));
                        ((TextBlock)item.Child).Text = e.Key.ToString().ToUpper();
                    }
                    else if (_ is Border item2 && Grid.GetColumn(item2) == x && Grid.GetRow(item2) == y && ((TextBlock)item2.Child).Text != "")
                    {
                        words[tag].Add(char.Parse(((TextBlock) item2.Child).Text));
                        switch (list[tag].Value)
                        {
                            case Orientation.Horizontal:
                                x++;
                                break;

                            case Orientation.Vertical:
                                y++;
                                break;
                        }
                    }
                }
            }
            else if (e.Key == Key.Back && words[tag].Count > 0)
            {
                switch (list[tag].Value)
                {
                    case Orientation.Horizontal:
                        x--;
                        break;

                    case Orientation.Vertical:
                        y--;
                        break;
                }
                foreach (var _ in CrossWordsGrid.Children)
                {
                    if (_ is Border item)
                    {
                        string[] nparts = ((TextBlock)item.Child).Tag.ToString().Split(',');
                        int tag2 = int.Parse(nparts[0]);
                        int? tag3 = nparts.Length > 1 ? (int?)int.Parse(nparts[1]) : null;
                        if (Grid.GetColumn(item) == x && Grid.GetRow(item) == y && !correct.Contains(tag2) && (!tag3.HasValue || !correct.Contains(tag3.Value)))
                        {
                            ((TextBlock)item.Child).Text = "";
                            words[tag].RemoveAt(words[tag].Count() - 1);
                            break;
                        }
                        else if (Grid.GetColumn(item) == x && Grid.GetRow(item) == y && (correct.Contains(tag2) || (tag3.HasValue || correct.Contains(tag3.Value))))
                        {
                            switch (list[tag].Value)
                            {
                                case Orientation.Horizontal:
                                    x--;
                                    break;

                                case Orientation.Vertical:
                                    y--;
                                    break;
                            }
                            foreach (var __ in CrossWordsGrid.Children)
                            {
                                if (__ is Border item2)
                                {
                                    nparts = ((TextBlock)item2.Child).Tag.ToString().Split(',');
                                    tag2 = int.Parse(nparts[0]);
                                    tag3 = nparts.Length > 1 ? (int?)int.Parse(nparts[1]) : null;
                                    if (Grid.GetColumn(item2) == x && Grid.GetRow(item2) == y && !correct.Contains(tag2) && (!tag3.HasValue || !correct.Contains(tag3.Value)))
                                    {
                                        ((TextBlock)item2.Child).Text = "";
                                        words[tag].RemoveAt(words[tag].Count() - 1);
                                        words[tag].RemoveAt(words[tag].Count() - 1);
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            else if (e.Key == Key.Enter && words[tag].Count() == list[tag].Key.Length && list[tag].Key.ToLower() == new string(words[tag].ToArray()).ToLower())
            {
                x = this.x[tag];
                y = this.y[tag];
                for (int i = 0; i < list[tag].Key.Length; i++)
                {
                    foreach (var _ in CrossWordsGrid.Children)
                    {
                        if (_ is Border item && Grid.GetColumn(item) == x && Grid.GetRow(item) == y)
                            ((TextBlock)item.Child).Background = Brushes.Green;
                    }
                    switch (list[tag].Value)
                    {
                        case Orientation.Horizontal:
                            x++;
                            break;

                        case Orientation.Vertical:
                            y++;
                            break;
                    }
                }
                correct.Add(tag);
                if (correct.Count == 10)
                {
                    MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                    if (mainWindow != null)
                    {
                        mainWindow.GameWin();
                    }
                    return;
                }
                foreach (var _ in CrossWordsGrid.Children)
                {
                    int next = 0;
                    if (x < 10)
                        x++;
                    else
                        x = 1;
                    if (_ is Border item && Grid.GetColumn(item) == this.x[next] && Grid.GetRow(item) == this.y[next])
                        focus = (TextBlock)item.Child;
                }
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            focus = (TextBlock)sender;
        }
    }
}
