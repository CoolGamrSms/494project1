using UnityEngine;
using UnityEngine.UI;
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

    public float horizontal_input, vertical_input;

	public StateMachine animation_state_machine;
	public StateMachine control_state_machine;
	
	public EntityState current_state = EntityState.NORMAL;
	public Direction current_direction = Direction.SOUTH;
	public WeaponType current_weapon = WeaponType.BOMBS;

	public GameObject selected_weapon_prefab;
    public GameObject magic_prefab;
    public GameObject magic_instance;
    public GameObject bow;
	public GameObject bomb;
	public GameObject boomerang;
    GameObject myBoomerang;
	public Hud rupee_text;
	public Hud heart_text;
	public Hud key_text;
	public Hud bomb_select;
	public Hud boom_select;
	public Hud bow_select;
	public Hud Equipped;
	public Hud Panel;
	public Hud select;

	public float walking_velocity = 1.0f;
	public int wallet = 0;
	public int health = 6;
	public int maxHealth = 6;
	public int keys = 0;
	public bool paused = false;
    public float cooldown;

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

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                cooldown = 0f;
                if(current_state == EntityState.DAMAGED) current_state = EntityState.NORMAL;
                GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
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

    public void DropBomb()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Round(pos.x * 2) / 2f;
        pos.y = Mathf.Round(pos.y * 2) / 2f;
        if (current_direction == Direction.NORTH) pos.y += 1.2f;
        if (current_direction == Direction.EAST) { pos.x += 1; pos.y += 0.2f; }
        if (current_direction == Direction.SOUTH) pos.y -= 0.8f;
        if (current_direction == Direction.WEST) { pos.x -= 1; pos.y += 0.2f; }
        Instantiate(bomb, pos, Quaternion.identity);
    }

    public void ThrowBoomerang()
    {
        if (myBoomerang != null) return;
        myBoomerang = (GameObject)Instantiate(boomerang, transform.position, Quaternion.identity);
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
		} else if (coll.gameObject.tag == "Key") {
			Destroy (coll.gameObject);
			keys++;
		} else if (coll.gameObject.tag == "Heart") {
			Destroy (coll.gameObject);
			if (health < maxHealth) {
				health+=2;
				if (health > maxHealth) {
					health = maxHealth;
				}
			}
		} else if (coll.gameObject.tag == "Container") {
			Destroy (coll.gameObject);
            		maxHealth += 2;
			health = maxHealth;
		} else if (coll.gameObject.tag == "Bow") {
			Destroy (coll.gameObject);
			Debug.Log ("Bow Entered");
			rupee_text.AddBow ();
			heart_text.AddBow ();
			key_text.AddBow ();
			bomb_select.AddBow ();
			bow_select.AddBow ();
			boom_select.AddBow ();
			Equipped.AddBow ();
			Panel.AddBow ();
			select.AddBow ();
		} else if (coll.gameObject.tag == "BoomPickup") {
			Destroy (coll.gameObject);
			Debug.Log ("Boom Entered");
			rupee_text.AddBoomerang ();
			heart_text.AddBoomerang ();
			key_text.AddBoomerang ();
			bomb_select.AddBoomerang ();
			bow_select.AddBoomerang ();
			boom_select.AddBoomerang ();
			Equipped.AddBoomerang ();
			Panel.AddBoomerang ();
			select.AddBoomerang ();
		}
		else if (coll.gameObject.tag == "Enemy") {
			if (this.current_state != EntityState.DAMAGED) {
                if (coll.gameObject.GetComponent<Enemy>() != null && coll.gameObject.GetComponent<Enemy>().isHurt) return;

                //Shield blocking for enemy boomerangs
                if(coll.gameObject.GetComponent<enemyboom>() != null)
                {
                    if (coll.gameObject.GetComponent<enemyboom>().WasHit()) return;
                    Vector3 v = coll.gameObject.GetComponent<Rigidbody>().velocity;
                    if(this.current_state == EntityState.NORMAL)
                    {
                        if ((v.x < 0f && this.current_direction == Direction.EAST) ||
                         (v.x > 0f && this.current_direction == Direction.WEST) ||
                         (v.y < 0f && this.current_direction == Direction.NORTH) ||
                         (v.y > 0f && this.current_direction == Direction.SOUTH)) {
                            coll.gameObject.GetComponent<enemyboom>().Reverse(true);
                            return;
                        }
                        coll.gameObject.GetComponent<enemyboom>().SetHit();
                    }
                }
                Vector3 myv = GetComponent<Rigidbody>().velocity;

                /*Vector3 knockback = coll.gameObject.GetComponent<Rigidbody>().velocity * 5;

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

                cooldown = 0.5f;

                this.current_state = EntityState.DAMAGED;

                GetComponent<SpriteRenderer>().color = Color.red;

                health--;
			}
			if (health <= 0) {
				//death scene
			}
		}
	}
}
