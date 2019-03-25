using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GunScript:MonoBehaviour
{
    private float damage, range, reloadSpeed, fireRate, magazineSize, bulletsFired;
    private bool SpreadFire = false;
    private bool canFire = true, reloading = false;

    public float metricScale;

    public KeyCode fireKey;
    public KeyCode reloadKey;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(fireKey)&&canFire)
        {
            if (reloading && bulletsFired < magazineSize) {
                StopCoroutine(Reloading(reloadSpeed));
                reloading = false;
            }

            if (bulletsFired < magazineSize)
                Fire();
        }
    }
    
    public void ChooseWeapon(PlayerController.gun x)
    {
        switch (x)
        {
            case PlayerController.gun.Shotgun:
                SetShotGun();
                break;
            case PlayerController.gun.Machinegun:
                SetMachineGun();
                break;
            case PlayerController.gun.Sniper:
                SetSniper();
                break;
            default:
                SetMachineGun();
                break;
        }
    }

    void SetShotGun()
    {
        damage = 15;
        range = 2;
        reloadSpeed = 3;
        magazineSize = 6;
        fireRate = 1;
    }

    void SetMachineGun()
    {
        damage = 1;
        range = 5;
        reloadSpeed = 2;
        magazineSize = 100;
        fireRate = 0.1f;
    }

    void SetSniper()
    {
        damage = 20;
        range = 10;
        reloadSpeed = 2;
        magazineSize = 10;
        fireRate = 1;
    }

    void Fire()
    {
        bulletsFired++;
        //Use a function where you pass in the damage values and range*metricScale for the instantiating the bullets from the bullet class.
        canFire = false;
        StartCoroutine(Firing(fireRate));
    }

    void Reload()
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
}
