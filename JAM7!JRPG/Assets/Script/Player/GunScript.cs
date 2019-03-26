using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GunScript:MonoBehaviour
{
    private float damage, range, reloadSpeed, fireRate, magazineSize, bulletsFired;
   
    private bool SpreadFire = false;
    private bool canFire = true, reloading = false;

    private enum currentgun { Shotgun,Machinegun,Sniper};

    public float metricScale;

    public KeyCode fireKey;
    public KeyCode reloadKey;
    private GameObject parent;
    private currentgun myGun;

    ////////////private functions below////////////////
    private void Start()
    {
        parent = GetComponent<Transform>().parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Fire(parent.GetComponent<PlayerController>().GetAllignment());
        if (Input.GetKeyDown(fireKey)&&canFire)
        {
            if (reloading && bulletsFired < magazineSize) {
                StopCoroutine(Reloading(reloadSpeed));
                reloading = false;
            }

            if (bulletsFired < magazineSize)
                Fire(parent.GetComponent<PlayerController>().GetAllignment());
        }
    }
    
    public void ChooseWeapon(PlayerController.gun x)
    {
        switch (x)
        {
            case PlayerController.gun.Shotgun:
                SetShotGun();
                myGun = currentgun.Shotgun;
                break;
            case PlayerController.gun.Machinegun:
                SetMachineGun();
                myGun = currentgun.Machinegun;
                    break;
            case PlayerController.gun.Sniper:
                SetSniper();
                myGun = currentgun.Sniper;
                break;
            default:
                SetMachineGun();
                break;
        }
    }


    // below functions set the basic stats for the gun(you can change the values to apply balance)
    void SetShotGun()
    {
        damage = 8;   
        range = 2;
        reloadSpeed = 2;
        magazineSize = 6;
        fireRate = 0.8f;
    }

    void SetMachineGun()
    {
        damage = 3;
        range = 5;
        reloadSpeed = 1.5f;
        magazineSize = 12;
        fireRate = 0.3f;
    }

    void SetSniper()
    {
        damage = 12;
        range = 10;
        reloadSpeed = 2;
        magazineSize = 10;
        fireRate = 1.2f;
    }

    void Reload()///function to call reloading of weapon ammo
    {
        reloading = true;
        StartCoroutine(Reloading(reloadSpeed));
    }

    private IEnumerator Firing(float x)
    {
        yield return new WaitForSeconds(x);
        canFire = true;
    }

    private IEnumerator Reloading(float x)
    {
        yield return new WaitForSeconds(x);
        bulletsFired = 0;
        reloading = false;
    }
    ///////////////

    //////////////////public functions
  

    public void Fire(Vector2 dir) // function to fire of weapon ammo
    {
        Debug.Log(1);
        bulletsFired++;

        Bullet bullet = ObjectPool.Singleton.Pop<Bullet>(0);
        bullet.isFriendly = true;
        bullet.range = range;

        bullet.GetComponent<LinearMovement>().initialPosition = parent.transform.position;
        bullet.GetComponent<LinearMovement>().orientation = dir;
        bullet.GetComponent<LinearMovement>().speed = 3;

        bullet.gameObject.SetActive(true);

        canFire = false;
        StartCoroutine(Firing(fireRate));
    }

    public void ApplyStats(int power,int dextirity, int wisdom)// applies stat buffs of the parent class of this script (which is the player)
    {
        damage *= (power * 0.1f);// increases damage by 10% for every point in power
        reloadSpeed *= (1 - dextirity * 0.1f); //increases decreses reloadtime by 10% for every point in dextirity
        range *= dextirity * 0.1f;// increases the range with dextirity(dextirity generally is sign of accuracy too)
        fireRate *= (1 - wisdom * 0.1f);//decreases the interval between successive bullet fire
    }

    public void ApplyUpgrade() //applies upgrade
    {
        if (myGun == currentgun.Shotgun)
        {
            damage += 4;
            range += 1;
            reloadSpeed -= 0.6f;
            magazineSize += 2;
            fireRate -= 0.2f;
        }
        else if (myGun == currentgun.Machinegun)
        {
            damage += 1;
            range += 1;
            reloadSpeed -= 0.5f;
            magazineSize += 10;
            fireRate -= 0.1f;
        }
        else if (myGun == currentgun.Sniper)
        {
            damage += 4;
            range += 5;
            reloadSpeed -= 0.4f;
            magazineSize += 5;
            fireRate -= 0.4f;
        }
    }

    public void UpdateStats(float a, float b, float c, float d, float e) //helper function to update gun stats to some specific value at any point if needed
    {
        damage = a;
        range = b;
        reloadSpeed = c;
        magazineSize = d;
        fireRate = e;
    } 
}
