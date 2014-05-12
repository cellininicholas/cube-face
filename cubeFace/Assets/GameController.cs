﻿using UnityEngine;
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
		if (swipeManager.swipeDirection == Swipe.Up) {
			// do something...
			
		}
	}
}
