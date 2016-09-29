using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour {

    public Sprite[] animation;
    float animation_start_time;
    public int fps;
    public Vector3 forward;

    public float walking_velocity;
    int cooldown;
    public int health = 100;
    public float knockback = 0f;
    Color c;

	public static Enemy instance;

	// Use this for initialization
	public void Start () {
		instance = this;
        animation_start_time = Time.time;
        c = GetComponent<SpriteRenderer>().material.color;
	}

    public void knockbackEnd()
    {
        GetComponent<SpriteRenderer>().material.color = c;
    }
	
	// Update is called once per frame
	public void Update () {
        if(knockback > 0f)
        {
            knockback -= Time.deltaTime / (1.0f / Application.targetFrameRate);

            RaycastHit hit2;

            Debug.DrawRay(transform.position, forward * 2, Color.green);
            if (Physics.Raycast(transform.position, forward * 2, out hit2))
            {
                if (hit2.collider.gameObject.CompareTag("Map"))
                {
                    if (hit2.distance < 0.5f) { GetComponent<Rigidbody>().velocity = Vector3.zero; }
                }
            }

            if (knockback <= 0f) knockbackEnd();
            return;
        }
        //Animation
        int current_frame_index = ((int)((Time.time - animation_start_time) / (1.0 / fps)) % animation.Length);
        GetComponent<SpriteRenderer>().sprite = animation[current_frame_index];
    }



    public void OnTriggerEnter(Collider coll)
    {
        if (knockback > 0f) return;
		if(coll.gameObject.CompareTag("Sword") || coll.gameObject.CompareTag("Explosion") || coll.gameObject.CompareTag("Boomerang"))
        {
            health--;
            if(health <= 0)
            {
                Destroy(gameObject);
            }
            else //Knockback
            {
                //Snap to grid
                Vector3 pos = transform.position;
                pos.x = Mathf.Round(pos.x);
                pos.y = Mathf.Round(pos.y);
                transform.position = pos;

                //Set velocity
                if(PlayerControl.instance.current_direction == Direction.EAST) forward = new Vector3(1f, 0f);
                if (PlayerControl.instance.current_direction == Direction.WEST) forward = new Vector3(-1f, 0f);
                if (PlayerControl.instance.current_direction == Direction.SOUTH) forward = new Vector3(0f, -1f);
                if (PlayerControl.instance.current_direction == Direction.NORTH) forward = new Vector3(0f, 1f);
                this.GetComponent<Rigidbody>().velocity = forward * 10f;

                //Set timer
                knockback = 15f;

                //Set the color
                
                GetComponent<SpriteRenderer>().material.color = Color.red;

                //Change tag
                this.gameObject.tag = "EnemyHurt";
            }
        }
    }

}
