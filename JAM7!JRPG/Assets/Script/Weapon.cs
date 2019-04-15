using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType : int
{
    GiantSword,
    LightSword,
    Hammer
}


public class Weapon : MonoBehaviour
{
    public float damage;

    public float animationFactor;

    public int AtkId = 0;

    public WeaponType type;

    public AnimationClip animation;
    
    public float hitStop;
    
    public float bouncingBackForce;

    private int level = 1;

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
    
    
    public void AcquireGun(int _level)
    {
        player = gameObject.GetComponentInParent<PlayerCombatController>();
        WeaponInfo info = WeaponManager._instance.getGunData(type);
    
        damage = info.damage[_level-1];
//        range = info.range[_level-1];
//        reloadSpeed = info.reloadSpeed[_level-1];
//        magazineSize = info.magazineSize[_level-1];
//        fireRate = info.fireRate[_level-1];
        level = _level;
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        WeaponManager._instance.generateDrop(player.transform.position,type,level);
        Destroy(gameObject);
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy && enemy.lastHitId != AtkId) 
            {
                // new attack
                // check for critical strike
                if (Random.value < player.Avatar.GetStatistic(StatisticType.CriticalChance))
                {
                    //then critical
                    enemy.getHit(AtkId, (int)(damage * player.Avatar.GetStatistic(StatisticType.BaseDamage) * player.Avatar.GetStatistic(StatisticType.CriticalDamage)));
                }
                else
                {
                    enemy.getHit(AtkId, (int)(damage * player.Avatar.GetStatistic(StatisticType.BaseDamage) * 1.0f));
                }
                
                enemy.knockBack(bouncingBackForce); 
                player.pauseAtkAnim(hitStop);
            }
        }
    }
    
}
