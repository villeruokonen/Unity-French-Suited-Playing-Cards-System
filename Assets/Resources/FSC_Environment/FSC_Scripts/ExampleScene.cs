using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FSC_Environment;

public class ExampleScene : MonoBehaviour
{
    private Deck _deck;
    private List<GameObject> _cardObjects;
    void Start()
    {
        transform.Find("Buttons/FillDeck").GetComponent<Button>().onClick.AddListener(() => FillDeck());
        transform.Find("Buttons/Shuffle").GetComponent<Button>().onClick.AddListener(() => ShuffleDeck());

        _cardObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_deck == null)
        {
            transform.Find("Buttons/Shuffle").GetComponent<Button>().interactable = false;
            return;
        }
        else transform.Find("Buttons/Shuffle").GetComponent<Button>().interactable = true;
    }

    void FillDeck()
    {
        _deck = new(false, false);

        MakeCardObjects();

        transform.Find("Buttons/FillDeck").GetComponent<Button>().interactable = false;
    }

    void ShuffleDeck()
    {
        _deck.ShuffleDeck();

        foreach (var obj in _cardObjects)
        {
            Destroy(obj);
        }

        MakeCardObjects();
    }

    void MakeCardObjects()
    {
        _cardObjects = new List<GameObject>(_deck.Cards.Count);

        for (var i = 0; i < _deck.Cards.Count; i++)
        {
            GameObject cardObj = new GameObject(_deck.Cards[i].ToString());
            cardObj.transform.parent = transform.Find("Card Scroll View/Viewport/Content");
            var img = cardObj.AddComponent<Image>();
            img.sprite = _deck.Cards[i].Sprite;
            _cardObjects.Add(cardObj);
        }
    }
}

