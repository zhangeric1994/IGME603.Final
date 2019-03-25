using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapControl : MonoBehaviour
{
	private SpriteRenderer renderer;
	private Rigidbody2D rb2d;
	private Animator anim;

	// Use this for initialization
	void Start()
	{
		renderer = GetComponent<SpriteRenderer>();
		rb2d = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		float x = Input.GetAxis("Horizontal" + Id);
		float y = Input.GetAxis("Vertical" + Id);

        if (x != 0 && y != 0)
			aimmingDirection = new Vector2(x, Mathf.Clamp01(y)).normalized;

		//anim.SetFloat("Speed",Mathf.Abs(h)+Mathf.Abs(v));

		if (x > 0)
		{
			rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);
			renderer.flipX = false;
		}

		else if (x < 0)
		{
			rb2d.velocity = new Vector2(-walkSpeed, rb2d.velocity.y);
			renderer.flipX = true;
		}

		else
			rb2d.velocity = new Vector2(0, rb2d.velocity.y);

		if (Input.GetButtonDown("Jump" + Id))
			CurrentState = PlayerState.InAir;
	}
}
