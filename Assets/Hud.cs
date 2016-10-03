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
	public Text bomb_text;
	public bool boomAdded = false;
	public bool bowAdded = false;
	public bool paused = false;

	enum ArrowState {BOOMERANG, BOW, BOMB}

	ArrowState current_arrow = ArrowState.BOMB;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
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
				if (current_arrow == ArrowState.BOW && boomAdded)
					current_arrow = ArrowState.BOOMERANG;
				else
					current_arrow = ArrowState.BOMB;
			}
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				if (current_arrow == ArrowState.BOMB && boomAdded)
					current_arrow = ArrowState.BOOMERANG;
				else if (bowAdded)
					current_arrow = ArrowState.BOW;
			}
			if (current_arrow == ArrowState.BOMB) {
				bomb_select.text = "> Bombs";
				if (boomAdded)
					boom_select.text = "Boomerang";
				else
					boom_select.text = "";
				if (bowAdded) 
					bow_select.text = "Bow";
				else
					bow_select.text = "";
			} else if (current_arrow == ArrowState.BOOMERANG) {
				bomb_select.text = "Bombs";
				boom_select.text = "> Boomerang";
				if (bowAdded)
					bow_select.text = "Bow";
				else
					bow_select.text = "";
			} else {
				bomb_select.text = "Bombs";
				if (boomAdded)
					boom_select.text = "Boomerang";
				else
					boom_select.text = "";
				bow_select.text = "> Bow";
			}
		}


		int num_player_rupees = PlayerControl.instance.wallet;
		rupee_text.text = "Rupees: " + num_player_rupees.ToString ();

		int player_health = PlayerControl.instance.health;
		heart_text.text = "HP: " + ((float)player_health/2f).ToString ();

		int player_keys = PlayerControl.instance.keys;
		key_text.text = "Keys: " + player_keys.ToString ();

		string eq = PlayerControl.instance.current_weapon.ToString ();
		Equipped.text = "Equipped: " + eq;

		int player_bombs = PlayerControl.instance.bombs;
		bomb_text.text = "Bombs: " + player_bombs;
		
	}

	public void AddBoomerang() {
		boomAdded = true;
	}

	public void AddBow() {
		bowAdded = true;
	}
}
