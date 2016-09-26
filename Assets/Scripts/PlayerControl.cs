using UnityEngine;
using System.Collections;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING, TRANSITION, DAMAGED};

public class PlayerControl : MonoBehaviour {

	public Sprite[] link_run_down;
	public Sprite[] link_run_up;
	public Sprite[] link_run_right;
	public Sprite[] link_run_left;

	StateMachine animation_state_machine;
	StateMachine control_state_machine;
	
	public EntityState current_state = EntityState.NORMAL;
	public Direction current_direction = Direction.SOUTH;

	public GameObject selected_weapon_prefab;

	public float walking_velocity = 1.0f;
	public int wallet = 0;
	public int health = 3;
	public int maxHealth = 3;
	public int keys = 0;

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


		/*float horizontal_input = Input.GetAxis ("Horizontal");
		float vertical_input = Input.GetAxis ("Vertical");
		if (horizontal_input != 0.0f) {
			vertical_input = 0.0f;
		}
		GetComponent<Rigidbody> ().velocity = new Vector3 (horizontal_input, vertical_input, 0) * walking_velocity;*/
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
				var magnitude = 5000;
				// calculate force vector
				var force = transform.position - coll.transform.position;
				// normalize force vector to get direction only and trim magnitude
				force.Normalize();
				GetComponent<Rigidbody>().AddForce(force * magnitude);
				this.current_state = EntityState.DAMAGED;
				control_state_machine.ChangeState(new StateLinkDamaged(this, GetComponent<SpriteRenderer> (), 100));
				health--;
			}
			if (health <= 0) {
				//death scene
			}
		}
	}
}
