using UnityEngine;
using System.Collections;

public class SFXScript : MonoBehaviour {

    public AudioClip keyAppear;
    public AudioClip itemObtain;
    public AudioClip itemReceive;
    public AudioClip enemyHit;
    public AudioClip playerHit;
    public AudioClip bombSound;
    public AudioClip openDoor;
    public AudioClip fireShot;
    public AudioClip stairsSound;
    public static SFXScript S;
	// Use this for initialization
	void Awake () {
        if(S == null) S = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void KeyAppear()
    {
        GetComponent<AudioSource>().PlayOneShot(keyAppear);
    }

    public void ItemObtain()
    {
        GetComponent<AudioSource>().PlayOneShot(itemObtain);
    }

    public void ItemReceive()
    {
        GetComponent<AudioSource>().PlayOneShot(itemReceive);
    }

    public void EnemyHit()
    {
        GetComponent<AudioSource>().PlayOneShot(enemyHit);
    }

    public void PlayerHit()
    {
        GetComponent<AudioSource>().PlayOneShot(playerHit);
    }
    public void BombSound()
    {
        GetComponent<AudioSource>().PlayOneShot(bombSound);
    }
    public void OpenDoor()
    {
        GetComponent<AudioSource>().PlayOneShot(openDoor);
    }
    public void StairsSound()
    {
        GetComponent<AudioSource>().PlayOneShot(stairsSound);
    }
    public void FireShot()
    {
        GetComponent<AudioSource>().PlayOneShot(fireShot);
    }
}
