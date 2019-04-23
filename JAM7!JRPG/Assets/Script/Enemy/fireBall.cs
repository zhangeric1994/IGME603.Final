using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBall : MonoBehaviour {
    private int move;

    void Start(){
        move = 1;
    }
    void Update() {
        gameObject.transform.position -= new Vector3(Time.deltaTime, 0.0f, 0.0f) * move;
    }

    public void setMove(){
        move = -1;
    }

    void OnTriggerEnter2D(Collider2D other) {
        // if (other.name == "Hitbox"){
        //     move = -1;
        //     return;
        // }

        if (other.name == "Hitbox"){
            Debug.Log("Player gets damage.");
            other.gameObject.GetComponentInParent<PlayerCombatController>().Hurt(4);
            Destroy(gameObject);
            return;
        }

        if (other.name == "boss"){
            other.GetComponent<FinalBoss>().fireballDamage();
            Destroy(gameObject);
        }
        
        Debug.Log(other.name);

    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name == "Wall"){
            Destroy(gameObject);
        }
    }
}
