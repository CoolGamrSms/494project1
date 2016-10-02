using UnityEngine;
using System.Collections;
using System;

public enum GoriyaState { WALKING, ATTACKING };

public class Goriya : Enemy
{

    float cooldown;
    float attackCooldown;
    public static GameObject BoomerangPrefab;
    GameObject boomerang;
    public static Sprite[] up;
    public static Sprite[] down;
    public static Sprite[] left;
    public static Sprite[] right;
    GoriyaState state;

    void Awake()
    {
        if (up == null)
        {
            up = new Sprite[2];
            left = new Sprite[2];
            right = new Sprite[2];
            down = new Sprite[2];

            left[0] = animation[0];
            left[1] = animation[1];
            down[0] = animation[2];
            down[1] = animation[3];
            right[0] = animation[4];
            right[1] = animation[5];
            up[0] = animation[6];
            up[1] = animation[7];

            
        }
        BoomerangPrefab = (GameObject)Resources.Load("EnemyBoomerang", typeof(GameObject));
    }
    // Use this for initialization
    void Start()
    {
        base.Start();
        state = GoriyaState.WALKING;
    }

    public override void StartMovement()
    {
        float roll = Mathf.Floor(UnityEngine.Random.Range(0f, 7f));
        if(roll == 0f && attackCooldown == 0f && GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            //Attack
            boomerang = (GameObject)Instantiate(BoomerangPrefab);
            boomerang.transform.position = this.transform.position;
            boomerang.GetComponent<enemyboom>().start = this.transform.position;
            boomerang.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity*3;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            SnapGrid();
            state = GoriyaState.ATTACKING;
            attackCooldown = 100f;
            fps = 0;
            return;
        }
        fps = 7;
        cooldown = UnityEngine.Random.Range(40f, 80f);
        SnapGrid();
        float v, h = Mathf.Round(UnityEngine.Random.Range(0f, 1f));

        if (h == 0f) v = 1f;
        else v = 0f;

        int flip = UnityEngine.Random.Range(0, 2);
        if (flip == 1)
        {
            h *= -1f;
            v *= -1f;
        }

        if (h == -1f) animation = left;
        else if (h == 1f) animation = right;
        else if (v == 1f) animation = up;
        else if (v == -1f) animation = down;
        this.GetComponent<Rigidbody>().velocity = new Vector3(h, v) * 2;
    }

    public override void UpdateMovement()
    {
        if(state == GoriyaState.ATTACKING)
        {
            if(boomerang == null)
            {
                state = GoriyaState.WALKING;
                StartMovement();
            }
            return;
        }
        if(cooldown > 0)
        {
            float time_delta_fraction = Time.deltaTime / (1.0f / Application.targetFrameRate);
            cooldown -= time_delta_fraction;
            if (cooldown < 0) cooldown = 0f;
        }
        if (attackCooldown > 0)
        {
            float time_delta_fraction = Time.deltaTime / (1.0f / Application.targetFrameRate);
            attackCooldown -= time_delta_fraction;
            if (attackCooldown < 0) attackCooldown = 0f;
        }

        if (Mathf.Round(transform.position.x * 10f) / 10f % 1.0f == 0)
            if (Mathf.Round(transform.position.y * 10f) / 10f % 1.0f == 0)
                if (cooldown == 0)
                {
                    StartMovement();
                    return;
                }
        if (CheckCollision()) StartMovement();
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