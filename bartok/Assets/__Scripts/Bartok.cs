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

    public CardBartok target;
	// The number of degrees to fan each card in a hand
	public float handFanDegrees = 10f;
	public bool ________________;
	public Transform layoutAnchor;
	public List<Player>         players;
	public CardBartok           targetCard;
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

	public void newTarget(CardBartok card){
		target.state = CBState.discard;
		target.SetSortingLayerName ("discard");
		/*if (card.state == CBState.hand)
			//tablea.Remove (card);
		else */if (card.state == CBState.drawpile)
			drawPile.Remove (card);
		card.faceUp = true;
		//card.layoutID = layout.discardPile.id;
		card.transform.parent = layoutAnchor;
		card.transform.localPosition = new Vector3 (layout.discardPile.x,layout.discardPile.y,-layout.discardPile.layerID);
		card.slotDef = layout.discardPile;
		card.state = CBState.target;
		card.SetSortingLayerName (layout.discardPile.layerName);
		discardPile.Add (card);
		target = card;
        if (drawPile.Count == 0)
        {
            Puntos.S.Totaliza();
            Win();
        }
        else if (drawPile.Count == 0)
        {
            bool lose = true;
            foreach (CardBartok c in drawPile)
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
	// Position all the cards in the drawPile properly
	public void ArrangeDrawPile() {
		CardBartok tCB;
		
		for (int i=0; i<drawPile.Count; i++) {
			tCB = drawPile[i];
			tCB.transform.parent = layoutAnchor;
			tCB.transform.localPosition = layout.drawPile.pos;
			// Rotation should start at 0
			tCB.faceUp = false;
			tCB.SetSortingLayerName(layout.drawPile.layerName);
			tCB.SetSortOrder(-i*4); // Order them front-to-back
			tCB.state = CBState.drawpile;
		}
		
	}
	
	// Perform the initial game layout
	void LayoutGame() {
		// Create an empty GameObject to serve as an anchor for the tableau
		if (layoutAnchor == null) {
			GameObject tGO = new GameObject("_LayoutAnchor");
			// ^ Create an empty GameObject named _LayoutAnchor in the Hierarchy
			layoutAnchor = tGO.transform;                 // Grab its Transform
			layoutAnchor.transform.position = layoutCenter;      // Position it
		}
		
		// Position the drawPile cards
		ArrangeDrawPile();
		
		// Set up the players
		Player pl;
		players = new List<Player>();
		foreach (SlotDef tSD in layout.slotDefs) {
			pl = new Player();
			pl.handSlotDef = tSD;
			players.Add(pl);
			pl.playerNum = players.Count;
		}
		players[0].type = PlayerType.human; // Make the 0th player human
		
	}
	
	// The Draw function will pull a single card from the drawPile and return it
	public CardBartok Draw() {
		CardBartok cd = drawPile[0];     // Pull the 0th CardProspector
		drawPile.RemoveAt(0);            // Then remove it from List<> drawPile
		return(cd);                        // And return it
	}
	
	
	// This Update method is used to test adding cards to players' hands
	void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			players[0].AddCard(Draw ());
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			players[1].AddCard(Draw ());
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			players[2].AddCard(Draw ());
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			players[3].AddCard(Draw ());
		}
	}
}

