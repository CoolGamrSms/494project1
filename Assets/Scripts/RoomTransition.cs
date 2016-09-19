using UnityEngine;
using System.Collections;


public class RoomTransition : MonoBehaviour {

    public GameObject PlayerObj;

    public EntityState current_state;
    public Direction current_direction;

    // Use this for initialization
    void Awake () {
        Screen.fullScreen = false;
        Screen.SetResolution(512, 480, false);
	}
	
	// Update is called once per frame
	void Update () {
	    
        //Check if link is at a horizontal door
        if(PlayerObj.transform.position.x % 16 >= 15f)
        {
            float offset = PlayerControl.instance.current_direction == Direction.EAST ? 1f : -1f;
            float cameraShouldBeX = ((float)((int)((PlayerObj.transform.position.x+offset) / 16))*16)+7.5f;
            Vector3 pos = transform.position;
            pos.x = cameraShouldBeX;
            transform.position = pos;
        }

	}
}
