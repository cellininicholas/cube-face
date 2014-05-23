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

	private WallPattern _wallPattern;

	// ANIMATION
	private const float FLOOR_PERC = 0.18f;
	private const float MAX_COL_HEIGHT = 2.0f;

	private WallMode _wallMode;    // Helps update _cascadePercent
	private bool _shouldCascade;
	private float _cascadePercent; // Gives _colsHeight(array) the staggered animation effect
	private float[,] _colsPercent;  // The heights of all the columns

	// Use this for initialization
	void Start () {
		_wallBars = new GameObject[8,8];
		_wallBarControllers = new ColumnRowController[8];

		// Get the Controllers
		for (int i=0; i < wallRows.Length; ++i) {
			_wallBarControllers[i] = wallRows[i].GetComponent<ColumnRowController>();

			for (int j=0; j < _wallBarControllers.Length; ++j) {
				_wallBars[i,j] = _wallBarControllers[i].columnParent[j];
			}
		}

		//setWallBarsToInterestingHeights ();

		_wallPattern = new WallPattern (WallType.StraightCrossWide);
		
		// SETUP INITIAL WALL HEIGHTS
		_colsPercent = new float[8,8];
		for (int i=0; i < 8; ++i) {
			for (int j=0; j < 8; ++j) {
				if (_wallPattern.pattern[i,j]) {
					_colsPercent[i,j] = FLOOR_PERC;
				} else {
					_colsPercent[i,j] = 0;
				}
			}
		}
		
		updateColumHeightsUsingPercentages ();
		
		// ANIMATION
		_wallMode = WallMode.Floor;
		_cascadePercent = 0;
	}

	public void animateWallToTall (bool cascade) {
		_shouldCascade = cascade;
		if (_wallMode != WallMode.FullLength && _wallMode != WallMode.ToFullLength) {
			_wallMode = WallMode.ToFullLength;
			_cascadePercent = 0;
		}
	}

	public void animateWallToShort (bool cascade) {
		_shouldCascade = cascade;
		if (_wallMode != WallMode.Floor && _wallMode != WallMode.ToFloor) {
			_wallMode = WallMode.ToFloor;
			_cascadePercent = 0;
		}
	}

	public void animateWallToZero (bool cascade) {
		_shouldCascade = cascade;
		if (_wallMode != WallMode.Invisible && _wallMode != WallMode.ToInvisible) {
			_wallMode = WallMode.ToInvisible;
			_cascadePercent = 0;
		}
	}

	void updateColumHeightsUsingPercentages () {
		for (int i=0; i < 8; ++i) {
			for (int j=0; j < 8; ++j) {
				GameObject obj = _wallBars[i,j];

				if (_colsPercent[i,j] >= FLOOR_PERC) {
					Vector3 scale = obj.transform.localScale;
					scale.y = _colsPercent[i,j] * MAX_COL_HEIGHT;
					obj.transform.localScale = scale;

					obj.SetActive(true);
				} else {
					obj.SetActive(false);
				}
			}
		}
	}

	void updateCascadePercentage () {
		if (_wallMode >= WallMode.ToFullLength) {
			_cascadePercent += Time.deltaTime;
			if (_cascadePercent >= 1.0f) _cascadePercent = 1.0f;
		}
	}

	void updateColumnPercentages () {
		for (int i=0; i < 8; ++i) {
			for (int j=0; j < 8; ++j) {
				if (_wallPattern.pattern[i,j]) {
					float percentForEachColumn = (float)((i * 8) + j) / 64.0f;
					if (percentForEachColumn < _cascadePercent) {
						// we can change the height of this column
						if (_wallMode == WallMode.ToFullLength) {
							_colsPercent[i,j] += Time.deltaTime * 6;
							if (_colsPercent[i,j] >= 1.0f) _colsPercent[i,j] = 1.0f;
						} else if (_wallMode == WallMode.ToFloor) {
							_colsPercent[i,j] -= Time.deltaTime * 6;
							if (_colsPercent[i,j] <= FLOOR_PERC) _colsPercent[i,j] = FLOOR_PERC;
						} else if (_wallMode == WallMode.ToInvisible) {
							_colsPercent[i,j] -= Time.deltaTime * 6;
							if (_colsPercent[i,j] <= 0) _colsPercent[i,j] = 0;
						}
					}
				}
			}
		}
	}

	/*
	     MAIN UPDATE
	 */

	// Update is called once per frame
	void Update () {
		//updateUsingSinWavePercentages ();

		if (_shouldCascade && _wallMode >= WallMode.ToFullLength) {
			updateCascadePercentage ();
			updateColumnPercentages ();
		}
		updateColumHeightsUsingPercentages ();
	}

	void setWallBarsToInterestingHeights () {
		for (int i=0; i < 8; ++i) {
			for (int j=0; j < 8; ++j) {
				GameObject obj = _wallBars[i,j];

				Vector3 scale = obj.transform.localScale;
				scale.y = ((float)i/8)+((float)j/8);
				if (scale.y == 0) {
					obj.SetActive(false);
				} else {
					obj.SetActive(true);
				}
				obj.transform.localScale = scale;
			}
		}
	}

	/*
	// SIN WAVES
	void updateUsingSinWavePercentages () {
		for (int i=0; i < wallRows.Length; ++i) {
			for (int j=0; j < _wallBarControllers.Length; ++j) {
				GameObject obj = _wallBars[i,j];

				if (_wallUp [i,j]) {
					_wallPercent [i,j] += Time.deltaTime / 2;
					if (_wallPercent [i,j] >= 1.0f) {
						_wallPercent [i,j] = 1.0f;
						_wallUp [i,j] = false;
					}
				} else {
					_wallPercent [i,j] -= Time.deltaTime / 2;
					if (_wallPercent [i,j] <= 0) {
						_wallPercent [i,j] = 0.01f;
						_wallUp [i,j] = true;
					}
				}

				// UPDATE HEIGHT
				Vector3 scale = obj.transform.localScale;
				float sinResult = Mathf.Cos(_wallPercent [i,j] * Mathf.PI * 2);
				if (sinResult > 1.0f || sinResult < -1.0f) sinResult = 0;

				scale.y = (sinResult);

				if (scale.y <= 0) {
					obj.SetActive(false);
				} else {
					obj.SetActive(true);
				}

				obj.transform.localScale = scale;

			}
		}
	}
	*/

}
