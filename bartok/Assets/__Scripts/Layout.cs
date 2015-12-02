using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The SlotDef class is not a subclass of MonoBehaviour, so it doesn't need a
// separate C# file.
[System.Serializable] // This makes SlotDefs visible in the Unity Inspector pane
public class SlotDef
{
    public float x;
    public float y;
    public bool faceUp = false;
    public string layerName = "Default";
    public int layerID = 0;
    public int id;
    public List<int> hiddenBy = new List<int>();
    public string type = "slot";
    public Vector2 stagger;
}
public class Layout : MonoBehaviour {
    public PT_XMLReader xmlr; // Just like Deck, this has a PT_XMLReader
    public PT_XMLHashtable xml; // This variable is for easier xml access
    public Vector2 multiplier; // Sets the spacing of the tableau
	// SlotDef references
    public List<SlotDef> slotDefs; // All the SlotDefs for Row0-Row3
    public SlotDef drawPile;
    public SlotDef discardPile;
	public SlotDef target;
    // This holds all of the possible names for the layers set by layerID
    public string[] sortingLayerNames = new string[] { "Row0", "Row1", "Row2", "Row3", "Discard", "Draw" };
    // This function is called to read in the LayoutXML.xml file
    public void ReadLayout(string xmlText){
        xmlr = new PT_XMLReader(); // Create a new PT_XMLReader
        xmlr.Parse(xmlText);   // Use that PT_XMLReader to parse DeckXML
        xml = xmlr.xml["xml"][0];
        PT_XMLHashList xMul = xml["multiplier"];
        multiplier = new Vector2(float.Parse(xMul[0].att("x")),float.Parse(xMul[0].att("y")));

        slotDefs = new List<SlotDef>();
        PT_XMLHashList xSlots = xml["slot"];
        SlotDef sd;
        for (int i = 0; i < xSlots.Count; i++)
        {
            sd = new SlotDef();
            if (xSlots[i].HasAtt("type"))
            {
                sd.x = float.Parse(xSlots[i].att("x"));
                sd.y = float.Parse(xSlots[i].att("y"));
                sd.layerID = int.Parse(xSlots[i].att("layer"));
                sd.layerName = sortingLayerNames[sd.layerID];
                sd.type = xSlots[i].att("type");
                if (sd.type == "drawpile")
                {
                    sd.stagger.x = float.Parse(xSlots[i].att("xstagger"));
                    drawPile = sd;
                }
                else
                {
                    discardPile = sd;
                }
            }
            else
            {
                sd.x = float.Parse(xSlots[i].att("x"));
                sd.y = float.Parse(xSlots[i].att("y"));
                sd.faceUp = (int.Parse(xSlots[i].att("faceup")) == 1);
                sd.layerID = int.Parse(xSlots[i].att("layer"));
                sd.layerName = sortingLayerNames[sd.layerID];
                sd.id = int.Parse(xSlots[i].att("id"));
                sd.type = "slot";
                if (xSlots[i].HasAtt("hiddenby"))
                {
                    string[] numbers = xSlots[i].att("hiddenby").Split(',');
                    sd.hiddenBy = new List<int>();
                    foreach(string s in numbers){
                        sd.hiddenBy.Add(int.Parse(s));
                    }
                }
                slotDefs.Add(sd);
            }

        }

    }
}
