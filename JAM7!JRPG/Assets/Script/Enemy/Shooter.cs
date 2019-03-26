using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy
{

    //shooting related
    private int lastShotTime;
    private int fireRate;
    // Start is called before the first frame update
    void Start()
    {
        defaultState = EnemyState.CircleMove;
        setState(defaultState);
        fireRate = Random.Range(4, 6);
        distanceConstraint = Random.Range(3, 7);
    }

    // Update is called once per frame
    void Update()
    {
        int time = (int)Time.unscaledTime;
        if(time%fireRate == 0 && lastShotTime != time)
        {
            //fire 
            lastShotTime = time;
            
            StartCoroutine(shot());
            
        }
    }

    IEnumerator shot() {
        float defaultSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(1f);
        //fire
        print("shot");
        yield return new WaitForSeconds(0.1f);
        speed = defaultSpeed;
    }
}
