using UnityEngine;
using System.Collections;

public class Bow : MonoBehaviour {

	public GameObject arrow;
	// Use this for initialization
	void Start () {
		Instantiate (arrow, this.transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
