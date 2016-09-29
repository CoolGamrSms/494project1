using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hud : MonoBehaviour {

	public Text rupee_text;
	public Text heart_text;
	public Text key_text;
	public Text bomb_select;
	public Text boom_select;
	public Text bow_select;
	public Text Equipped;
	public bool paused = false;

	enum ArrowState {BOOMERANG,BOW, BOMB}

	ArrowState current_arrow = ArrowState.BOOMERANG;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.RightShift)) {
			if (paused) {
				Time.timeScale = 1;
				paused = false;
				PlayerControl.instance.paused = false;
				Vector3 temp = new Vector3 (0, 80, 0);
				this.gameObject.transform.position += temp;
			} else {
				Time.timeScale = 0;
				paused = true;
				PlayerControl.instance.paused = true;
				Vector3 temp = new Vector3 (0, 80, 0);
				this.gameObject.transform.position -= temp;
			}

		}
		if (paused) {

			if (current_arrow == ArrowState.BOOMERANG) {
				PlayerControl.instance.current_weapon = WeaponType.BOOMERANG;
			} else if (current_arrow == ArrowState.BOMB) {
				PlayerControl.instance.current_weapon = WeaponType.BOMBS;
			} else {
				PlayerControl.instance.current_weapon = WeaponType.BOW;
			}
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				if (current_arrow == ArrowState.BOW)
					current_arrow = ArrowState.BOOMERANG;
				else
					current_arrow = ArrowState.BOMB;
			}
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				if (current_arrow == ArrowState.BOMB)
					current_arrow = ArrowState.BOOMERANG;
				else
					current_arrow = ArrowState.BOW;
			}
			if (current_arrow == ArrowState.BOMB) {
				bomb_select.text = "> Bombs";
				boom_select.text = "Boomerang";
				bow_select.text = "Bow";					
			} else if (current_arrow == ArrowState.BOOMERANG) {
				bomb_select.text = "Bombs";
				boom_select.text = "> Boomerang";
				bow_select.text = "Bow";
			} else {
				bomb_select.text = "Bombs";
				boom_select.text = "Boomerang";
				bow_select.text = "> Bow";
			}
		}


		int num_player_rupees = PlayerControl.instance.wallet;
		rupee_text.text = "Rupees: " + num_player_rupees.ToString ();

		int player_health = PlayerControl.instance.health;
		heart_text.text = "Health: " + player_health.ToString ();

		int player_keys = PlayerControl.instance.keys;
		key_text.text = "Keys: " + player_keys.ToString ();

		string eq = PlayerControl.instance.current_weapon.ToString ();
		Equipped.text = "Equipped: " + eq;
		
	}
}
