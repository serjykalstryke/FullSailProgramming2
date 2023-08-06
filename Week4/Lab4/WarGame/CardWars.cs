using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarGame
{
    public class CardWars
    {

        static Random rando = new Random();
        static List<string> Shuffle(List<string> cards)
        {
            return cards.OrderBy(x => rando.Next()).ToList();
        }

        private static int CompareCards(string card1, string card2)
        {
            int pValue = GetCardValue(card1);
            int cValue = GetCardValue(card2);
            if (pValue > cValue)
                return 1;//player wins
            else if (pValue < cValue)
                return -1;//npc wins 

            return 0;
        }

        private static int GetCardValue(string card)
        {
            int value;
            if (card.Length == 3) //10 card
                value = 10;
            else
            {
                string face = card[0].ToString();
                if (face == "A")
                    value = 1;
                else if (face == "J")
                    value = 11;
                else if (face == "Q")
                    value = 12;
                else if (face == "K")
                    value = 13;
                else
                    value = int.Parse(face);
            }

            return value;
        }
    }
}
