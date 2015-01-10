using UnityEngine;
using System.Collections;

// H - denotes Horizontal
// V - deontes Vertical

public enum WallType : int { 
	None, OriginCenter, DestinationCenter,
	StraightCenter, StraightLeft, StraightRight,
	StraightCrossWide, StraightCrossNarrow,
	CenterTurnCenter, CenterTurnLeft, CenterTurnRight
};

public class WallPattern {
	// IDENTIFICATION
	public WallType wallType;
	public bool[,] pattern;

	/*
	public string ToString () {
		// TODO: construct a nice string describing the Face of the cube
	}
	*/

	/*
	 * Constructor
	 * */
	public WallPattern(WallType type) {
		wallType = type;

		// STRAIGHT BRIDGES
		if (type == WallType.StraightCenter) {
			pattern = new bool[5, 5] {
				{false,false,false,false,false},
				{false,false,false,false,false},
				{ true, true, true, true, true},
				{false,false,false,false,false},
				{false,false,false,false,false}};
		} else if (type == WallType.StraightRight) {
			pattern = new bool[5, 5] {
				{false,false,false,false,false},
				{false,false,false,false,false},
				{false,false,false,false,false},
				{ true, true, true, true, true},
				{false,false,false,false,false}};
		} else if (type == WallType.StraightLeft) {
			pattern = new bool[5, 5] {
				{false,false,false,false,false},
				{ true, true, true, true, true},
				{false,false,false,false,false},
				{false,false,false,false,false},
				{false,false,false,false,false}};

			// STRAIGHT CROSS
		} else if (type == WallType.StraightCrossWide) {
			pattern = new bool[5, 5] {
				{false,false,false,false,false},
				{false,false, true, true, true},
				{false,false, true,false,false},
				{ true, true, true,false,false},
				{false,false,false,false,false}};
		} else if (type == WallType.StraightCrossNarrow) {
			pattern = new bool[5, 5] {
				{false,false,false,false,false},
				{ true, true, true,false,false},
				{false,false, true,false,false},
				{false,false, true, true, true},
				{false,false,false,false,false}};

			// CENTER TURN
		} else if (type == WallType.CenterTurnCenter) {
			pattern = new bool[5, 5] {
				{false,false, true,false,false},
				{false,false, true,false,false},
				{ true, true, true,false,false},
				{false,false,false,false,false},
				{false,false,false,false,false}};
		} else if (type == WallType.CenterTurnLeft) {
			pattern = new bool[5, 5] {
				{false, true,false,false,false},
				{false, true,false,false,false},
				{ true, true,false,false,false},
				{false,false,false,false,false},
				{false,false,false,false,false}};

		} else if (type == WallType.CenterTurnRight) {
			pattern = new bool[5, 5] {
				{false,false,false, true,false},
				{false,false,false, true,false},
				{ true, true, true, true,false},
				{false,false,false,false,false},
				{false,false,false,false,false}};

			// ORIGIN
		} else if (type == WallType.OriginCenter) {
			pattern = new bool[5, 5] {
				{false,false,false,false,false},
				{false, true, true, true,false},
				{false, true, true, true, true},
				{false, true, true, true,false},
				{false,false,false,false,false}};

			// DESTINATION
		} else if (type == WallType.DestinationCenter) {
			pattern = new bool[5, 5] {
				{false,false,false,false,false},
				{false, true, true, true,false},
				{ true, true, true, true,false},
				{false, true, true, true,false},
				{false,false,false,false,false}};
		} else {
			// Initialise
			pattern = new bool[5, 5];
			for (int i=0; i < 5; ++i) {
				for (int j=0; j < 5; ++j) {
					pattern[i,j] = false;
				}
			}
		}
	}
}

public class WallPatternManager : MonoBehaviour {

	public static WallPattern[] patterns;

	// Use this for initialization
	void Start () {
		patterns = new WallPattern[32];
		for (int i=0; i<patterns.Length; ++i) {
			patterns[i] = new WallPattern((WallType)i);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
