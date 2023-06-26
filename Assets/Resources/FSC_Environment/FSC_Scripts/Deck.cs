using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace FSC_Environment
{
    /// <summary>
    /// A deck contains cards, internally in a List collection.
    /// Some useful object-specific functions are included, such as DrawCard() and Shuffle().
    /// A Deck can also be used for eg. a player's hand - helper functions such as TransferCard() allow
    /// drawing from one deck to another
    /// </summary>
    public class Deck
    {
        /// <summary>
        /// Main collection.
        /// Do not modify directly; use wrapper methods such as AddCard, RemoveCard etc.
        /// </summary>
        public ReadOnlyCollection<Card> Cards => _cards.AsReadOnly();
        private List<Card> _cards;
        public int TotalValue { get { return GetTotalValue(); } private set {}}
        /// <summary>
        /// Creates a new empty Deck and initializes its internal collection.
        /// </summary>
        public Deck()
        {
            _cards = new();
        }

        /// <summary>
        /// Creates a new Deck object and fills it with 52 cards, or 54 if useJokers is true, in a rising order from ace to king, for each suit.
        /// </summary>
        /// <param name="useJokers">Should the deck contain two Joker cards?</param>
        /// <param name="shuffleDeck">Should the deck be shuffled using the internal ShuffleDeck() function before return?</param>
        public Deck(bool useJokers = false, bool shuffleDeck = true)
        {
            int maxCount = useJokers ? 54 : 52;
            _cards = new(maxCount);

            for (byte suit = 1; suit <= 4; suit++)
            {
                Card.Suit curSuit = Card.Suit.Clubs;
                switch (suit)
                {
                    case 1:
                        // curSuit starts set to 1, proceed
                        break;
                    case 2:
                        curSuit = Card.Suit.Diamonds;
                        break;
                    case 3:
                        curSuit = Card.Suit.Hearts;
                        break;
                    case 4:
                        curSuit = Card.Suit.Spades;
                        break;
                }

                for (int i = 1; i <= 13; i++)
                {
                    _cards.Add(new Card(i, curSuit));
                }
            }

            if (useJokers)
            {
                _cards.Add(Card.Joker);
                _cards.Add(Card.Joker);
            }

            if (shuffleDeck)
            {
                ShuffleDeck();
            }
        }

        /// <summary>
        /// Creates a new Deck object and fills it with the given collection.
        /// </summary>
        /// <param name="cards">The collection to fill the deck with.</param>
        public Deck(IEnumerable<Card> cards)
        {
            _cards = new();
            foreach(Card c in cards)
            {
                _cards.Add(c);
            }
        }

        /// <summary>
        /// Add a card to the deck.
        /// </summary>
        /// <param name="card">The card to add to the deck.</param>
        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        /// <summary>
        /// Add a collection of cards to the deck.
        /// </summary>
        /// <param name="card">The collection of cards to add to the deck.</param>
        public void AddCards(IEnumerable<Card> cards)
        {
            _cards.AddRange(cards);
        }

        /// <summary>
        /// Removes a given card from the deck, if it exists in the deck.
        /// </summary>
        /// <param name="card"> The card to remove.</param>
        public void RemoveCard(Card card)
        {
            if(_cards.Contains(card))
            {
                var i = _cards.IndexOf(card);
                _cards.RemoveAt(i);
            }
            //else throw new System.Exception($"{card} does not exist in the given collection {_cards} but its removal from it was attempted.");
        }

        /// <summary>
        /// Returns a copy of the last card in the deck's collection.
        /// </summary>
        public Card GetLast()
        {
            Card c = _cards[_cards.Count - 1];
            return c;
        }

        /// <summary>
        /// Returns a copy of the first card in the deck's collection.
        /// </summary>
        public Card GetFirst()
        {
            Card c = _cards[0];
            return c;
        }

        /// <summary>
        /// Returns a copy of the last card in the deck's collection, and removes the original from the deck.
        /// </summary>
        public Card DrawLast()
        {
            Card c = _cards[_cards.Count - 1];
            _cards.Remove(c);
            return c;
        }
        /// <summary>
        /// Returns a copy of the first card in the deck's collection, and removes the original from the deck.
        /// </summary>
        public Card DrawFirst()
        {
            Card c = _cards[0];
            _cards.Remove(c);
            return c;
        }
        
        /// <summary>
        /// Returns true if a given card exists in the deck, and false when not.
        /// </summary>
        /// <param name="card"> The card to look for.</param>
        public bool IsCardInDeck(Card card)
        {
            return _cards.Contains(card);
        }

        /// <summary>
        /// Returns the number of cards of the given value in the deck.
        /// </summary>
        /// <param name="value"> The value to look for the number of.</param>
        public int NumCardsOfValue(int value)
        {
            int num = 0;
            for (var i = 0; i < _cards.Count; i++)
            {
                Card c = _cards[i];
                if (c.Value != value) { continue; }
                num++;
            }

            return num;
        }

        /// <summary>
        /// Shuffles the deck using the Fisher-Yates shuffle algorithm, RNG seeded with the current minute's second.
        /// </summary>
        public void ShuffleDeck()
        {
            // A deck with only one or no cards cannot be shuffled...
            if (_cards.Count < 2) { return; }

            // Seeding the RNG class with whatever the current time's second is.
            System.Random rng = new System.Random(System.DateTime.Now.Second);

            // Shuffling with the Fisher-Yates shuffle.
            int n = _cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = _cards[k];
                _cards[k] = _cards[n];
                _cards[n] = value;
            }
        }

        /// <summary>
        /// Transfer a card from one deck to another. Returns true if the transfer was succesful.
        /// </summary>
        /// <param name="from"> The deck to transfer the card from.</param>
        /// <param name="to"> The deck to transfer the card to.</param>
        /// <param name="card"> The card to transfer.</param>
        public static bool TransferCard(Deck from, Deck to, Card card)
        {
            Card c = card;
            if(from.Cards.Contains(c))
            {
                from.RemoveCard(c);
                to.AddCard(c);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the sum total of the values of each card in the deck.
        /// </summary>
        private int GetTotalValue()
        {
            int value = 0;

            for(var i = 0; i < _cards.Count; i++)
            {
                value += _cards[i].Value;
            }

            return value;
        }
    }
}
