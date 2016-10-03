using UnityEngine;
using System.Collections;

public class Lantern : MonoBehaviour {

	public bool lit;
	public Sprite isLit;
	public Sprite notLit;
	bool stayslit = false;
	public int doorX, doorY;
	public GameObject partner;
	// Use this for initialization
	void Start () {
		if (lit) {
			GetComponent<SpriteRenderer> ().sprite = isLit;
			stayslit = true;
			this.tag = "LitLantern";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (stayslit) {
			lit = true;
		}
		if (lit) {
			this.tag = "LitLantern";
			GetComponent<SpriteRenderer> ().sprite = isLit;
		}
		if (!lit) {
			this.tag = "Lantern";
			GetComponent<SpriteRenderer> ().sprite = notLit;
		}
		if (partner.tag == "LitLantern" && this.tag == "LitLantern" && !stayslit) {
			stayslit = true;
			if (ShowMapOnCamera.MAP [doorX + 1, doorY] == 29) {
				ShowMapOnCamera.MAP [doorX, doorY] = 51;
				ShowMapOnCamera.S.RedrawScreen (true);
                SFXScript.S.OpenDoor();
            }
			if (ShowMapOnCamera.MAP [doorX - 1, doorY] == 29) {
				ShowMapOnCamera.MAP [doorX, doorY] = 48;
				ShowMapOnCamera.S.RedrawScreen (true);
                SFXScript.S.OpenDoor();
            }
			/*if (doorX < this.transform.position.x) {
				ShowMapOnCamera.MAP [doorX, doorY] = 51;
				ShowMapOnCamera.S.RedrawScreen (true);
			} else {
				ShowMapOnCamera.MAP [doorX, doorY] = 48;
				ShowMapOnCamera.S.RedrawScreen (true);
			}*/
			stayslit = true;
		}
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "LitTorch") {
			if (!stayslit)
				StartCoroutine (Light());
			
		}
	}

	IEnumerator Light() {
		lit = true;
		yield return new WaitForSeconds(10);
		lit = false;
	}
}
