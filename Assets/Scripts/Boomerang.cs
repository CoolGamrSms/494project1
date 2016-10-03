using UnityEngine;
using System.Collections;

public class Boomerang : MonoBehaviour {

	public enum Direction {NORTH, EAST, SOUTH, WEST};
	public Vector3 forward;
	public float speed = 10f;
	public float rotation = 1000f;
	private float startTime;
	bool done = false;
    float roomX, roomY;
    public Direction start_direction;
    Vector3 start;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
        start = transform.position;
		start_direction = Direction.NORTH;

        roomX = Mathf.Floor((transform.position.x - 2) / 16f) * 16f + 2f;
        roomY = Mathf.Floor((transform.position.y - 2) / 11f) * 11f + 2f;

        if (PlayerControl.instance.current_direction == global::Direction.EAST) {
			forward = new Vector3 (1f, 0f);
			start_direction = Direction.EAST;
		}
		if (PlayerControl.instance.current_direction == global::Direction.NORTH) {
			forward = new Vector3 (0f, 1f);
			start_direction = Direction.NORTH;
		}
		if (PlayerControl.instance.current_direction == global::Direction.SOUTH) {
			forward = new Vector3 (0f, -1f);
			start_direction = Direction.SOUTH;
		}
		if (PlayerControl.instance.current_direction == global::Direction.WEST) {
			forward = new Vector3 (-1f, 0f);
			start_direction = Direction.WEST;
		}
		this.GetComponent<Rigidbody> ().velocity = forward * speed;
	}



	public float m_distanceTraveled = 0f;
    // Update is called once per frame
    void Update() {

        this.transform.Rotate(Vector3.forward, Time.deltaTime * rotation);
        if (!done)
        {
            if (CheckCollision())
            {
                done = true;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            else if (Vector3.Distance(start, transform.position) > 5f)
            {
                done = true;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, PlayerControl.instance.transform.position, speed * Time.deltaTime);
            if (Vector3.Distance(PlayerControl.instance.transform.position, transform.position) < 0.2f)
            {
                Destroy(this.gameObject);
            }
        }
        /*
        this.transform.Rotate(Vector3.forward, Time.deltaTime * rotation);
		this.GetComponent<Rigidbody> ().velocity = forward * speed;
		if (m_distanceTraveled > 5f || CheckCollision()) {
			done = true;
			this.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			//Destroy (this.gameObject);
			//this.transform.position = Vector3.MoveTowards(this.transform.position, PlayerControl.instance.transform.position,   speed*Time.deltaTime);
		}
		m_distanceTraveled = (Time.time - startTime) * speed;
		if (done) {

			//StartCoroutine(moveTo(PlayerControl.instance.transform.position));
			transform.position = Vector3.MoveTowards (transform.position, PlayerControl.instance.transform.position , speed * Time.deltaTime);
			if (this.transform.position == PlayerControl.instance.transform.position) {
				Destroy (this.gameObject);
			}
		}*/
	}

	void OnTriggerEnter(Collider coll) {
		//Debug.Log ("Hit something");
		if (coll.gameObject.CompareTag ("EnemyHurt") || coll.gameObject.CompareTag ("Enemy")) {
			m_distanceTraveled = 5.1f;
			this.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			done = true;
			transform.position = Vector3.MoveTowards (transform.position, PlayerControl.instance.transform.position , speed * Time.deltaTime);
			if (this.transform.position == PlayerControl.instance.transform.position) {
				Destroy (this.gameObject);
			}
		}
	}

    public virtual bool CheckCollision()
    {
        if (transform.position.x < roomX-1) return true;
        if (transform.position.y < roomY-1) return true;
        if (transform.position.x > roomX + 12) return true;
        if (transform.position.y > roomY + 7) return true;
        return false;
    }
}
