using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    float Fade;
	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, 1.0f);
        Fade = -0.4f;
	}

    void Update()
    {
        Fade += Time.deltaTime;
        foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()) 
            sr.color = new Color(1f, 1f, 1f, Random.Range(0.6f-Fade, 1f-Fade));
    }
}
