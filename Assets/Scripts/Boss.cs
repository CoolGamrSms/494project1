using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

	public int doorX, doorY;
	public GameObject container;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy() {
		ShowMapOnCamera.MAP[doorX, doorY] = 51;
		ShowMapOnCamera.S.RedrawScreen(true);
		Instantiate (container, this.transform.position, Quaternion.identity);
	}
}
