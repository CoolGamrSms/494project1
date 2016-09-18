using UnityEngine;
using System.Collections;

public class player_cont : MonoBehaviour {

	public float walking_velocity = 1.0f;
	public int wallet = 0;
	public int health = 3;
	public int maxHealth = 3;

	public static player_cont instance;

	// Use this for initialization
	void Start () {
		if (instance != null) {
			Debug.LogError ("Multiple Link objects detected");
		}
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal_input = Input.GetAxis ("Horizontal");
		float vertical_input = Input.GetAxis ("Vertical");
		if (horizontal_input != 0.0f) {
			vertical_input = 0.0f;
		}
		GetComponent<Rigidbody> ().velocity = new Vector3 (horizontal_input, vertical_input, 0) * walking_velocity;
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.tag == "Rupee") {
			Destroy (coll.gameObject);
			wallet++;
		} else if (coll.gameObject.tag == "Heart") {
			if (health < maxHealth) {
				Destroy (coll.gameObject);
				health++;
				if (health > maxHealth) {
					health = maxHealth;
				}
			}
		}
	}
}
