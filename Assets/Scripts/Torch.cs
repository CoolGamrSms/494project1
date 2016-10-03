using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour {
	bool first = true;
	public bool lit = false;
	public bool pickedUp = false;
	float distance = 0.75f;
	Color32 original;
	public GameObject torchText;

	void Start () {
		original = this.GetComponent<SpriteRenderer> ().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		if (lit) {
			this.GetComponent<SpriteRenderer> ().color = Color.red;
			this.tag = "LitTorch";
		}
		if (!lit) {
			this.GetComponent<SpriteRenderer> ().color = original;
			this.tag = "Torch";
		}

		if (pickedUp) {
			Vector3 direction_offset = Vector3.zero;
			Vector3 direction_eulerangle = Vector3.zero;

			if(PlayerControl.instance.current_direction == Direction.NORTH) {
				direction_offset = new Vector3 (0, distance, 0);
				direction_eulerangle = new Vector3 (0, 0, 0);
			}
			else if(PlayerControl.instance.current_direction == Direction.EAST) {
				direction_offset = new Vector3 (distance, 0, 0);
				direction_eulerangle = new Vector3 (0, 0, 270);
			}
			else if(PlayerControl.instance.current_direction == Direction.SOUTH) {
				direction_offset = new Vector3 (0, distance * -1, 0);
				direction_eulerangle = new Vector3 (0, 0, 180);
			}
			else if(PlayerControl.instance.current_direction == Direction.WEST) {
				direction_offset = new Vector3 (distance * -1, 0, 0);
				direction_eulerangle = new Vector3 (0, 0, 90);
			}

			this.transform.position = direction_offset + PlayerControl.instance.transform.position;
			Quaternion new_weapon_rotation = new Quaternion ();
			new_weapon_rotation.eulerAngles = direction_eulerangle;
			this.transform.rotation = new_weapon_rotation;
		}
		if (Input.GetKeyDown (KeyCode.X) || Input.GetKeyDown (KeyCode.Z)) {
			pickedUp = false;
			this.transform.parent = null;
			StartCoroutine (Drop());
		}
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.CompareTag ("Link") || !pickedUp) {
			pickedUp = true;
			this.transform.parent = PlayerControl.instance.transform;
			Vector3 direction_offset = Vector3.zero;
			if (first) {
				first = false;
				Time.timeScale = 0;
				Instantiate (torchText, this.transform.position, Quaternion.identity);
			}
		}
		if (coll.tag == "LitLantern") {
			StartCoroutine (Light ());
		}

	}

	IEnumerator Drop() {
		GetComponent<BoxCollider> ().enabled = false;
		yield return new WaitForSeconds(1);
		GetComponent<BoxCollider> ().enabled = true;
	}
	IEnumerator Light() {
		lit = true;
		yield return new WaitForSeconds(10);
		lit = false;
	}
}
