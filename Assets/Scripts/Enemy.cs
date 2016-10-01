using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour {

    public Sprite[] animation;
    public int health = 1;
    float animation_start_time;
    protected int fps;

    Color c;

    bool isHurt;
    bool isKnockback;
    bool stopped;

    float roomX, roomY;

	// Use this for initialization
	public void Start () {
        //Initialize values
        animation_start_time = Time.time;
        c = GetComponent<SpriteRenderer>().material.color;
        isHurt = false;
        isKnockback = false;
        stopped = false;

        //Determine what room boundaries are
        roomX = Mathf.Floor((transform.position.x - 2) / 16f) * 16f + 2f;
        roomY = Mathf.Floor((transform.position.y - 2) / 11f) * 11f + 2f;

        if (isOnScreen()) StartMovement();
        else stopped = true;
	}

    public abstract void StartMovement();

    public abstract void UpdateMovement();

    public void SnapGrid()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);
        transform.position = pos;
    }

    public void Hurt()
    {
        if (isHurt) return;
        isHurt = true;
        GetComponent<SpriteRenderer>().material.color = Color.red;
        health--;
        if (health <= 0) Kill();
        else Invoke("EndHurt", 0.5f);
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }

    public void Knockback(Vector3 direction)
    {
        direction /= direction.sqrMagnitude;
        if (isKnockback) return;
        isKnockback = true;
        GetComponent<Rigidbody>().velocity = direction*10;
        Invoke("EndKnockback", 0.3f);
    }

    public void EndKnockback()
    {
        if (!isKnockback) return;
        isKnockback = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        StartMovement();
    }

    public void EndHurt()
    {
        if (!isHurt) return;
        isHurt = false;
        GetComponent<SpriteRenderer>().material.color = c;
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
        if (transform.position.x < roomX) return true;
        if (transform.position.y < roomY) return true;
        if (transform.position.x > roomX + 11) return true;
        if (transform.position.y > roomY + 6) return true;
        return false;
    }

    public void SnapBounds()
    {
        Vector3 p = transform.position;
        if (p.x < roomX) p.x = roomX;
        if (p.y < roomY) p.y = roomY;
        if (p.x > roomX + 11) p.x = roomX + 11;
        if (p.y > roomY + 6) p.y = roomY + 6;
        transform.position = p;
    }

    // Update is called once per frame
    public void Update () {

        if(!stopped)
        {
            if (!isOnScreen())
            {
                stopped = true;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                SnapGrid();
                return;
            }
        }

        if(stopped)
        {
            if (isOnScreen())
            {
                stopped = false;
                StartMovement();
            }
            return;
        }

        if (isKnockback) {
            if (CheckCollision()) EndKnockback();
        } else UpdateMovement();

        //Animation
        int current_frame_index;
        if (fps == 0) current_frame_index = 0;
        else current_frame_index = ((int)((Time.time - animation_start_time) / (1.0 / fps)) % animation.Length);
        GetComponent<SpriteRenderer>().sprite = animation[current_frame_index];
    }



    public void OnTriggerEnter(Collider coll)
    {
		if(coll.gameObject.CompareTag("Sword") || coll.gameObject.CompareTag("Explosion") || coll.gameObject.CompareTag("Boomerang"))
        {
            Hurt();
            Knockback(Vector3.left);
        }
    }

}
