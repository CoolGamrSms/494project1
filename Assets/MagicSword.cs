using UnityEngine;
using System.Collections;

public class MagicSword : MonoBehaviour {

    float roomX, roomY;
	// Use this for initialization
	void Start () {
        roomX = Mathf.Floor((transform.position.x - 2) / 16f) * 16f + 2f;
        roomY = Mathf.Floor((transform.position.y - 2) / 11f) * 11f + 2f;
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.6f, 1f), Random.Range(0.6f, 1f), Random.Range(0.6f, 1f));
        if (CheckCollision()) Kill();
	}

    public void Kill()
    {
        //TODO: Particles
        Destroy(this.gameObject);
    }

    public virtual bool CheckCollision()
    {
        if (transform.position.x < roomX-0.5f) return true;
        if (transform.position.y < roomY-0.5f) return true;
        if (transform.position.x > roomX + 11.5f) return true;
        if (transform.position.y > roomY + 6.5f) return true;
        return false;
    }
}
