using UnityEngine;
using System.Collections;

public class enemyboom : MonoBehaviour {

    public float rotation = 720f;
    public bool reversed;
    float roomX, roomY;
    public Vector3 start;
    bool alreadyHit = false;

    // Use this for initialization
    void Start () {
        reversed = false;

        //Determine what room boundaries are
        roomX = Mathf.Floor((transform.position.x - 2) / 16f) * 16f + 2f;
        roomY = Mathf.Floor((transform.position.y - 2) / 11f) * 11f + 2f;
    }
	
    public void SetHit()
    {
        alreadyHit = true;
    }

    public bool WasHit()
    {
        return alreadyHit;
    }

    public void Reverse(bool hit=false)
    {
        if(hit) alreadyHit = true;
        if (reversed) return;
        reversed = true;
        GetComponent<Rigidbody>().velocity *= -1f;
    }
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(Vector3.forward, Time.deltaTime * rotation);
        if (!isOnScreen()) Destroy(this.gameObject);
        if (!reversed)
        {
            if (CheckCollision())
            {
                Reverse();
            }
            else if(Vector3.Distance(start, transform.position)>5f)
            {
                Reverse();
            }
        }
        else if (Vector3.Distance(start, transform.position) < 0.5f)
        {
            Destroy(this.gameObject);
        }
    }

    public bool isOnScreen()
    {
        if (RoomTransition.S.current_state == EntityState.TRANSITION) return false;
        Camera cam = RoomTransition.S.GetComponent<Camera>();
        Vector3 p = cam.transform.position;
        if (p.x > roomX && p.x < roomX + 8 && p.y > roomY && p.y < roomY + 6) return true;
        return false;
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
