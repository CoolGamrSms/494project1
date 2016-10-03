using UnityEngine;
using System.Collections;

public class TorchText2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			Destroy (this.gameObject);
			Time.timeScale = 1;
		}
	}
}
