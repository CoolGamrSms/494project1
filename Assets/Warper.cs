using UnityEngine;
using System.Collections;

public class Warper : MonoBehaviour {

    public float playerX, playerY;
    public float camX, camY;
    public GameObject MainCamera;
    bool shouldWarp = false;


	void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.CompareTag("Player"))
        {
            coll.gameObject.transform.position = new Vector3(playerX, playerY);
            Vector3 p = MainCamera.transform.position;
            p.x = camX;
            p.y = camY;
            MainCamera.transform.position = p;
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            coll.gameObject.transform.position = new Vector3(playerX, playerY);
            Vector3 p = MainCamera.transform.position;
            p.x = camX;
            p.y = camY;
            MainCamera.transform.position = p;
        }
    }
}
