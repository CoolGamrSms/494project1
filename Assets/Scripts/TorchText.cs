using UnityEngine;
using System.Collections;

public class TorchText : MonoBehaviour {

	public GameObject nextText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z)) {
			Destroy (this.gameObject);
			Instantiate (nextText, this.transform.position, Quaternion.identity);
		}
	}
}
