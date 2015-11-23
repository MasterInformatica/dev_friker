using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Prospector : MonoBehaviour {
	static public Prospector    S;
	
	public Deck                 deck;
	public TextAsset            deckXML;

    public Layout layout;
    public TextAsset layoutXML;

	void Awake() {
		S = this; // Set up a Singleton for Prospector
	}
	
	void Start () {
		deck = GetComponent<Deck>(); // Get the Deck
		deck.InitDeck(deckXML.text); // Pass DeckXML to it
		Deck.Shuffle(ref deck.cards);    // This shuffles the deck

        // The ref keyword passes a reference to deck.cards, which allows
        // deck.cards to be modified by Deck.Shuffle()
        layout = GetComponent<Layout>(); // Get the Layout
        layout.ReadLayout(layoutXML.text); // Pass LayoutXML to it
	}
}

