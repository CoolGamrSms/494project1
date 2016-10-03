using UnityEngine;
using System.Collections;
using System;

public class Aquamentus : Enemy
{

    float cooldown;
    Vector3 goal, last;
    public Vector3 direction;
    Vector3 pvt_dir;
    public GameObject fireballPrefab;

    // Use this for initialization

    void Start()
    {
        pvt_dir = direction;
        base.Start();
        fps = 2;
    }

    void Fire()
    {
        SFXScript.S.FireShot();
        GameObject f1 = (GameObject)Instantiate(fireballPrefab);
        f1.transform.position = transform.position;
        f1.GetComponent<Rigidbody>().velocity = Vector3.left * 4;
        GameObject f2 = (GameObject)Instantiate(fireballPrefab);
        f2.transform.position = transform.position;
        f2.GetComponent<Rigidbody>().velocity = Vector3.left * 4 + Vector3.up;
        GameObject f3 = (GameObject)Instantiate(fireballPrefab);
        f3.transform.position = transform.position;
        f3.GetComponent<Rigidbody>().velocity = Vector3.left * 4 + Vector3.down;
    }
    public override void StartMovement()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        SnapGrid();
        last = transform.position;
        goal = last + pvt_dir * 2f;
        this.GetComponent<Rigidbody>().velocity = pvt_dir*0.7f;
    }

    public override void UpdateMovement()
    {
        if(Vector3.Distance(transform.position, goal) < 0.1f)
        {
            if (pvt_dir == Vector3.left) pvt_dir = Vector3.right;
            else if (pvt_dir == Vector3.right) pvt_dir = Vector3.left;
            StartMovement();
            Fire();
        }

        
    }

    public override bool CheckCollision()
    {
        return false;
    }
}