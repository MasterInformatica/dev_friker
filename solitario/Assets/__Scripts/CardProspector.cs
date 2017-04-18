using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// This is an enum, which defines a type of variable that only has a few
// possible named values. The CardState variable type has one of four values:
// drawpile, tableau, target, & discard
public enum CardState {
    drawpile,
    tableau,
    target,
    discard
}
public class CardProspector : Card { // Make sure CardProspector extends Card
    // This is how you use the enum CardState
    public CardState state = CardState.drawpile;
    // The hiddenBy list stores which other cards will keep this one face down
    public List<CardProspector> hiddenBy = new List<CardProspector>();
    // LayoutID matches this card to a Layout XML id if it's a tableau card
    public int layoutID;
    // The SlotDef class stores information pulled in from the LayoutXML <slot>
    public SlotDef slotDef;


	public override void OnMouseUpAsButton ()
	{
		if (state == CardState.drawpile) {
            Puntos.S.Totaliza();
			Prospector.S.newTarget(Prospector.S.Draw());
		} else if (state == CardState.tableau) {
			if (hiddenBy.Count > 0)
				return;
			if( Prospector.S.possibleTarget(this) ){
				Prospector.S.oneLessTableau(this);
                Puntos.S.addPoints();
                Prospector.S.newTarget(this);
			}
		} else {
			//base.OnMouseUpAsButton();
			return;
		}
	}

}