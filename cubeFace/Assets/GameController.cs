using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public SwipeManager swipeManager;
	public CubeController cubeController;

	private GameObject[] _cubeFaces;
	
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
		Vector2 firstTap = SwipeManager.firstTapPos;
		Vector2 liftTap = SwipeManager.lastPos;

		if (SwipeManager.swipeDirection != Swipe.None) {
			Debug.Log ("FirstTap: " + firstTap + "LiftTap: " + liftTap);
		}
		if (SwipeManager.swipeDirection == Swipe.Up) {
			playerSwipedVertically (false, true);
		} else if (SwipeManager.swipeDirection == Swipe.Down) {
			playerSwipedVertically (true, true);
		} else if (SwipeManager.swipeDirection == Swipe.Left) {
			playerSwipedHorizontally (true);
		} else if (SwipeManager.swipeDirection == Swipe.Right) {
			playerSwipedHorizontally (false);
		}
	}
}
