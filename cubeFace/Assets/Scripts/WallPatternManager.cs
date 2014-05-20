﻿using UnityEngine;
using System.Collections;

// H - denotes Horizontal
// V - deontes Vertical

public enum WallType : int { 
	None, OriginCenter, DestinationCenter,
	StraightCenter, StraightLeft, StraightRight, StraightMidLeft, StraightMidRight,
	StraightCrossWide, StraightCrossNarrow,
	CenterTurnCenter, CenterTurnLeft, CenterTurnRight, CenterTurnMidLeft, CenterTurnMidRight
};

public enum WallMode : int { 
	Invisible, Floor, ToFloor, FullLength, ToFullLength 
};

public class WallPattern {
	// IDENTIFICATION
	WallType wallType;
	public bool[,] pattern;

	// ANIMATION
	private const float FLOOR_PERC = 0.05f;

	private WallMode _wallMode;    // Helps update _cascadePercent
	private float _cascadePercent; // Gives _colsHeight(array) the staggered animation effect
	private float[,] _colsHeight;  // The heights of all the columns

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

		// ANIMATION
		_wallMode = WallMode.Floor;
		_cascadePercent = 0;
		_colsPercent = new float[8,8];
		for (int i=0; i < 8; ++i) {
			for (int j=0; j < 8; ++j) {
				_colsPercent[i,j] = FLOOR_PERC;
			}
		}

		// STRAIGHT BRIDGES
		if (type == WallType.StraightCenter) {
			pattern = new bool[8, 8] {
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{true,true,true,true,true,true,true,true},
				{true,true,true,true,true,true,true,true},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false}};
		} else if (type == WallType.StraightRight) {
			pattern = new bool[8, 8] {
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{true,true,true,true,true,true,true,true},
				{true,true,true,true,true,true,true,true},
				{false,false,false,false,false,false,false,false}};
		} else if (type == WallType.StraightLeft) {
			pattern = new bool[8, 8] {
				{false,false,false,false,false,false,false,false},
				{true,true,true,true,true,true,true,true},
				{true,true,true,true,true,true,true,true},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false}};
		} else if (type == WallType.StraightMidRight) {
			pattern = new bool[8, 8] {
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{true,true,true,true,true,true,true,true},
				{true,true,true,true,true,true,true,true},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false}};
		} else if (type == WallType.StraightMidLeft) {
			pattern = new bool[8, 8] {
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{true,true,true,true,true,true,true,true},
				{true,true,true,true,true,true,true,true},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false}};

			// STRAIGHT CROSS
		} else if (type == WallType.StraightCrossWide) {
			pattern = new bool[8, 8] {
				{false,false,false,false,false,true,true,false},
				{false,false,false,false,false,true,true,false},
				{false,false,false,false,false,true,true,false},
				{false, true, true, true, true,true,true,false},
				{false, true, true, true, true,true,true,false},
				{false, true, true,false,false,false,false,false},
				{false, true, true,false,false,false,false,false},
				{false, true, true,false,false,false,false,false}};
		} else if (type == WallType.StraightCrossNarrow) {
			pattern = new bool[8, 8] {
				{false,false,false,false, true,true,false,false},
				{false,false,false,false, true,true,false,false},
				{false,false,false,false, true,true,false,false},
				{false,false, true, true, true,true,false,false},
				{false,false, true, true, true,true,false,false},
				{false,false, true, true,false,false,false,false},
				{false,false, true, true,false,false,false,false},
				{false,false, true, true,false,false,false,false}};

			// CENTER TURN
		} else if (type == WallType.CenterTurnCenter) {
			pattern = new bool[8, 8] {
				{false,false,false, true, true,false,false,false},
				{false,false,false, true, true,false,false,false},
				{false,false,false, true, true,false,false,false},
				{ true, true, true, true, true,false,false,false},
				{ true, true, true, true, true,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false}};
		} else if (type == WallType.CenterTurnLeft) {
			pattern = new bool[8, 8] {
				{false, true, true,false,false,false,false,false},
				{false, true, true,false,false,false,false,false},
				{false, true, true,false,false,false,false,false},
				{ true, true, true,false,false,false,false,false},
				{ true, true, true,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false}};
		} else if (type == WallType. CenterTurnMidLeft) {
			pattern = new bool[8, 8] {
				{false,false, true, true,false,false,false,false},
				{false,false, true, true,false,false,false,false},
				{false,false, true, true,false,false,false,false},
				{ true, true, true, true,false,false,false,false},
				{ true, true, true, true,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false}};
		} else if (type == WallType.CenterTurnRight) {
			pattern = new bool[8, 8] {
				{false,false,false,false,false, true, true,false},
				{false,false,false,false,false, true, true,false},
				{false,false,false,false,false, true, true,false},
				{ true, true, true, true, true, true, true,false},
				{ true, true, true, true, true, true, true,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false}};
		} else if (type == WallType.CenterTurnMidRight) {
			pattern = new bool[8, 8] {
				{false,false,false,false, true, true,false,false},
				{false,false,false,false, true, true,false,false},
				{false,false,false,false, true, true,false,false},
				{ true, true, true, true, true, true,false,false},
				{ true, true, true, true, true, true,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false}};

			// ORIGIN
		} else if (type == WallType.OriginCenter) {
			pattern = new bool[8, 8] {
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false, true, true, true, true,false,false},
				{false,false, true, true, true, true, true, true},
				{false,false, true, true, true, true, true, true},
				{false,false, true, true, true, true,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false}};

			// DESTINATION
		} else if (type == WallType.DestinationCenter) {
			pattern = new bool[8, 8] {
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false, true, true, true, true,false,false},
				{ true, true, true, true, true, true,false,false},
				{ true, true, true, true, true, true,false,false},
				{false,false, true, true, true, true,false,false},
				{false,false,false,false,false,false,false,false},
				{false,false,false,false,false,false,false,false}};
		} else {
			// Initialise
			pattern = new bool[8, 8];
			for (int i=0; i < 8; ++i) {
				for (int j=0; j < 8; ++j) {
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
