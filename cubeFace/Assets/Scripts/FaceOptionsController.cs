using UnityEngine;
using System.Collections;

public class FaceOptionsController : MonoBehaviour {

	public GameObject face1;
	public GameObject face2;
	public GameObject face3;
	public GameObject face4;

	public void setAlphaWithRotationOffset (float a, int rotOffset) {
		//Debug.Log ("SetAlpha: " + a);
		//Debug.Log (rotOffset);

		CubeFaceController faceController1 = (CubeFaceController) face1.GetComponent(typeof(CubeFaceController));
		faceController1.setAlpha (0);
		
		CubeFaceController faceController2 = (CubeFaceController) face2.GetComponent(typeof(CubeFaceController));
		faceController2.setAlpha (0);
		
		CubeFaceController faceController3 = (CubeFaceController) face3.GetComponent(typeof(CubeFaceController));
		faceController3.setAlpha (0);
		
		CubeFaceController faceController4 = (CubeFaceController) face4.GetComponent(typeof(CubeFaceController));
		faceController4.setAlpha (0);

		if (rotOffset == 0) {
			faceController4.setAlpha (a);
			faceController1.setAlpha (a);
		} else if (rotOffset == 90) {
			faceController3.setAlpha (a);
			faceController4.setAlpha (a);
		} else if (rotOffset == 180) {
			faceController2.setAlpha (a);
			faceController3.setAlpha (a);
		} else if (rotOffset == 270) {
			faceController3.setAlpha (a);
			faceController4.setAlpha (a);
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
