using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hud : MonoBehaviour {

	public Text rupee_text;
	public Text heart_text;
	public Text key_text;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int num_player_rupees = PlayerControl.instance.wallet;
		rupee_text.text = "Rupees: " + num_player_rupees.ToString ();

		int player_health = PlayerControl.instance.health;
		heart_text.text = "Health: " + player_health.ToString ();

		int player_keys = PlayerControl.instance.keys;
		key_text.text = "Keys: " + player_keys.ToString ();
		
	}
}
