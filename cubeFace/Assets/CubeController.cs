using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour {

	enum Face : byte {Face1=0, Face2=1, Face3=2, Face4=3, Face5=4, Face6=5};
	enum Facing : byte {Facing1=0, Facing2=1, Facing3=2, Facing4=3};

	private Face _topFace;
	private Facing _facing;

	private Quaternion _targetRotation;

	// Use this for initialization
	void Start () {
		_topFace = Face.Face1;
		_facing = Facing.Facing1;
		_targetRotation = transform.rotation;
	}

	// VERTICAL ROTATION
	public void rotateTargetRotationUp (bool leftSide) {
		Debug.Log ("RotateUp");
		if (leftSide) {
			//_targetRotation *= Quaternion.Euler (90,0,0);
			_targetRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(90,0,0));
		} else {
			//_targetRotation *= Quaternion.Euler (0,0,90);
			_targetRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(0,0,90));
		}
	}

	public void rotateTargetRotationDown (bool leftSide) {
		Debug.Log ("RotateDown");
		if (leftSide) {
			_targetRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(-90,0,0));
			//_targetRotation *= Quaternion.Euler (-90,0,0);
		} else {
			_targetRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(0,0,-90));
			//_targetRotation *= Quaternion.Euler (0,0,-90);
		}
	}

	// HORIZONTAL ROTATION
	public void rotateTargetRotationLeft () {
		_targetRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(0,90,0));
		//_targetRotation *= Quaternion.Euler (0,90,0);
	}

	public void rotateTargetRotationRight () {
		_targetRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(0,-90,0));
		_targetRotation *= Quaternion.Euler (0,-90,0);
	}

	// GSDFGFJSDIFHGID

	void SmoothLookAt(float smooth) {
		transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * smooth);
	}

	// Update is called once per frame
	void Update () {
		SmoothLookAt (4.5f);
	}
}
