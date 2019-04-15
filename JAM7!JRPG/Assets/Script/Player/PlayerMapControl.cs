using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapControl : MonoBehaviour
{
	private SpriteRenderer renderer;
	private Rigidbody2D rb2d;
	private Animator anim;

	[SerializeField]
    private int id;

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
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		if (x > 0)
			renderer.flipX = false;

		else if (x < 0)
			renderer.flipX = true;

		rb2d.velocity = new Vector2(Mathf.Lerp(0, x, 0.6f), Mathf.Lerp(0, y, 0.6f));
	}
}
