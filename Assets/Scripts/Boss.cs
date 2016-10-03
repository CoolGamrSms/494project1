using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

	public int doorX, doorY;
    public bool eastwest = false;
	public GameObject container;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy() {
        SFXScript.S.OpenDoor();
        ShowMapOnCamera.MAP[doorX, doorY] = (eastwest)?48:51;
		ShowMapOnCamera.S.RedrawScreen(true);
		Instantiate (container, this.transform.position, Quaternion.identity);
	}
}
