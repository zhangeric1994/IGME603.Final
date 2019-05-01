using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniFireball : MonoBehaviour {
    private float forceX;
    private float forceY;

    private Rigidbody2D rb2d;

    private int forceFactor = 20;

    void Awake(){
        forceX = 0;
        forceY = 0;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        if (!rb2d) Debug.Log("mini fire ball rigidbody not loaded");
    }

    //Todo:
    //1. deal with fireball outside the screen

    void Update(){
        //gameObject.transform.position += new Vector3(Time.deltaTime * forceX, Time.deltaTime * forceY, 0.0f);
    }

    public void setForce(float _forceX, float _forceY){
        forceX = _forceX;
        forceY = _forceY;

        rb2d.AddForce(new Vector2(_forceX * forceFactor, _forceY * forceFactor));
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.name != "boss" & other.name != "Hitbox" & other.tag != "fireball"){
            Destroy(gameObject);
        }
        
        if (other.name == "Hitbox"){
            other.gameObject.GetComponentInParent<PlayerCombatController>().Hurt(1);
            Destroy(gameObject);
        }
    }
}
