using UnityEngine;
using System.Collections;

public class CubeFaceController : MonoBehaviour {

	public GameObject quad1;
	public GameObject quad2;
	
	public void setAlpha (float a) {
		Debug.Log ("SetAlpha: " + a);

		Color col = quad1.renderer.material.color;
		col.a = a;
		quad1.renderer.material.SetColor ("_Color", col);

		col = quad2.renderer.material.color;
		col.a = a;
		quad2.renderer.material.SetColor ("_Color", col);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
