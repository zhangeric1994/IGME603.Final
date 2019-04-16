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

    void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Hitbox"){
            move = -1;
            return;
        }

        if (other.name == "PlayerCombat"){
            Debug.Log("Player gets damage.");
            return;
        }

        if (other.name == "boss"){
            other.GetComponent<FinalBoss>().fireballDamage();
            Destroy(gameObject);
        }

        //Debug.Log(other.name);
    }
}
