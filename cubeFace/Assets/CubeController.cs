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
		//Debug.Log ("RotateUp - Left: " + leftSide);
		if (leftSide) {
			_targetRotation = Quaternion.AngleAxis(90, Vector3.back) * _targetRotation;
		} else {
			_targetRotation = Quaternion.AngleAxis(90, Vector3.right) * _targetRotation;
		}
		printTargetRotation ();
	}

	public void rotateTargetRotationDown (bool leftSide) {
		//Debug.Log ("RotateDown - Left: " + leftSide);
		if (leftSide) {
			_targetRotation = Quaternion.AngleAxis(90, Vector3.forward) * _targetRotation;
		} else {
			_targetRotation = Quaternion.AngleAxis(90, Vector3.left) * _targetRotation;
		}
		printTargetRotation ();
	}

	// HORIZONTAL ROTATION
	public void rotateTargetRotationLeft () {
		//Debug.Log ("RotateLeft");
		_targetRotation = Quaternion.AngleAxis(90, Vector3.up) * _targetRotation;

		printTargetRotation ();
	}

	public void rotateTargetRotationRight () {
		//Debug.Log ("RotateRight");
		_targetRotation = Quaternion.AngleAxis(90, Vector3.down) * _targetRotation;

		printTargetRotation ();
	}

	// GSDFGFJSDIFHGID

	void SmoothLookAt(float smooth) {
		transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * smooth);
	}

	// Update is called once per frame
	void Update () {
		SmoothLookAt (6f);
	}

	void printTargetRotation () {
		//Debug.Log ("Target Rotation: " + _targetRotation);
	}
}
