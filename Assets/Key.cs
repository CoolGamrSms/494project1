using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

    public bool attached = false;
    public GameObject attachedTo;
    public GameObject[] mustDie;
	// Use this for initialization
	void Start () {
	    if(!attached)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
	
	// Update is called once per frame
	void LateUpdate () {
	    if(attached && attachedTo != null)
        {
            transform.position = attachedTo.transform.position;
        }
        else if(attached)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        else if (gameObject.GetComponent<Renderer>().enabled == false)
        {
            foreach (GameObject go in mustDie) if (go != null) return;
            gameObject.GetComponent<BoxCollider>().enabled = true;
            gameObject.GetComponent<Renderer>().enabled = true;
        }
	}
}
