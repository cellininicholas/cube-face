using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour {
	public bool updateActive;

	enum Face : byte {Face1=0, Face2=1, Face3=2, Face4=3, Face5=4, Face6=5, FaceNone=6};

	public GameObject[] faceSpaces;
	public GameObject[] faces;
	private float[] _facePercent;
	private int[] _faceRotOffset;
	private Face _topFace;

	public GameObject faceOptionsPrefab;
	private GameObject[] _faceOptions;

	private Quaternion _targetRotation;

	// Use this for initialization
	void Start () {
		_topFace = Face.Face1;
		_targetRotation = transform.rotation;

		_facePercent = new float[] { 1, 0, 0, 0, 0, 0 };
		_faceRotOffset = new int[] { 0, 0, 0, 0, 0, 0 };

		_faceOptions = new GameObject[6];
		// Instantiate FaceOptions Prefab
		for (int y = 0; y < faceSpaces.Length; ++y) {
			_faceOptions[y] = Instantiate(faceOptionsPrefab) as GameObject;
			_faceOptions[y].transform.parent = faceSpaces[y].transform;
			_faceOptions[y].transform.localPosition = Vector3.zero;
			_faceOptions[y].transform.localRotation = Quaternion.Euler(0,90, 90);
		}
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
		_faceRotOffset[(int)_topFace] = (_faceRotOffset[(int)_topFace] + 90) % 360;
		updateTopFace ();

		printTargetRotation ();
	}

	public void rotateTargetRotationRight () {
		//Debug.Log ("RotateRight");
		_targetRotation = Quaternion.AngleAxis(90, Vector3.down) * _targetRotation;
		_faceRotOffset[(int)_topFace] = (_faceRotOffset[(int)_topFace] - 90) % 360;
		if (_faceRotOffset [(int)_topFace] < 0) _faceRotOffset [(int)_topFace] += 360;

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

		int offset = _faceRotOffset[(int)_topFace] - (int)_targetRotation.eulerAngles.y;

		//Debug.Log(" Target Rotation: " + (int)_targetRotation.eulerAngles.y);
		//Debug.Log(" Face Rot Offset: " + (int)_faceRotOffset[(int)_topFace]);
		Debug.Log("Offset: " + offset);
	}
	
	void updateFacePositions () {
		for (int i=0; i<faces.Length;++i) {
			GameObject cubeFace = faces [i];
			GameObject faceOptionsObj = _faceOptions[i];
			//GameObject faceSpaceObj = faceSpaces[i];
			float objPerc = _facePercent [i];

			FaceOptionsController faceController = null;
			if (faceOptionsObj != null) {
				faceController = (FaceOptionsController) faceOptionsObj.GetComponent(typeof(FaceOptionsController));
			}

			if (i == (int)_topFace) {
				objPerc += Time.deltaTime * 0.8f;
				if (objPerc > 1.0f) objPerc = 1;
				cubeFace.transform.localPosition = Vector3.Slerp (cubeFace.transform.localPosition, new Vector3(0f,0f,2.85f), objPerc);
				faceOptionsObj.transform.localPosition = Vector3.Slerp (faceOptionsObj.transform.localPosition, new Vector3(0f,0f,2.85f), objPerc);


			} else {
				objPerc -= Time.deltaTime * 0.8f;
				if (objPerc < 0) objPerc = 0;
				cubeFace.transform.localPosition = Vector3.Slerp (cubeFace.transform.localPosition, new Vector3(0f,0f,0f), 1.0f-objPerc);
				faceOptionsObj.transform.localPosition = Vector3.Slerp (faceOptionsObj.transform.localPosition, new Vector3(0f,0f,6f), 1.0f-objPerc);

			}

			// offset CubeFace and FaceOptions
			cubeFace.transform.localRotation = Quaternion.Slerp (cubeFace.transform.localRotation, Quaternion.AngleAxis(_faceRotOffset[i], Vector3.back), Time.deltaTime * 6f);
			faceOptionsObj.transform.localRotation = Quaternion.Slerp (faceOptionsObj.transform.localRotation, Quaternion.Euler(_faceRotOffset[i],90, 90), Time.deltaTime * 6f);

			//Quaternion.Euler(90,0, _faceRotOffset[i]);

			if (faceController != null) {
				float percent = 4 - faceOptionsObj.transform.localPosition.z;
				percent /= 3f;

				int offset = _faceRotOffset[i] - (int)_targetRotation.eulerAngles.y;

				faceController.setAlphaWithRotationOffset(percent, offset);
			}

			_facePercent [i] = objPerc;
		}
	}

	// Update is called once per frame
	void Update () {
		if (updateActive) {
			transform.rotation = Quaternion.Slerp (transform.rotation, _targetRotation, Time.deltaTime * 6f);
			updateFacePositions ();
		}
	}

	void printTargetRotation () {
		//Debug.Log ("Top Face: " +_topFace);
		//Debug.Log ("Target Rotation: " + _targetRotation.eulerAngles);
	}
}
