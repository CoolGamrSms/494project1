using UnityEngine;
using System.Collections;

public enum BladeTrapState { ATTACK, STANDBY, RESET };
public enum BladeTrapHorizontal { LEFT, RIGHT };
public enum BladeTrapVertical { UP, DOWN };

public class bladetrap : MonoBehaviour {

    BladeTrapState state;
    public BladeTrapHorizontal h;
    public BladeTrapVertical v;

    Vector3 goal, start;

	// Use this for initialization
	void Start () {
        state = BladeTrapState.STANDBY;
        start = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        //Player detection
        if(state == BladeTrapState.ATTACK)
        {
            if (Vector3.Distance(transform.position, goal) < 0.1f)
            {
                GetComponent<Rigidbody>().velocity *= -0.25f;
                state = BladeTrapState.RESET;
            }
        }
        else if (state == BladeTrapState.RESET)
        {
            if (Vector3.Distance(transform.position, start) < 0.1f)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                state = BladeTrapState.STANDBY;
                transform.position = start;
            }
        }
        else if (state == BladeTrapState.STANDBY)
        {
            RaycastHit hit;
            Vector3 hor, hor1s, hor2s, ver;
            if (h == BladeTrapHorizontal.LEFT) hor = Vector3.right * 11;
            else hor = Vector3.left * 11;

            hor1s = transform.position;
            hor2s = transform.position;
            hor1s.y -= 0.25f;
            hor2s.y += 0.25f;

            if (v == BladeTrapVertical.UP) ver = Vector3.down * 6;
            else ver = Vector3.up * 6;

            Debug.DrawRay(hor1s, hor, Color.green);
            Debug.DrawRay(hor2s, hor, Color.green);
            Debug.DrawRay(transform.position, ver, Color.green);
            if (Physics.Raycast(hor1s, hor, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Player")) StartMoveHorizontal();
            }
            if (Physics.Raycast(hor2s, hor, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Player")) StartMoveHorizontal();
            }
            if (Physics.Raycast(transform.position, ver, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Player")) StartMoveVertical();
            }
        }

    }

    void StartMoveHorizontal()
    {
        state = BladeTrapState.ATTACK;
        goal = start;
        float mult = ((h == BladeTrapHorizontal.RIGHT) ? -1f : 1f);
        goal.x +=   5f * mult;
        float mag = 6f * mult;
        GetComponent<Rigidbody>().velocity = new Vector3(mag, 0);
    }
    void StartMoveVertical()
    {
        state = BladeTrapState.ATTACK;
        goal = start;
        float mult = ((v == BladeTrapVertical.UP) ? -1f : 1f);
        goal.y += 2.5f * mult;
        Debug.Log(goal);
        float mag = 6f * mult;
        GetComponent<Rigidbody>().velocity = new Vector3(0, mag);
    }
}
