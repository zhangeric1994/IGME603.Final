using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrtollerForTest : MonoBehaviour {
    public float speed = 10;
	private Rigidbody2D rigidPlayer;
	// Use this for initialization
	void Start () {
		rigidPlayer = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		rigidPlayer.MovePosition(rigidPlayer.position + new Vector2(speed * Time.deltaTime * Input.GetAxis("Horizontal"), speed * Time.deltaTime * Input.GetAxis("Vertical")));
	}
}
