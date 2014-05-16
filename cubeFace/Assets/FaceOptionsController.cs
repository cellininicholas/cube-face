using UnityEngine;
using System.Collections;

public class FaceOptionsController : MonoBehaviour {

	public GameObject face1;
	public GameObject face2;
	public GameObject face3;
	public GameObject face4;

	public void setAlpha (float a) {
		Debug.Log ("SetAlpha: " + a);

		CubeFaceController faceController = (CubeFaceController) face1.GetComponent(typeof(CubeFaceController));
		faceController.setAlpha (a);

		faceController = (CubeFaceController) face2.GetComponent(typeof(CubeFaceController));
		faceController.setAlpha (a);

		faceController = (CubeFaceController) face3.GetComponent(typeof(CubeFaceController));
		faceController.setAlpha (a);

		faceController = (CubeFaceController) face4.GetComponent(typeof(CubeFaceController));
		faceController.setAlpha (a);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
