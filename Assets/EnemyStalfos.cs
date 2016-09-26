using UnityEngine;
using System.Collections;

public class EnemyStalfos : MonoBehaviour {

    public Sprite[] animation;
    float animation_start_time;
    public int fps;
    public Vector3 forward;

    public float walking_velocity;
    int cooldown;
    public int health = 100;
    public float knockback = 0f;
    Color c;

	// Use this for initialization
	void Start () {
        animation_start_time = Time.time;
        c = GetComponent<SpriteRenderer>().material.color;
        changeDirection();
	}
	
	// Update is called once per frame
	void Update () {
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

            if (knockback <= 0f) changeDirection();
            return;
        }
        //Animation
        int current_frame_index = ((int)((Time.time - animation_start_time) / (1.0 / fps)) % animation.Length);
        GetComponent<SpriteRenderer>().sprite = animation[current_frame_index];

        //Movement
        if (cooldown > 0) --cooldown;
        if (Mathf.Round(transform.position.x * 10f) / 10f % 1.0f == 0)
        if (Mathf.Round(transform.position.y * 10f) / 10f % 1.0f == 0)
        if (cooldown == 0)
            {
                changeDirection();
            }
        RaycastHit hit;

        Debug.DrawRay(transform.position, forward*2, Color.green);
        if(Physics.Raycast(transform.position, forward*2, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Map"))
            {
                if (hit.distance < 0.5f) changeDirection();
            }
        }
    }

    void changeDirection()
    {
        GetComponent<SpriteRenderer>().material.color = c;
        this.gameObject.tag = "Enemy";
        cooldown = 100;
        //Snap to the grid
        Vector3 pos = transform.position;
        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);
        transform.position = pos;

        //Pick a random direction
        float v, h = Mathf.Round(Random.Range(0f, 1f));

        if (h == 0f) v = 1f;
        else        v = 0f;

        int flip = Random.Range(0, 2);
        if(flip == 1)
        {
            h *= -1f;
            v *= -1f;
        }

        forward = new Vector3(h, v);
        this.GetComponent<Rigidbody>().velocity = forward * walking_velocity;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (knockback > 0f) return;
        if(coll.gameObject.CompareTag("Sword"))
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
                this.GetComponent<Rigidbody>().velocity = forward *walking_velocity* 5;

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
