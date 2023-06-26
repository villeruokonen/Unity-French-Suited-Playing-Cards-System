# Unity-French-Suited-PlayingCards-System
A ready-to-use system for handling cards and decks, using French-suited playing cards.

#### Deck

A **Deck** contains any number of **Card**s. **Cards** can be added to, removed from, or transferred between **Deck**s.
*Extensive in-file summaries (Deck.cs, Card.cs) clarify the usage of methods.*

#### Card

A **Card** holds a **Suit** (clubs, diamonds, hearts, spades and Joker) and a **Value**. Cards with different values can
be created, but no textures for them exist - you'll have to create your own. 
*The enum* `Suit` *in* `Card.cs` *can be modified to add other suits as well.*

#### Texture/sprite loading

By default, accessing a card's Sprite field will return a sprite that is loaded using Unity's Resources.Load using the path
*Assets/Resources/FSC_Environment/FSC_Sprites/...* specified as a `const string` in `Card.cs`.
> The filenames and the lookup functionality for them in `Card.FindSpriteForCard()` follow these rules:
>    
>   - **first letter, capitalized, or number, of the card's value** (such as A for ace, 5 for five, Q for queen...)
>   - **first letter, capitalized, of the card's suit** (H for hearts, S for spades...)
>   - **Result: 5H = five of hearts**
>   - The system will load the sprite at *Assets/Resources/FSC_Environment/FSC_CardSprites/5H.png*

*You are free to relocate the folder containing the textures, but you'll have to modify the field*
```cs 
const string SPRITE_PATH
``` 
*in* `Card.cs` *to reflect your changes.*

#### Example code snippet:
```cs
    // Create and fill a new deck with the standard 52 cards, no jokers, shuffled
    Deck deck = new Deck(false, true);

    // Create an empty deck to act as the player's hand
    Deck playerHand = new Deck();

    // Get a random card from the deck
    int rand = Random.Range(0, deck.Count);
    Card c = deck[rand];

    // Transfer the random card from the deck to the player's hand
    Deck.TransferCard(deck, playerHand, c);

    // Set some UI element's sprite to use the correct sprite for this card
    someUIElement.sprite = c.Sprite;

    // Set some score counter's text to display the player's hands current total value
    someScoreCounter.text = playerHand.TotalValue;
```