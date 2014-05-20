using UnityEngine;
using System.Collections;

public class WallBarsController : MonoBehaviour {

	// PUBLIC
	public GameObject[] wallRows;

	// PRIVATE
	private ColumnRowController[] _wallBarControllers;
	private GameObject[,] _wallBars;

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

		setWallBarsToInterestingHeights ();
	}

	void setWallBarsToInterestingHeights () {
		for (int i=0; i < 8; ++i) {
			for (int j=0; j < 8; ++j) {
				GameObject obj = _wallBars[i,j];

				Vector3 scale = obj.transform.localScale;
				scale.y = ((float)i/8)+((float)j/8);
				obj.transform.localScale = scale;
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
