using UnityEngine;
using System.Collections;

public class Dialogue : MonoBehaviour {

    public string text = "EASTMOST PENINSULA\n   IS THE SECRET.";
    float roomX, roomY;
    float tick;
    bool on = false;

	// Use this for initialization
	void Start () {
        GetComponent<TextMesh>().text = "";
        roomX = Mathf.Floor((transform.position.x - 2) / 16f) * 16f + 2f;
        roomY = Mathf.Floor((transform.position.y - 2) / 11f) * 11f + 2f;
    }

    public bool isOnScreen()
    {
        if (RoomTransition.S.current_state == EntityState.TRANSITION) return false;
        Camera cam = RoomTransition.S.GetComponent<Camera>();
        Vector3 p = cam.transform.position;
        if (p.x > roomX && p.x < roomX + 8 && p.y > roomY && p.y < roomY + 6) return true;
        return false;
    }

    // Update is called once per frame
    void Update () {

        if (on)
        {
            tick += Time.deltaTime*6f;
            GetComponent<TextMesh>().text = text.Substring(0, Mathf.Min(text.Length, (int)Mathf.Floor(tick)));
        }



        if (isOnScreen() && !on)
        {
            on = true;
            tick = 0f;
        } 
        if(!isOnScreen() && on)
        {
            on = false;
            GetComponent<TextMesh>().text = "";
        }
	}
}
