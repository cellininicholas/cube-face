using UnityEngine;
using System.Collections;

public enum WallMode : int { 
	Invisible, Floor, FullLength, ToFullLength, ToFloor, ToInvisible
};

public class WallBarsController : MonoBehaviour {

	// PUBLIC
	public GameObject[] wallRows;

	// PRIVATE
	private ColumnRowController[] _wallBarControllers;
	private GameObject[,] _wallBars;

	public WallType initWallType;
	private WallPattern _wallPattern;

	// ANIMATION
	private const float FLOOR_PERC = 0.18f;
	private const float MAX_COL_HEIGHT = 2.0f;

	private WallMode _wallMode; // Helps update _cascadePercent

	private float _pulseTimeOut;
	private float[,] _colsGlowPercent;  // The glow of all the columns
	private bool[,] _preUpdateColsGlowPassed;
	private bool[,] _colsGlowPassed;

	// Use this for initialization
	void Start () {
		_wallBars = new GameObject[wallRows.Length , wallRows.Length];
		_wallBarControllers = new ColumnRowController[wallRows.Length];

		// Get the Controllers
		for (int i=0; i < wallRows.Length; ++i) {
			_wallBarControllers[i] = wallRows[i].GetComponent<ColumnRowController>();

			for (int j=0; j < _wallBarControllers.Length; ++j) {
				_wallBars[i,j] = _wallBarControllers[i].columnParent[j];
			}
		}

		//setWallBarsToInterestingHeights ();

		_wallPattern = new WallPattern (initWallType);
		
		// SETUP INITIAL WALL GLOW
		_pulseTimeOut = 0;
		_colsGlowPercent = new float[wallRows.Length , wallRows.Length];
		_colsGlowPassed = new bool[wallRows.Length , wallRows.Length];
		_preUpdateColsGlowPassed = new bool[wallRows.Length , wallRows.Length];
		for (int i=0; i < wallRows.Length; ++i) {
			for (int j=0; j < wallRows.Length; ++j) {
				_colsGlowPassed[i,j] = true;
				_preUpdateColsGlowPassed[i,j] = true;

				if (_wallPattern.pattern[i,j]) {
					_colsGlowPercent[i,j] = 0;
				} else {
					_colsGlowPercent[i,j] = 0;
				}
			}
		}

		setCascadePercent (0.0f);

		// ANIMATION
		_wallMode = WallMode.Floor;
		//_colsGlowPercent = 0;
	}

	public void setCascadePercent(float percent) {
		//Debug.Log ("Percent: " + percent);

		//percent = (FLOOR_PERC + percent) / (1 + FLOOR_PERC);
		percent *= (1 - FLOOR_PERC);
		percent += FLOOR_PERC;

		for (int i=0; i < wallRows.Length; ++i) {
			for (int j=0; j < wallRows.Length; ++j) {
				GameObject obj = _wallBars[i,j];

				if (_wallPattern.pattern[i,j]) {
					if (percent >= FLOOR_PERC) {
						Vector3 scale = obj.transform.localScale;
						scale.y = percent * MAX_COL_HEIGHT;
						obj.transform.localScale = scale;
						
						obj.SetActive(true);
					} else {
						obj.SetActive(false);
					}
				} else {
					obj.SetActive(false);
				}
			}
		}
	}

	/*
	 *  GLOW
	 * */

	bool indicesAreWithArrayRange (int i, int j) {
		return (i >= 0 && i < wallRows.Length && j >= 0 && j < wallRows.Length);
	}

	void reduceGlowAtIndex (int i, int j) {
		// Glow doesn't last forever :(
		_colsGlowPercent[i,j] = (_colsGlowPercent[i,j] * 0.9f);
		if (_colsGlowPercent[i,j] <= 0.001f) _colsGlowPercent[i,j] = 0;
	}

	void floodAdjacentBlocksAndReduceGlow (int i, int j) {
		if (_colsGlowPassed [i, j] == false) {
			int tmpI = i - 1;
			int tmpJ = j;
			if (indicesAreWithArrayRange(tmpI, tmpJ) &&
			    _colsGlowPassed [tmpI, tmpJ]) {
				_colsGlowPercent[tmpI, tmpJ] = 1.111f;
				_colsGlowPassed [tmpI, tmpJ] = false;
			}

			tmpI = i + 1;
			tmpJ = j;
			if (indicesAreWithArrayRange(tmpI, tmpJ) &&
			    _colsGlowPassed [tmpI, tmpJ]) {
				_colsGlowPercent[tmpI, tmpJ] = 1.111f;
				_colsGlowPassed [tmpI, tmpJ] = false;
			}

			tmpI = i;
			tmpJ = j - 1;
			if (indicesAreWithArrayRange(tmpI, tmpJ) &&
			    _colsGlowPassed [tmpI, tmpJ]) {
				_colsGlowPercent[tmpI, tmpJ] = 1.111f;
				_colsGlowPassed [tmpI, tmpJ] = false;
			}

			tmpI = i;
			tmpJ = j + 1;
			if (indicesAreWithArrayRange(tmpI, tmpJ) &&
			    _colsGlowPassed [tmpI, tmpJ]) {
				_colsGlowPercent[tmpI, tmpJ] = 1.111f;
				_colsGlowPassed [tmpI, tmpJ] = false;
			}

			//_colsGlowPassed [i, j] = true;
		}
		reduceGlowAtIndex (i, j);
	}

	void updateGlowFlood () {

		// Boots. Cats... a' Boots, Cats!
		for (int i=0; i < wallRows.Length; ++i) {
			for (int j=0; j < wallRows.Length; ++j) {
				if (_colsGlowPercent[i,j] > 0) {
					if (_preUpdateColsGlowPassed [i, j] == false && 
					    _colsGlowPercent[i,j] < 0.2f) {
						// Move the flood to adjacent Blocks
						floodAdjacentBlocksAndReduceGlow (i, j);
					} else {
						reduceGlowAtIndex (i, j);
					}
				}
			}
		}
	}

	void copyTmpGlowFlowArray () {
		for (int i=0; i < wallRows.Length; ++i) {
			for (int j=0; j < wallRows.Length; ++j) {
				_preUpdateColsGlowPassed[i,j] = _colsGlowPassed[i,j];
			}
		}
	}

	bool allColGlowHasZeroed () {
		if (_colsGlowPercent != null) {
			for (int i=0; i < wallRows.Length; ++i) {
				for (int j=0; j < wallRows.Length; ++j) {
					if (_colsGlowPercent[i,j] != 0) return false;
				}
			}
		}
		return true;
	}

	/*
	     MAIN UPDATE
	 */

	// Update is called once per frame
	void Update () {
		copyTmpGlowFlowArray ();
		updateGlowFlood ();

		if (_wallPattern.wallType == WallType.OriginCenter &&
		    _colsGlowPercent[2,2] == 0.0f &&
		    _pulseTimeOut <= 0.0f &&
		    allColGlowHasZeroed() ) {

			// START THE FLOOD!!!
			Debug.Log ("Start the FLOOD!");

			// Change the ripple flag
			for (int i=0; i < wallRows.Length; ++i) {
				for (int j=0; j < wallRows.Length; ++j) {
					_colsGlowPassed[i,j] = true;
				}
			}

			_colsGlowPassed[2,2] = false;
			_colsGlowPercent[2,2] = 1.111f;
			_pulseTimeOut = 2;
		}
		_pulseTimeOut -= Time.deltaTime;

		// TESTING GLOW PERCENTAGES
		for (int i=0; i < wallRows.Length; ++i) {
			for (int j=0; j < wallRows.Length; ++j) {
				GameObject obj = _wallBars[i,j];
				
				Vector3 scale = obj.transform.localScale;
				scale.y = FLOOR_PERC + (0.4f + _colsGlowPercent[i,j]);
				obj.transform.localScale = scale;
			}
		}
	}

	void setWallBarsToInterestingHeights () {
		for (int i=0; i < wallRows.Length; ++i) {
			for (int j=0; j < wallRows.Length; ++j) {
				GameObject obj = _wallBars[i,j];

				Vector3 scale = obj.transform.localScale;
				scale.y = ((float)i/wallRows.Length)+((float)j/wallRows.Length);
				if (scale.y == 0) {
					obj.SetActive(false);
				} else {
					obj.SetActive(true);
				}
				obj.transform.localScale = scale;
			}
		}
	}


}
