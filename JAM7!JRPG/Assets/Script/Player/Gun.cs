using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//new stuff


public class Gun : MonoBehaviour
{
    public float damage, range, reloadSpeed, fireRate, magazineSize, bulletsFired;
    private bool SpreadFire = false;
    private bool canFire = true, reloading = false;

    private float reloadingStart, reloadingDuration;
    
    private float lastFire;
    public GunType type;
    
    
    private PlayerCombatController player;
    private int level;
    
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            AcquireGun(1);
        }
        float time = Time.unscaledTime;
        switch (type)
        {
                case GunType.MachineGun:
                    if (Input.GetButton("Fire" + player.PlayerID) && lastFire + fireRate < time)
                    {
                        if (bulletsFired < magazineSize)
                        {
                            Fire(player.GetAllignment());
                        }
                        else
                        {
                            reloadingStart = time;
                            reloadingDuration = reloadSpeed  / (1.0f + player.Dexterity*0.1f);
                            reloading = true;
                            StartCoroutine(Reloading());
                        }

                        lastFire = time;
                    }
                    break;
                
                default:
                    if (Input.GetButtonDown("Fire" + player.PlayerID) && lastFire + fireRate < time)
                    {
                        if (bulletsFired < magazineSize)
                        {
                            Fire(player.GetAllignment());
                        }
                        else
                        {
                            reloadingStart = time;
                            reloadingDuration = reloadSpeed  / (1.0f + player.Dexterity*0.1f);
                            reloading = true;
                            StartCoroutine(Reloading());
                        }

                        lastFire = time;
                    }
                    break;
        }
       

        if (Input.GetButtonDown("Reload" + player.PlayerID) &&!reloading && bulletsFired != 0)
        {
            reloadingStart = time;
            reloadingDuration = reloadSpeed / (1.0f + player.Dexterity*0.1f);
            reloading = true;
            StartCoroutine(Reloading());
        }
    }
    
    void Fire(Vector2 dir)
    {
        
        bulletsFired++;
        
        Bullet bullet = ObjectPool.Singleton.Pop<Bullet>(0);
        bullet.isFriendly = true;
        bullet.range = range;
        float dmg = damage * (1.0f + player.Power*0.1f);
        bullet.rawDamage = (int) dmg;
        bullet.transform.rotation = Quaternion.identity;
        bullet.transform.right = dir;
        if (dir.x == 0 && dir.y == 0)
        {
            // prevent direction is zero 
            dir = player.GetComponent<SpriteRenderer>().flipX ? -Vector2.right : Vector2.right;
        }

        bullet.GetComponent<LinearMovement>().initialPosition = player.transform.position;
        bullet.GetComponent<LinearMovement>().orientation = dir;
        bullet.GetComponent<LinearMovement>().speed = 3.0f;

        bullet.gameObject.SetActive(true);

        //canFire = false;
        //StartCoroutine(Firing(fireRate));
    }
    
    public void AcquireGun(int _level)
    {
        player = gameObject.GetComponentInParent<PlayerCombatController>();
        GunInfo info = GunManager._instance.getGunData(type);
    
        damage = info.damage[_level-1];
        range = info.range[_level-1];
        reloadSpeed = info.reloadSpeed[_level-1];
        magazineSize = info.magazineSize[_level-1];
        fireRate = info.fireRate[_level-1];
        level = _level;
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        GunManager._instance.generateDrop(player.transform.position,type,level);
        Destroy(gameObject);
    }


    private IEnumerator Firing(float x)
    {
        yield return new WaitForSeconds(x);
        canFire = true;
    }

    private IEnumerator Reloading()
    {
        while (reloadingStart + reloadingDuration >= Time.unscaledTime)
        {
            yield return new WaitForSeconds(0.1f);
            //do updates in ui here
            
        }
        
        bulletsFired = 0;
        reloading = false;
    }
}