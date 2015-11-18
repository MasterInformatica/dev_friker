using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Card : MonoBehaviour {


}
[System.Serializable]
public class Decorator {
	// This class stores information about each decorator or pip from DeckXML
	public string type; // For card pips, type = "pip"
	public Vector3 loc; // The location of the Sprite on the Card
	public bool flip = false; // Whether to flip the Sprite vertically
	public float scale = 1f; // The scale of the Sprite
}
[System.Serializable]
public class CardDefinition {
	// This class stores information for each rank of card
	public string face; // Sprite to use for each face card
	public int rank; // The rank (1-13) of this card
	public List<Decorator> pips = new List<Decorator>(); // Pips used
	// Because decorators (from the XML) are used the same way on every card in
	// the deck, pips only stores information about the pips on numbered cards
}