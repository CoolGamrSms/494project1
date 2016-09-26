using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	//float delay = 2.0f; //This implies a delay of 2 seconds.
	//GameObject explosion;
	public GameObject explosion;

	IEnumerator WaitForExplosion() {

		yield return new WaitForSeconds(2);
		Destroy (this.gameObject);
		GameObject go = Instantiate(explosion, this.transform.position, Quaternion.identity) as GameObject;
	}

	// Use this for initialization
	void Start () {

		StartCoroutine (WaitForExplosion ());

	}

	// Update is called once per frame
	void Update () {
	
	}
}
