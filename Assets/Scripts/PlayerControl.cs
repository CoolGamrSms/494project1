using UnityEngine;
using System.Collections;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING, TRANSITION, DAMAGED};
public enum WeaponType {BOMBS, BOW, BOOMERANG}

public class PlayerControl : MonoBehaviour {

	public Sprite[] link_run_down;
	public Sprite[] link_run_up;
	public Sprite[] link_run_right;
	public Sprite[] link_run_left;
	public Sprite link_attack_up;
	public Sprite link_attack_down;
	public Sprite link_attack_left;
	public Sprite link_attack_right;

	StateMachine animation_state_machine;
	StateMachine control_state_machine;
	
	public EntityState current_state = EntityState.NORMAL;
	public Direction current_direction = Direction.SOUTH;
	public WeaponType current_weapon = WeaponType.BOMBS;

	public GameObject selected_weapon_prefab;
	public GameObject bomb;
	public GameObject boomerang;

	public float walking_velocity = 1.0f;
	public int wallet = 0;
	public int health = 3;
	public int maxHealth = 3;
	public int keys = 0;
	public bool paused = false;

	public static PlayerControl instance;

	// Use this for initialization
	void Start () {
		if (instance != null) {
			Debug.LogError ("Multiple Link objects detected");
		}
		instance = this;

		animation_state_machine = new StateMachine ();
		animation_state_machine.ChangeState (new StateIdleWithSprite (this, GetComponent<SpriteRenderer> (), link_run_down [0]));

		control_state_machine = new StateMachine ();
		control_state_machine.ChangeState (new StateLinkNormalMovement (this));
	}

	// Update is called once per frame
	void Update () {

		animation_state_machine.Update ();

		control_state_machine.Update ();

		if (control_state_machine.IsFinished ()) {
			control_state_machine.ChangeState (new StateLinkNormalMovement (this));
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			if (current_weapon == WeaponType.BOMBS) {
				Instantiate (bomb, this.transform.position, Quaternion.identity);
			}
			if (current_weapon == WeaponType.BOW) {
				//Shoot arrow
			}
			if (current_weapon == WeaponType.BOOMERANG) {
				Instantiate (boomerang, this.transform.position, Quaternion.identity);
			}
		}

		/*float horizontal_input = Input.GetAxis ("Horizontal");
		float vertical_input = Input.GetAxis ("Vertical");
		if (horizontal_input != 0.0f) {
			vertical_input = 0.0f;
		}
		GetComponent<Rigidbody> ().velocity = new Vector3 (horizontal_input, vertical_input, 0) * walking_velocity;*/
	}

    void OnCollisionEnter(Collision coll)
    {
        //Locked doors
        if(coll.gameObject.CompareTag("Map"))
        {
            Tile c = coll.gameObject.GetComponent<Tile>();
            if (c == null) return;
            if (c.tileNum == 81)
            {
                //North door
                if (transform.position.x % 1f != 0.5f) return;
                if (keys > 0) keys--;
                else return;  
                ShowMapOnCamera.MAP[c.x, c.y] = 93;
                ShowMapOnCamera.MAP[c.x-1, c.y] = 92;
                ShowMapOnCamera.S.RedrawScreen(true);
                
            }
            else if(c.tileNum == 106)
            {
                //West door
                if (transform.position.y % 1f != 0f) return;
                if (keys > 0) keys--;
                else return;
                ShowMapOnCamera.MAP[c.x, c.y] = 51;
                ShowMapOnCamera.S.RedrawScreen(true);
            }
            else if (c.tileNum == 101)
            {
                //East door
                if (transform.position.y % 1f != 0f) return;
                if (keys > 0) keys--;
                else return;
                ShowMapOnCamera.MAP[c.x, c.y] = 48;
                ShowMapOnCamera.S.RedrawScreen(true);
            }
        }
    }

	void OnTriggerEnter(Collider coll) {

		if (coll.gameObject.tag == "Rupee") {
			Destroy (coll.gameObject);
			wallet++;
		}
		else if(coll.gameObject.tag == "Key") {
			Destroy (coll.gameObject);
			keys++;
		} else if (coll.gameObject.tag == "Heart") {
			Destroy (coll.gameObject);
			if (health < maxHealth) {
				health++;
				if (health > maxHealth) {
					health = maxHealth;
				}
			}
		} else if (coll.gameObject.tag == "Enemy") {
			if (this.current_state != EntityState.DAMAGED) {

                /*Vector3 myv = GetComponent<Rigidbody>().velocity;

                Vector3 knockback = coll.gameObject.GetComponent<Rigidbody>().velocity * 5;

                if(Vector3.Cross(myv, knockback) == Vector3.zero && Vector3.Dot(myv, knockback) > 0f) {
                    knockback *= -1f;
                }

                if(current_direction == Direction.WEST || current_direction == Direction.EAST)
                {
                    if (knockback.x > 0f) knockback.y = 0f;
                }
                if (current_direction == Direction.NORTH || current_direction == Direction.SOUTH)
                {
                    if (knockback.y > 0f) knockback.x = 0f;
                }

                GetComponent<Rigidbody>().velocity = knockback;*/

                control_state_machine.ChangeState(new StateLinkDamaged(this, GetComponent<SpriteRenderer>(), 40, false));

                this.current_state = EntityState.DAMAGED;
				
				health--;
			}
			if (health <= 0) {
				//death scene
			}
		}
	}
}
