using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour {

	enum Face : byte {Face1=0, Face2=1, Face3=2, Face4=3, Face5=4, Face6=5, FaceNone=6};

	public GameObject[] faces;
	private float[] _facePercent;

	private Face _topFace;

	private Quaternion _targetRotation;

	// Use this for initialization
	void Start () {
		_topFace = Face.Face1;
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

	bool floatsWithinDist (float a, float b, float dist) {
		if (Mathf.Abs (a-b) <= dist) {
			return true;
		}
		return false;
	}

	void updateTopFace () {
		float dist = 10.0f;
		if (floatsWithinDist (_targetRotation.eulerAngles.z, 0.0f, dist)) {
			if (floatsWithinDist (_targetRotation.eulerAngles.x, 0.0f, dist)) {
				// FACE 1
				_topFace = Face.Face1;
			} else if (floatsWithinDist (_targetRotation.eulerAngles.x, 90.0f, dist)) {
				// FACE 3
				_topFace = Face.Face3;
			} else if (floatsWithinDist (_targetRotation.eulerAngles.x, 270.0f, dist)) {
				// FACE 4
				_topFace = Face.Face4;
			} else {
				Debug.Log ("NO");
			}
		} else {
			if (floatsWithinDist (_targetRotation.eulerAngles.z, 270.0f, dist)) {
				// FACE 2
				_topFace = Face.Face2;
			} else if (floatsWithinDist (_targetRotation.eulerAngles.z, 90.0f, dist)) {
				// FACE 5
				_topFace = Face.Face5;
			} else if (floatsWithinDist (_targetRotation.eulerAngles.z, 180.0f, dist)) {
				// FACE 6
				_topFace = Face.Face6;
			} else {
				Debug.Log ("NO");
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
		Debug.Log ("Target Rotation: " + _targetRotation.eulerAngles);
	}
}
