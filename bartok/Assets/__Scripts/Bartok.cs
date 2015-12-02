using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bartok : MonoBehaviour {
	static public Bartok    S;

    public GameObject prefabWin;
    public GameObject prefabLose;
	
	public Deck                 deck;
	public TextAsset            deckXML;

    public Layout layout;
    public TextAsset layoutXML;

    public Vector3 layoutCenter;
    public float xOffset = 3;
    public float yOffset = -2.5f;
    public Transform layoutAnchor;
    public CardBartok target;
    public List< List<CardBartok> > hands;
    public List<CardBartok> discardPile;
    public List<CardBartok> drawPile;
	public float gameRestartDelay = 6f;

	void Awake() {
		S = this; // Set up a Singleton for Bartok
	}
	
	void Start () {
		deck = GetComponent<Deck>(); // Get the Deck
		deck.InitDeck(deckXML.text); // Pass DeckXML to it
		Deck.Shuffle(ref deck.cards);    // This shuffles the deck
        
        // The ref keyword passes a reference to deck.cards, which allows
        // deck.cards to be modified by Deck.Shuffle()
        layout = GetComponent<Layout>(); // Get the Layout
        layout.ReadLayout(layoutXML.text); // Pass LayoutXML to it

        drawPile = ConvertListCardsToListCardBartoks(deck.cards);
        LayoutGame();

	}

	public void DelayedRestart( ) {
		// Invoke the Restart() method in delay seconds
		Invoke("Restart", gameRestartDelay);
	}
	public void Restart() {
		// Reload _Scene_0 to restart the game
		Application.LoadLevel("__Bartok_Scene_0");
	}
	private void Win(){
        Instantiate(prefabWin);
		DelayedRestart ();
	}
	private void Lose(){

        Instantiate(prefabLose);
		DelayedRestart ();
	}

    // The Draw function will pull a single card from the drawPile and return it
    public CardBartok Draw()
    {
        CardBartok cd = drawPile[0]; // Pull the 0th CardBartok
        drawPile.RemoveAt(0); // Then remove it from List<> drawPile
        return (cd); // And return it
    }
    // LayoutGame() positions the initial tableau of cards, a.k.a. the "mine"
    void LayoutGame()
    {
        // Create an empty GameObject to serve as an anchor for the tableau
        if (layoutAnchor == null)
        {
            GameObject tGO = new GameObject("_LayoutAnchor");
            // ^ Create an empty GameObject named _LayoutAnchor in the Hierarchy
            layoutAnchor = tGO.transform; // Grab its Transform
            layoutAnchor.transform.position = layoutCenter; // Position it
        }
        CardBartok cp;
        // Follow the layout
        foreach (SlotDef tSD in layout.slotDefs)
        {
            // ^ Iterate through all the SlotDefs in the layout.slotDefs as tSD
            cp = Draw(); // Pull a card from the top (beginning) of the drawPile
            cp.faceUp = tSD.faceUp; // Set its faceUp to the value in SlotDef
            cp.transform.parent = layoutAnchor; // Make its parent layoutAnchor
            // This replaces the previous parent: deck.deckAnchor, which appears
            // as _Deck in the Hierarchy when the scene is playing.
            cp.transform.localPosition = new Vector3(
                                                layout.multiplier.x * tSD.x,
                                                layout.multiplier.y * tSD.y,
                                                -tSD.layerID);
            // ^ Set the localPosition of the card based on slotDef
            cp.layoutID = tSD.id;
            cp.slotDef = tSD;
            cp.state = CardState.tableau;
			//cp.hiddenBy = List<CardBartok>();
			//foreach(int tapa in tSD.hiddenBy){
			//	cp.hiddenBy.Add(
			//}
            cp.SetSortingLayerName(tSD.layerName); // Set the sorting layers
            tableau.Add(cp); // Add this CardBartok to the List<> tableau
        }
		for( int i = 0; i < layout.slotDefs.Count; i++) {
			foreach( int tapa in layout.slotDefs[i].hiddenBy){
				tableau[i].hiddenBy.Add(tableau[tapa]);
			}
		}
		SlotDef dP = layout.discardPile;
		cp = Draw ();
		cp.faceUp = true;
		cp.layoutID = dP.id;
		cp.transform.parent = layoutAnchor;
		cp.transform.localPosition = new Vector3 (dP.x,dP.y,-dP.layerID);
		cp.slotDef = dP;
		cp.state = CardState.target;
		cp.SetSortingLayerName (dP.layerName);
		discardPile.Add (cp);
		target = cp;

		dP = layout.drawPile;
        int N = 0;
        foreach (CardBartok c in drawPile)
        {
            c.faceUp = dP.faceUp;
            c.transform.parent = layoutAnchor;
            c.transform.localPosition = new Vector3(
                                                dP.x + dP.stagger.x * N,
                                                dP.y + dP.stagger.y * N, 
                                                -dP.layerID);
            c.layoutID = dP.id;
            c.slotDef = dP;
            c.state = CardState.drawpile;
            c.SetSortingLayerName(dP.layerName);
            N++;
        }
    }

	public void newTarget(CardBartok card){
		target.state = CardState.discard;
		target.SetSortingLayerName ("discard");
		if (card.state == CardState.tableau)
			tableau.Remove (card);
		else if (card.state == CardState.drawpile)
			drawPile.Remove (card);
		card.faceUp = true;
		card.layoutID = layout.discardPile.id;
		card.transform.parent = layoutAnchor;
		card.transform.localPosition = new Vector3 (layout.discardPile.x,layout.discardPile.y,-layout.discardPile.layerID);
		card.slotDef = layout.discardPile;
		card.state = CardState.target;
		card.SetSortingLayerName (layout.discardPile.layerName);
		discardPile.Add (card);
		target = card;
        if (tableau.Count == 0)
        {
            Puntos.S.Totaliza();
            Win();
        }
        else if (drawPile.Count == 0)
        {
            bool lose = true;
            foreach (CardBartok c in tableau)
            {
                if (c.faceUp && possibleTarget(c))
                {
                    lose = false;
                    break;
                }
            }
            if (lose)
            {
                Puntos.S.Totaliza();
                Lose();
            }
        }
	}

	public void oneLessTableau(CardBartok card){

		foreach (CardBartok c in tableau) {
			c.hiddenBy.Remove(card);
			if (c.hiddenBy.Count == 0)
				c.faceUp = true;
		}
	}

	public bool possibleTarget(CardBartok card){
		int dis = target.rank - card.rank;
		if (dis == 1 || dis == -1 || dis == 12 || dis == -12)
			return true;
		return false;
	}

    List<CardBartok> ConvertListCardsToListCardBartoks(List<Card> lCD)
    {
        List<CardBartok> lCP = new List<CardBartok>();
        CardBartok tCP;
        foreach (Card tCD in lCD)
        {
            tCP = tCD as CardBartok;
            lCP.Add(tCP);
        }
        return (lCP);
    }
}

