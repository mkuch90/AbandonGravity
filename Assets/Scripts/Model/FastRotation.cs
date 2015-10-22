﻿using UnityEngine;
using System.Collections;

public class FastRotation : MonoBehaviour {
	
	static System.Random rand;
	// Use this for initialization
	void Start () {
		rand=new System.Random();
		//this.rigidbody.angularVelocity = new Vector3(0, 0, 20);	
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion oldrotation = this.GetComponent<Rigidbody>().rotation;
		this.GetComponent<Rigidbody>().rotation= Quaternion.Lerp(oldrotation, Quaternion.LookRotation(new Vector3(1,1,0), Vector3.up), Time.deltaTime * 8);
	}
}

