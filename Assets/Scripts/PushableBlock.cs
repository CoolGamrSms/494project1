using UnityEngine;
using System.Collections;

public class PushableBlock : MonoBehaviour {

    public bool isTrigger = false;
    public int doorX, doorY;
    public GameObject[] killFirst;
    bool moved = false;
    bool complete = false;
    Vector3 goal;

	// Use this for initialization
	void Start () {
        goal = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (moved)
        {
            if (transform.position != goal)
            {
                transform.position = Vector3.MoveTowards(transform.position, goal, Time.deltaTime);
            }
            else if (!complete && isTrigger)
            {
                ShowMapOnCamera.MAP[doorX, doorY] = 51;
                ShowMapOnCamera.S.RedrawScreen(true);
                SFXScript.S.OpenDoor();
                complete = true;
            }
        }
	}

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.CompareTag("Player"))
        {
            if (moved) return;

            //Check if the requisite enemies are dead
            foreach(GameObject go in killFirst)
            {
                if (go != null) return;
            }
            //Check if the player is in the right position
            PlayerControl pc = coll.gameObject.GetComponent<PlayerControl>();
            if(pc.current_direction == Direction.NORTH)
            {
                if (coll.gameObject.transform.position.x == transform.position.x)
                {
                    if (coll.gameObject.transform.position.y < transform.position.y)
                    {
                        moved = true;
                        Vector3 p = transform.position;
                        p.y += 1;
                        goal = p;
                    }
                }
            }
        }
    }
}
