using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcards
{
    public enum CardType
    {
        Unknown,
        DaveAndBusters,
        Eamuse,
        EamuseIC,
        AMPass,
        AIME,
        Nesica,
        BanaPass,
    };

    public class Card
    {
        public string Name { get; set; } //Name applied by user
        public string Serial { get; set; } //Serial of the card
        public byte[] SystemCode { get; set; } //System code; felica, null otherwise
        public byte[] Manufacturer { get; set; } //Manufacturer code; felica, null otherwise
        public string KonamiID { get; set; } //Used by konami only, null otherwise
        public CardType CardType { get; set; } //Card type; automatically detected or set by user
        public string Data { get; set; } //Buffer of the data on the card in base64

        public static CardType DetermineCardType(Card card)
        {
            //Konami serials
            if (card.Serial.StartsWith("012E"))
            {
                //Non IC felica
                if (card.Serial[4] == '4') return CardType.Eamuse;
                //IC felica
                else if (card.Serial[4] == '5') return CardType.EamuseIC;
            }

            return CardType.Unknown;
        }
    }
}
