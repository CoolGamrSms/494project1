using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

    float roomX, roomY;

	// Use this for initialization
	void Start () {

        roomX = Mathf.Floor((transform.position.x - 2) / 16f) * 16f + 2f;
        roomY = Mathf.Floor((transform.position.y - 2) / 11f) * 11f + 2f;
	}
	
	// Update is called once per frame
	void Update () {
        if (CheckCollision()) Destroy(this.gameObject);
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "Player") {
			Destroy (this.gameObject);
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
    /*void OnCollisionEnter(Collision coll){
		if (coll.collider.tag == "Map") {
			Tile b = coll.gameObject.GetComponent<Tile>();
			char c = ShowMapOnCamera.S.collisionS[b.tileNum];
			if (c == 'S' || c == 'T') {
				Destroy (this.gameObject);
			}
		}
	}*/
}
