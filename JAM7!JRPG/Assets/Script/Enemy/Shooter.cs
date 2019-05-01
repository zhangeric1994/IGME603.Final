using System.Collections;
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
        if (targetCharacter == null) return;
        if (time % fireRate == 0 && lastShotTime != time && (targetCharacter.position - transform.position).sqrMagnitude < 5f)
        {
            //fire 
            lastShotTime = time;
            StartCoroutine(shot());

        }
    }

    void fire(Vector2 dir)
    {
        Bullet bullet = ObjectPool.Singleton.Pop<Bullet>(1);
        bullet.isFriendly = false;
        bullet.range = 5;
        float dmg = 1.0f;
        bullet.rawDamage = (int)dmg;
        bullet.transform.rotation = Quaternion.identity;
        bullet.transform.right = dir;
        if (dir.x == 0 && dir.y == 0)
        {
            // prevent direction is zero 
            dir = transform.localScale.x < 0 ? -Vector2.right : Vector2.right;
        }

        bullet.GetComponent<LinearMovement>().initialPosition = transform.position;
        bullet.GetComponent<LinearMovement>().orientation = dir;
        bullet.GetComponent<LinearMovement>().speed = 1.0f;

        bullet.gameObject.SetActive(true);
        FMOD.Studio.EventInstance fireball = FMODUnity.RuntimeManager.CreateInstance("event:/Combat/EnemyBullet");
        fireball.start();
    }

    IEnumerator shot()
    {      
        float defaultSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(1f);
        //fire
        Vector2 dir = (targetCharacter.position - transform.position).normalized;
        dir.y = 0;
        dir.x = dir.x > 0 ? 1.0f : -1.0f;
        fire(dir);
        yield return new WaitForSeconds(0.1f);
        speed = defaultSpeed;
    }
}
