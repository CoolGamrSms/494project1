using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	public Sprite[] animation;
	float animation_start_time;
	public int fps;

	// Use this for initialization
	void Start () {
		animation_start_time = Time.time;
		Destroy (this.gameObject, 2.0f);
	}
	
	// Update is called once per frame
	void Update () {
		int current_frame_index = ((int)((Time.time - animation_start_time) / (1.0 / fps)) % animation.Length);
		GetComponent<SpriteRenderer>().sprite = animation[current_frame_index];
	}
}
