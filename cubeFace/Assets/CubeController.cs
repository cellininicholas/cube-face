﻿using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour {

	enum Face : byte {Face1=0, Face2=1, Face3=2, Face4=3, Face5=4, Face6=5, FaceNone=6};

	public GameObject[] faces;
	private float[] _facePercent;

	private Face _topFace;
	private Face _oldTopFace;

	private Quaternion _targetRotation;

	// Use this for initialization
	void Start () {
		_topFace = Face.Face1;
		_oldTopFace = Face.Face1;
		_targetRotation = transform.rotation;

		_facePercent = new float[] { 1, 0, 0, 0, 0, 0 };
	}

	// VERTICAL ROTATION
	public void rotateTargetRotationUp (bool leftSide) {
		//Debug.Log ("RotateUp - Left: " + leftSide);
		if (leftSide) {
			_targetRotation = Quaternion.AngleAxis(90, Vector3.back) * _targetRotation;
		} else {
			_targetRotation = Quaternion.AngleAxis(90, Vector3.right) * _targetRotation;
		}
		updateTopFace ();

		printTargetRotation ();
	}

	public void rotateTargetRotationDown (bool leftSide) {
		//Debug.Log ("RotateDown - Left: " + leftSide);
		if (leftSide) {
			_targetRotation = Quaternion.AngleAxis(90, Vector3.forward) * _targetRotation;
		} else {
			_targetRotation = Quaternion.AngleAxis(90, Vector3.left) * _targetRotation;
		}
		updateTopFace ();

		printTargetRotation ();
	}

	// HORIZONTAL ROTATION
	public void rotateTargetRotationLeft () {
		//Debug.Log ("RotateLeft");
		_targetRotation = Quaternion.AngleAxis(90, Vector3.up) * _targetRotation;
		updateTopFace ();

		printTargetRotation ();
	}

	public void rotateTargetRotationRight () {
		//Debug.Log ("RotateRight");
		_targetRotation = Quaternion.AngleAxis(90, Vector3.down) * _targetRotation;
		updateTopFace ();

		printTargetRotation ();
	}


	void updateTopFace () {
		_oldTopFace = _topFace;
		if (_targetRotation.eulerAngles.z == 0) {
			if (_targetRotation.eulerAngles.x == 0) {
				// FACE 1
				_topFace = Face.Face1;
			} else if (_targetRotation.eulerAngles.x == 90) {
				// FACE 3
				_topFace = Face.Face3;
			} else if (_targetRotation.eulerAngles.x == 270) {
				// FACE 4
				_topFace = Face.Face4;
			}
		} else {
			if (_targetRotation.eulerAngles.z == 270) {
				// FACE 2
				_topFace = Face.Face2;
			} else if (_targetRotation.eulerAngles.z == 90) {
				// FACE 5
				_topFace = Face.Face5;
			} else if (_targetRotation.eulerAngles.z == 180) {
				// FACE 6
				_topFace = Face.Face6;
			}
		}
	}

	void updateFacePositions () {
		for (int i=0; i<faces.Length;++i) {
			GameObject obj = faces [i];
			float objPerc = _facePercent [i];
			if (i == (int)_topFace) {
				objPerc += Time.deltaTime;
				if (objPerc > 1.0f) objPerc = 1;
				obj.transform.localPosition = Vector3.Slerp (obj.transform.localPosition, new Vector3(0f,0f,2.85f), objPerc);
				//float step = 2 * Time.deltaTime;
				//obj.transform.localPosition = Vector3.MoveTowards(obj.transform.localPosition, new Vector3(0f,0f,2.85f), step);
			} else {
				objPerc -= Time.deltaTime;
				if (objPerc < 0) objPerc = 0;
				obj.transform.localPosition = Vector3.Slerp (obj.transform.localPosition, new Vector3(0f,0f,0f), 1.0f-objPerc);
				//float step = 2 * Time.deltaTime;
				//obj.transform.localPosition = Vector3.MoveTowards(obj.transform.localPosition, new Vector3(0f,0f,0f), step);
			}
			_facePercent [i] = objPerc;
		}
	}

	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * 6f);
		updateFacePositions ();
	}

	void printTargetRotation () {
		Debug.Log ("Top Face: " +_topFace);
		//Debug.Log ("Target Rotation: " + _targetRotation.eulerAngles);
	}
}
