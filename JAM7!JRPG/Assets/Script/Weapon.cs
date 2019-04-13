using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public float damage;

    public float animationFactor;

    public int AtkId = 0;

    public AnimationClip animation;
    
    public float hitStop;

    private PlayerCombatController player;

    private void Start()
    {
         player = gameObject.GetComponentInParent<PlayerCombatController>();
    }

    public void setAttackId()
    {
        AtkId++;
        if (AtkId > 10000)
        {
            AtkId = 0;
        }
    }

    public string getAnimationName()
    {
        return animation.name;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy && enemy.lastHitId != AtkId) 
            {
                // new attack
                enemy.getHit(AtkId,(int)damage);
                enemy.knockBack(); 
                player.pauseAtkAnim(hitStop);
            }
        }
    }
}
