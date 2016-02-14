﻿using UnityEngine;
using System.Collections;
public class Tile : PT_MonoBehaviour {
	// HEREDADO... casi diria que no hace falta
	// public fields
	public string type;
	// Hidden private fields
	private string _tex;
	private int _height = 0;
	private Vector3 _pos;
	// Properties with get{} and set{}
	// height moves the Tile up or down. Walls have height=1
	public int height {
		get { return( _height ); }
		set {
			_height = value;
			AdjustHeight();
		}
	}
	// Sets the texture of the Tile based on a string
	// It requires LayoutTiles, so it's commented out for now
	public string tex {
		get {
			return( _tex );
		}
		set {
			_tex = value;
			name = "TilePrefab_"+_tex; // Sets the name of this GameObject
		}
	}
	// Uses the "new" keyword to replace the pos inherited from PT_MonoBehaviour
	// Without the "new" keyword, the two properties would conflict
	new public Vector3 pos {
		get { return( _pos ); }
		set {
			_pos = value;
			AdjustHeight();
		}
	}
	// Methods
	public void AdjustHeight() {
		// Moves the block up or down based on _height
		Vector3 vertOffset = Vector3.back*(_height);
		// The -0.5f shifts the Tile down 0.5 units so that it's top surface is
		// at z=0 when pos.z=0 and height=0
		transform.position = _pos+vertOffset;
	}
}