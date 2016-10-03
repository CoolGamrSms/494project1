using UnityEngine;
using System.Collections;

public class Keese : Enemy
{
	public bool boss1 = false;
	bool closed = false;
    float cooldown, dur, timer, deg;
    Vector3 direction;

    // Use this for initialization
    void Update()
    {
        base.Update();
        fps = (int)(GetComponent<Rigidbody>().velocity.sqrMagnitude/2f) + 1;
    }

    public override void StartMovement()
    {
        timer = -30f;
        dur = UnityEngine.Random.Range(350f, 600f);
        deg = UnityEngine.Random.Range(0, 7) * Mathf.PI / 4f;
        direction = new Vector3(Mathf.Cos(deg), Mathf.Sin(deg));
    }

    void ChangeDirection()
    {
        cooldown = UnityEngine.Random.Range(40f, 120f);
        deg += (UnityEngine.Random.Range(0, 2) * 2 - 1) * Mathf.PI / 4;
        direction = new Vector3(Mathf.Cos(deg), Mathf.Sin(deg));
    }

    public override void UpdateMovement()
    {

		if (boss1 && PlayerControl.instance.transform.position.y > 12.7f && !closed) { 
			ShowMapOnCamera.MAP[72, 12] = 018;
			ShowMapOnCamera.MAP[72, 11] = 002;
			ShowMapOnCamera.MAP[71, 12] = 018;
			ShowMapOnCamera.MAP[71, 11] = 002;
			ShowMapOnCamera.S.RedrawScreen(true);
			closed = true;
		}
        float time_delta_fraction = Time.deltaTime / (1.0f / Application.targetFrameRate);
        timer += time_delta_fraction;
        cooldown -= time_delta_fraction;

        if(timer >= dur)
        {
            StartMovement();
        }

        if (CheckCollision())
        {
            ChangeDirection();
            SnapBounds();
        }
        else if (cooldown < 0) ChangeDirection();

        float speed, psuedoTimer = Mathf.Max(0f, timer)/dur*2;
        if(psuedoTimer > 1.0f) speed = Mathf.Lerp(4f, 0f, psuedoTimer-1);
        else speed = Mathf.Lerp(0, 4f, psuedoTimer);

        GetComponent<Rigidbody>().velocity = direction * speed;
    }

}
