using UnityEngine;
using System.Collections;
using System;

public enum GelState { WALKING, PAUSED };

public class Gel : Enemy
{

    float cooldown;
    GelState state;
    Vector3 last;

    // Use this for initialization

    void Start()
    {
        base.Start();
        fps = 10;
    }

    public override void StartMovement()
    {
        state = GelState.WALKING;
        cooldown = UnityEngine.Random.Range(5f, 60f);
        SnapGrid();
        float v, h = Mathf.Round(UnityEngine.Random.Range(0f, 1f));
        last = transform.position;

        if (h == 0f) v = 1f;
        else v = 0f;

        int flip = UnityEngine.Random.Range(0, 2);
        if (flip == 1)
        {
            h *= -1f;
            v *= -1f;
        }

        this.GetComponent<Rigidbody>().velocity = new Vector3(h, v) * 2;
    }

    public override void UpdateMovement()
    {
        if(state == GelState.WALKING)
        {
            if(Vector3.Distance(transform.position, last) >= 1f)
            {
                state = GelState.PAUSED;
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                SnapGrid();
                return;
            }
            if (CheckCollision()) StartMovement();
            return;
        }
        if (state == GelState.PAUSED && cooldown > 0)
        {
            float time_delta_fraction = Time.deltaTime / (1.0f / Application.targetFrameRate);
            cooldown -= time_delta_fraction;
            if (cooldown < 0) cooldown = 0f;
        }
        if (cooldown == 0)
        {
            StartMovement();
            return;
        }

        
    }

    public override bool CheckCollision()
    {
        RaycastHit hit;
        Vector3 v = GetComponent<Rigidbody>().velocity;
        v = v / v.sqrMagnitude;
        Debug.DrawRay(transform.position, v * 2, Color.green);
        if (Physics.Raycast(transform.position, v * 2, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Map"))
            {
                if (hit.distance < 0.5f) return true;
            }
        }
        return base.CheckCollision();
    }
}