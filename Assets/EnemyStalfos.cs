using UnityEngine;
using System.Collections;

public class EnemyStalfos : MonoBehaviour {

    public Sprite[] animation;
    float animation_start_time;
    public int fps;
    public Vector3 forward;

    public float walking_velocity;
    int cooldown;

	// Use this for initialization
	void Start () {
        animation_start_time = Time.time;
        changeDirection();
	}
	
	// Update is called once per frame
	void Update () {

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

}
