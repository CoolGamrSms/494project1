using UnityEngine;
using System.Collections;
using System;

public class Wallmaster : Enemy
{

    float cooldown;
    Vector3 goal, last;
    public Vector3 direction;
    Vector3 pvt_dir;
    bool randomness;

    // Use this for initialization

    void Start()
    {
        pvt_dir = direction;
        randomness = true;
        base.Start();
        fps = 5;
    }

    public override void StartMovement()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        SnapGrid();
        last = transform.position;
        goal = last + pvt_dir * 2;
        if (randomness)
        {
            Invoke("StartMovement", UnityEngine.Random.Range(0f, 3f));
            randomness = false;
            return;
        }
        this.GetComponent<Rigidbody>().velocity = pvt_dir * 2;
    }

    public override bool isOnScreen()
    {
        return true;
    }

    public override void UpdateMovement()
    {
        if(!randomness && Vector3.Distance(transform.position, goal) < 0.1f)
        {
            if (pvt_dir == Vector3.right) pvt_dir = Vector3.down;
            else if (pvt_dir == Vector3.down) pvt_dir = Vector3.left;
            else if (pvt_dir == Vector3.left) pvt_dir = Vector3.up;
            else if (pvt_dir == Vector3.up) pvt_dir = Vector3.right;
            if (pvt_dir == direction) randomness = true;
            StartMovement();
        }

        
    }

    public override bool CheckCollision()
    {
        return false;
    }
}