using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public SwipeManager swipeManager;
	public CubeController cubeController;

	//private GameObject[] _cubeFaces;
	
	public void playerSwipedVertically (bool down, bool onLeftOfScreen) {
		if (down) {
			cubeController.rotateTargetRotationDown (onLeftOfScreen);
		} else {
			cubeController.rotateTargetRotationUp (onLeftOfScreen);
		}

	}

	public void playerSwipedHorizontally(bool left) {
		if (left) {
			cubeController.rotateTargetRotationLeft();
		} else {
			cubeController.rotateTargetRotationRight();
		}
	}

	
	// Update is called once per frame
	void Update () {
		//Vector2 firstTap = SwipeManager.firstTapPos;
		//Vector2 liftTap = SwipeManager.lastPos;

		if (SwipeManager.swipeDirection != Swipe.None) {
			//Debug.Log ("FirstTap: " + firstTap + "LiftTap: " + liftTap);
			//Debug.Log (SwipeManager.swipeHorizontalPercentage);
		}
		if (SwipeManager.swipeDirection == Swipe.Up) {
			bool leftSide = SwipeManager.swipeHorizontalPercentage <= 0.5f;
			playerSwipedVertically (false, leftSide);
		} else if (SwipeManager.swipeDirection == Swipe.Down) {
			bool leftSide = SwipeManager.swipeHorizontalPercentage <= 0.5f;
			playerSwipedVertically (true, leftSide);
		} else if (SwipeManager.swipeDirection == Swipe.Left) {
			playerSwipedHorizontally (true);
		} else if (SwipeManager.swipeDirection == Swipe.Right) {
			playerSwipedHorizontally (false);
		}

		// CHECK KEYBOARD
		if (Input.GetKeyDown("w")) {
			playerSwipedVertically (false, true);
		} else if (Input.GetKeyDown("s")) {
			playerSwipedVertically (true, true);
		} else if (Input.GetKeyDown("e")) {
			playerSwipedVertically (false, false);
		} else if (Input.GetKeyDown("d")) {
			playerSwipedVertically (true, false);
		} else if (Input.GetKeyDown("a")) {
			playerSwipedHorizontally (true);
		} else if (Input.GetKeyDown("f")) {
			playerSwipedHorizontally (false);
		}
	}
}
