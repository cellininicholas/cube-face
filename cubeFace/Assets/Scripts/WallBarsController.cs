using UnityEngine;
using System.Collections;

public class WallBarsController : MonoBehaviour {

	// PUBLIC
	public GameObject[] wallRows;

	// PRIVATE
	private ColumnRowController[] _wallBarControllers;
	private GameObject[,] _wallBars;

	// SIN WAVES
	private bool[,] _wallUp;
	private float[,] _wallPercent;

	// Use this for initialization
	void Start () {

		_wallBars = new GameObject[8,8];
		_wallBarControllers = new ColumnRowController[8];

		_wallUp = new bool[8,8];
		_wallPercent = new float[8,8];

		// Get the Controllers
		for (int i=0; i < wallRows.Length; ++i) {
			_wallBarControllers[i] = wallRows[i].GetComponent<ColumnRowController>();

			for (int j=0; j < _wallBarControllers.Length; ++j) {
				_wallBars[i,j] = _wallBarControllers[i].columnParent[j];

				_wallUp [i,j] = true;
				_wallPercent [i,j] = (float)(i + j) / 32;
				//_wallPercent [i,j] = (float)((i*8)+j) / 64.0f;
				if (_wallPercent [i,j] >= 1.0f) {
					_wallUp [i,j] = false;
				}
			}
		}

		//setWallBarsToInterestingHeights ();
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
				/*
				if (i == 0 && j == 0) {
					Debug.Log ("SinInput: " + _wallPercent [i,j] + " SinResult: " + sinResult);
				}
				*/
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
	
	// Update is called once per frame
	void Update () {
		updateUsingSinWavePercentages ();
	}
}
