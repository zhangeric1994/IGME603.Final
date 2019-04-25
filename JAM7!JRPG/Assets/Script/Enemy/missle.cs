using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missle : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Hitbox" | other.name == "PlayerCombat(Clone)"){
            other.gameObject.GetComponentInParent<PlayerCombatController>().Hurt(1);
            Destroy(gameObject);
            return;
        }

        if (other.name != "boss") {
            Destroy(gameObject);}


    }
}
