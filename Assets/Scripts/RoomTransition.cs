using UnityEngine;
using System.Collections;


public class RoomTransition : MonoBehaviour {

    public GameObject PlayerObj;

    public EntityState current_state;
    Vector3 goalPos;
    Vector3 startPos;
    float tween;

    public static RoomTransition S;

    void Awake()
    {
        S = this;
    }
    void Start()
    {

        // Use this for initialization
            // set the desired aspect ratio (the values in this example are
            // hard-coded for 16:9, but you could make them into public
            // variables instead so you can set them at design time)
            float targetaspect = 16f / 15f;

            // determine the game window's current aspect ratio
            float windowaspect = (float)Screen.width / (float)Screen.height;

            // current viewport height should be scaled by this amount
            float scaleheight = windowaspect / targetaspect;

            // obtain camera component so we can modify its viewport
            Camera camera = GetComponent<Camera>();

            // if scaled height is less than current height, add letterbox
            if (scaleheight < 1.0f)
            {
                Rect rect = camera.rect;

                rect.width = 1.0f;
                rect.height = scaleheight;
                rect.x = 0;
                rect.y = (1.0f - scaleheight) / 2.0f;

                camera.rect = rect;
            }
            else // add pillarbox
            {
                float scalewidth = 1.0f / scaleheight;

                Rect rect = camera.rect;

                rect.width = scalewidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scalewidth) / 2.0f;
                rect.y = 0;

                camera.rect = rect;
            }
        current_state = EntityState.NORMAL;
    }
	
	// Update is called once per frame
	void Update () {
	    
        //Check if link is at a horizontal door
        if(current_state == EntityState.NORMAL && PlayerObj.transform.position.x % 16 >= 15f)
        {
            float offset = PlayerControl.instance.current_direction == Direction.EAST ? 1f : -1f;
            float cameraShouldBeX = ((float)((int)((PlayerObj.transform.position.x+offset) / 16))*16)+7.5f;
            Vector3 pos = transform.position;
            pos.x = cameraShouldBeX;
            goalPos = pos;
            startPos = transform.position;
            tween = 0f;
            current_state = EntityState.TRANSITION;
            PlayerControl.instance.current_state = EntityState.TRANSITION;
        }
        if (current_state == EntityState.NORMAL && PlayerObj.transform.position.y % 11 >= 10f)
        {
            float offset = PlayerControl.instance.current_direction == Direction.NORTH ? 1f : -1f;
            float cameraShouldBeY = ((float)((int)((PlayerObj.transform.position.y + offset) / 11)) * 11) + 6.5f;
            Vector3 pos = transform.position;
            pos.y = cameraShouldBeY;
            goalPos = pos;
            startPos = transform.position;
            tween = 0f;
            current_state = EntityState.TRANSITION;
            PlayerControl.instance.current_state = EntityState.TRANSITION;
        }
        else if(current_state == EntityState.TRANSITION)
        {
            tween += 0.01f;
            if(tween >= 1f)
            {
                transform.position = goalPos;
                float time_delta_fraction = Time.deltaTime / (1.0f / Application.targetFrameRate);
                //Autowalk after entering room
                if (PlayerControl.instance.current_direction == Direction.EAST && (PlayerObj.transform.position.x + 1f) % 16 < 2.0f)
                {
                    PlayerControl.instance.GetComponent<Rigidbody>().velocity = new Vector3(1f, 0f, 0)
                                                                                * PlayerControl.instance.walking_velocity
                                                                                * time_delta_fraction;
                    return;
                }
                else if (PlayerControl.instance.current_direction == Direction.WEST && (PlayerObj.transform.position.x) % 16 > 14.0f)
                {
                    PlayerControl.instance.GetComponent<Rigidbody>().velocity = new Vector3(-1f, 0f, 0)
                                                                                * PlayerControl.instance.walking_velocity
                                                                                * time_delta_fraction;
                    return;
                }
                else if (PlayerControl.instance.current_direction == Direction.NORTH && (PlayerObj.transform.position.y + 1f) % 11 < 2.0f)
                {
                    PlayerControl.instance.GetComponent<Rigidbody>().velocity = new Vector3(0f, 1f, 0)
                                                                                * PlayerControl.instance.walking_velocity
                                                                                * time_delta_fraction;
                    return;
                }
                else if (PlayerControl.instance.current_direction == Direction.SOUTH && (PlayerObj.transform.position.y) % 11 > 9f)
                {
                    PlayerControl.instance.GetComponent<Rigidbody>().velocity = new Vector3(0f, -1f, 0)
                                                                                * PlayerControl.instance.walking_velocity
                                                                                * time_delta_fraction;
                    return;
                }
                current_state = EntityState.NORMAL;
                PlayerControl.instance.current_state = EntityState.NORMAL;
                return;
            }
            else
            {
                PlayerControl.instance.GetComponent<Rigidbody>().velocity = Vector3.zero; //Freeze link during transitions
            }
            transform.position = Vector3.Lerp(startPos, goalPos, tween);
        }

	}
}
