using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Clue
{
    public class Card
    {
        public static BitmapImage CardsBack = new BitmapImage(new Uri("Images/Cards/cardsBack.png", UriKind.Relative));
        public string name { get; set; }
        public BitmapImage image { get; set; }

        public Type type { get; set; }

        public Card(string name, BitmapImage image, Type type)
        {
            this.name = name;
            this.image = image;
            this.type = type;
        }

        public enum Type
        {
            None,
            Character,
            Weapon,
            Location
        }
    }
}
