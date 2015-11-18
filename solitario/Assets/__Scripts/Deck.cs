using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Deck : MonoBehaviour {
	// Suits
	public Sprite suitClub;
	public Sprite suitDiamond;
	public Sprite suitHeart;
	public Sprite suitSpade;
	public Sprite[] faceSprites;
	public Sprite[] rankSprites;
	public Sprite cardBack;
	public Sprite cardBackGold;
	public Sprite cardFront;
	public Sprite cardFrontGold;
	// Prefabs
	public GameObject prefabSprite;
	public GameObject prefabCard;
	public bool ________________;
	public PT_XMLReader xmlr;
	public List<string> cardNames;
	public List<Card> cards;
	public List<Decorator> decorators;
	public List<CardDefinition> cardDefs;
	public Transform deckAnchor;
	public Dictionary<string,Sprite> dictSuits;

	// InitDeck is called by Prospector when it is ready
	public void InitDeck(string deckXMLText) {
		ReadDeck(deckXMLText);
	}
	
	// ReadDeck parses the XML file passed to it into CardDefinitions
	public void ReadDeck(string deckXMLText) {
		xmlr = new PT_XMLReader (); // Create a new PT_XMLReader
		xmlr.Parse (deckXMLText); // Use that PT_XMLReader to parse DeckXML
		// Read decorators for all Cards
		decorators = new List<Decorator> (); // Init the List of Decorators
		// Grab a PT_XMLHashList of all <decorator>s in the XML file
		PT_XMLHashList xDecos = xmlr.xml ["xml"] [0] ["decorator"];
		Decorator deco;
		for (int i=0; i<xDecos.Count; i++) {
			// For each <decorator> in the XML
			deco = new Decorator (); // Make a new Decorator
			// Copy the attributes of the <decorator> to the Decorator
			deco.type = xDecos [i].att ("type");
			// Set the bool flip based on whether the text of the attribute is
			// "1" or something else. This is an atypical but perfectly fine
			// use of the == comparison operator. It will return a true or
			// false, which will be assigned to deco.flip.
			deco.flip = (xDecos [i].att ("flip") == "1");
			// floats need to be parsed from the attribute strings
			deco.scale = float.Parse (xDecos [i].att ("scale"));
			// Vector3 loc initializes to [0,0,0], so we just need to modify it
			deco.loc.x = float.Parse (xDecos [i].att ("x"));
			deco.loc.y = float.Parse (xDecos [i].att ("y"));
			deco.loc.z = float.Parse (xDecos [i].att ("z"));
			// Add the temporary deco to the List decorators
			decorators.Add (deco);
		}
		// Read pip locations for each card number
		cardDefs = new List<CardDefinition> (); // Init the List of Cards
		PT_XMLHashList xCards = xmlr.xml ["xml"] [0] ["card"];
		CardDefinition card;
		for (int i = 0; i< xCards.Count; i++){
			card = new CardDefinition();
			card.pips = new List<Decorator>();
			Decorator pip;
			if (xCards[i]["pip"] != null)
			for (int j = 0; j < xCards[i]["pip"].Count; j++){
				pip = new Decorator();
				pip.type = "pip";
				pip.flip = (xCards [i]["pip"][j].att ("flip") == "1");
				if( xCards [i]["pip"][j].ContainsAtt("scale"))
					pip.scale = float.Parse (xCards [i]["pip"][j].att ("scale"));
				pip.loc.x = float.Parse (xCards [i]["pip"][j].att ("x"));
				pip.loc.y = float.Parse (xCards [i]["pip"][j].att ("y"));
				pip.loc.z = float.Parse (xCards [i]["pip"][j].att ("z"));
				card.pips.Add(pip);
			}
			if( xCards [i].ContainsAtt("face"))
				card.face = xCards [i].att ("face");
			card.rank = int.Parse (xCards [i].att ("rank")); 
		}
	}


}