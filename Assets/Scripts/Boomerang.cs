using UnityEngine;
using System.Collections;

public class Boomerang : MonoBehaviour {

	public enum Direction {NORTH, EAST, SOUTH, WEST};
	public Vector3 forward;
	public float speed = 10f;
	public float rotation = 1000f;
	private float startTime;
	bool done = false;
	public Direction start_direction;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		start_direction = Direction.NORTH;

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



	IEnumerator moveTo(Vector3 pos) {
		transform.position = Vector3.MoveTowards (transform.position, pos , speed * Time.deltaTime);
		yield return null;   
	}

	public float m_distanceTraveled = 0f;
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(Vector3.forward, Time.deltaTime * rotation);
		this.GetComponent<Rigidbody> ().velocity = forward * speed;
		if (m_distanceTraveled > 5f) {
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
		}
	}

	void OnTriggerEnter(Collider coll) {
		//Debug.Log ("Hit something");
		if (coll.gameObject.CompareTag ("EnemyHurt") || coll.gameObject.CompareTag ("Enemy")) {
			Debug.Log ("Hit Enemy");
			m_distanceTraveled = 5.1f;
			this.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			done = true;
			transform.position = Vector3.MoveTowards (transform.position, PlayerControl.instance.transform.position , speed * Time.deltaTime);
			if (this.transform.position == PlayerControl.instance.transform.position) {
				Destroy (this.gameObject);
			}
		}
	}

	void OnCollisionEnter(Collision coll){
		if (coll.collider.tag == "Map") {
			Tile b = coll.gameObject.GetComponent<Tile>();
			char c = ShowMapOnCamera.S.collisionS[b.tileNum];
			if (c == 'S' || c == 'T') {
				Destroy (this.gameObject);
			}
		}
	}
}
