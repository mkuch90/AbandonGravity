﻿using UnityEngine;
using System.Collections;

public enum IntroAnimation{
	FlyBy00,
	FlyBy01,
	FlyBy02,
	FlyBy03,
	FlyBy04,
	FlyBy06
}
public class CameraFlyby : MonoBehaviour {

	private CameraScrolling nextCamera;
	public float speed =0.2f;
	public float timeUntilNormal = 1f;
	private bool PlayAnimation;
	private float timeLevelLoaded = 0f;
	public IntroAnimation aniSelected = IntroAnimation.FlyBy00;
	bool flyby;
	// Use this for initialization
	void Start () {
		PlayAnimation = true;
		nextCamera = (CameraScrolling) FindObjectOfType(typeof(CameraScrolling));
		if (nextCamera != null)
		{
			nextCamera.Disable();
		}
		flyby=true;
		ResponseManager.CanPause=false;

		//AnimationState jump = this.animation[aniSelected.ToString()];

		this.GetComponent<Animation>().Play (aniSelected.ToString ());
	}
	
	// Update is called once per frame
	void Update () {
		if(flyby && PlayAnimation){
			Time.timeScale=speed;
			if(Time.time-timeLevelLoaded>timeUntilNormal){
				ResponseManager.CanPause=true;
				Time.timeScale=1f;

				if (nextCamera != null)
				{

				this.GetComponent<Camera>().enabled=false;
				nextCamera.SetToActiveCamera();
				this.enabled=false;
				flyby = false;
					nextCamera.transform.position = transform.position;
					nextCamera.transform.rotation = transform.rotation;
					PlayAnimation = false;
				}
			}
		}
	}
	void OnLevelWasLoaded(){

		timeLevelLoaded=Time.time;
		
	}
}
