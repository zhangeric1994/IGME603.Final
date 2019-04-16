using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skull : MonoBehaviour {
    private bool isMove;

    private Rigidbody2D rb2d;

    void Start(){
        isMove = false;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update(){
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "boss"){
            other.GetComponent<FinalBoss>().SkullDamage();
            Debug.Log("hit boss");
            Destroy(gameObject);
            return;
        }

        if (other.name == "HitBox" | other.name == "PlayerCombat"){
            rb2d.AddForce(new Vector2(0.0f, 10.0f));
        }
    }
}
