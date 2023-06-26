using UnityEngine;

namespace FSC_Environment
{
    /// <summary>
    /// A card object contains its value and suit; these default to 1 and Spades (ace of spades) if the value and suit
    /// aren't specified at creation time. Some helper functions, static and object-specific, are included.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// The card's value.
        /// </summary>
        public int Value { get { return _value; } private set { } }
        private int _value;

        /// <summary>
        /// The card's suit.
        /// </summary>
        public Suit CardSuit { get { return _suit; } private set { } }
        /// <summary>
        /// Loads and returns the correct sprite, if found, for this particular instance of Card.
        /// </summary>
        private Suit _suit;

        public Sprite Sprite { get { return FindSpriteForCard(this); } private set { } }
        
        /// <summary>
        /// The available suits: clubs, diamonds, hearts, spades, and the special Joker "suit".
        /// </summary>
        public enum Suit
        {
            Clubs,
            Diamonds,
            Hearts,
            Spades,
            Joker
        }
        public Card()
        {
            _value = 1;
            _suit = Suit.Spades;
        }
        public Card(int value, Suit suit)
        {
            _value = value;
            _suit = suit;
        }

        // A "special" card with "no value" that isn't part of any of the 4 suits.
        public static Card Joker = new Card(0, Suit.Joker);

        /// <summary> The style used in ToString(). </summary>
        public enum NameStyle
        {
            /// <summary> eg. "Three Of Diamonds" </summary>
            Full,
            /// <summary> eg. "3oD" </summary>
            Abbreviated,
            /// <summary> eg. "Three" </summary>
            ValueOnly,
        }

        public override string ToString()
        {
            return ToString(NameStyle.Full);
        }

        /// <summary> Returns a string of the card's value and suit in a given style. </summary>
        public string ToString(NameStyle style)
        {
            string suit = "?";
            string value = "?";

            switch (_value)
            {
                case 1:
                    value = "Ace";
                    break;
                case 2:
                    value = "Deuce";
                    break;
                case 3:
                    value = "Three";
                    break;
                case 4:
                    value = "Four";
                    break;
                case 5:
                    value = "Five";
                    break;
                case 6:
                    value = "Six";
                    break;
                case 7:
                    value = "Seven";
                    break;
                case 8:
                    value = "Eight";
                    break;
                case 9:
                    value = "Nine";
                    break;
                case 10:
                    value = "Ten";
                    break;
                case 11:
                    value = "Jack";
                    break;
                case 12:
                    value = "Queen";
                    break;
                case 13:
                    value = "King";
                    break;
            }

            if (style == NameStyle.ValueOnly) { return value; }

            switch (_suit)
            {
                case Suit.Spades:
                    suit = "Spades";
                    break;
                case Suit.Diamonds:
                    suit = "Diamonds";
                    break;
                case Suit.Hearts:
                    suit = "Hearts";
                    break;
                case Suit.Clubs:
                    suit = "Clubs";
                    break;
            }

            if (style == NameStyle.Abbreviated)
            {
                switch (_value)
                {
                    case 1:
                        value = "A";
                        break;
                    case 11:
                        value = "J";
                        break;
                    case 12:
                        value = "Q";
                        break;
                    case 13:
                        value = "K";
                        break;
                    default:
                        value = _value.ToString();
                        break;
                }
                return value + "o" + suit[0];
            }

            return value + " of " + suit;
        }

        /// <summary>
        /// Returns 1 if a's value is larger than b's, 0 if they are equal, and -1 if a's value is smaller than b's.
        /// If one of the cards is a Joker, returns 2.
        /// </summary>
        public static int Compare(Card a, Card b)
        {
            if (a.CardSuit == Suit.Joker || b.CardSuit == Suit.Joker) { return 2; }
            if (a.Value > b.Value) { return 1; }
            else if (a.Value == b.Value) { return 0; }
            else if (a.Value < b.Value) { return -1; }

            // This condition should never be reached since the field Value is int (non-nullable).
            else throw new UnityException("One or more of the 2 cards used in Card.Compare(a,b) was null");
        }

        /// <summary>
        /// Returns the correct sprite, if found, for a given card.
        /// </summary>
        public static Sprite FindSpriteForCard(Card card)
        {
            string suit = "";
            string value = "";

            // If card is a Joker, just find the singular Joker texture.
            if (card.CardSuit == Suit.Joker)
            {
                return GetCardSprite("JOKER");
            }

            switch (card.Value)
            {
                case 1:
                    value = "A"; // A = ace
                    break;
                case 11:
                    value = "J"; // jack
                    break;
                case 12:
                    value = "Q"; // queen
                    break;
                case 13:
                    value = "K"; // king
                    break;
                default:
                    value = card.Value.ToString();
                    break;
            }

            switch (card.CardSuit)
            {
                case Card.Suit.Spades:
                    suit = "S";
                    break;
                case Card.Suit.Diamonds:
                    suit = "D";
                    break;
                case Card.Suit.Hearts:
                    suit = "H";
                    break;
                case Card.Suit.Clubs:
                    suit = "C";
                    break;
            }

            return GetCardSprite(value + suit);
        }

        private static Sprite GetCardSprite(string cardAbbr)
        {
            // For whatever reason, Resources.Load seems to work best for loading sprites when you both
            // specify the type as <Sprite> AND also cast it as Sprite
            var tex = Resources.Load<Sprite>($"FSC_Environment/FSC_CardSprites/{cardAbbr}") as Sprite;

            // Best-case scenario: we don't have a texture for this card - maybe it's a 15 of Michaels or something.
            if (tex == null) { tex = Resources.Load<Sprite>($"FSC_Environment/FSC_CardSprites/BLANK") as Sprite; }

            // Worst-case scenario: we can't even find a placeholder texture to return!
            if (tex == null)
            {
                throw new UnityException(
                    $"Can't find BLANK.png in CardSprites! " +
                    "Is Assets/Resources/FSC_Environment/FSC_CardSprites a valid path?"
                );
            }

            return tex;
        }
    }
}
