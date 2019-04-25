using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crack : MonoBehaviour {

public GameObject finalBoss;

/// <summary>
/// Sent when another object enters a trigger collider attached to this
/// object (2D physics only).
/// </summary>
/// <param name="other">The other Collider2D involved in this collision.</param>
void OnTriggerEnter2D(Collider2D other) {
    if (other.name == "HitBox" | other.tag == "Player" & this.name == "crank-down"){
        Debug.Log(other.name);

        FinalBoss finalBossSrc = finalBoss.GetComponent<FinalBoss>();

        finalBossSrc.enableCrackUp();
        finalBossSrc.disableCrackDown();
        finalBossSrc.isInCrack = true;
    }
    
}

}
