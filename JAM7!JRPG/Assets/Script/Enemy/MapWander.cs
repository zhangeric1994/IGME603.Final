using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWander : MonoBehaviour
{
	[SerializeField]
	private float maxStray;
	private Vector3 startPos;
	private bool wait = false;
	private float waitTime = 0.0f;
	Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
		rb2d = GetComponent<Rigidbody2D>();
		startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		if(!wait)
		{
			if(transform.position.x > startPos.x + maxStray || transform.position.y > startPos.y + maxStray)
			{
				Vector3 sub = transform.position - startPos;
				rb2d.velocity = new Vector2(sub.x, sub.y).normalized;
			}

			rb2d.velocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

			wait = (Random.Range(0.0f, 1.0f) > 0.7f) ? true : false;
		}

		else
		{
			waitTime += Time.deltaTime;

			if(waitTime > 1.0f){
				wait = false;
			}
		}
    }

    void OnCollisionEnter(Collision coll)
	{
		//TODO: Switch scenes here
	}
}
