﻿using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z)) {
			Application.LoadLevel ("Dungeon");
		}
		if (Input.GetKeyDown (KeyCode.X)) {
			Application.LoadLevel ("Custom");
		}
	}
}
