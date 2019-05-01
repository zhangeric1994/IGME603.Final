using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossBehaviour : int
{
    Generate = 0,
    BulletHell,
    Shake
}

public class Boss : Enemy
{
    private int lastBehaviourId;
    
    private bool idle;

    [SerializeField] private GameObject dummy;
    // Start is called before the first frame update
    void Start()
    {
        defaultState = EnemyState.ChaseMove;
        setState(defaultState);
        health = health * 1;
        idle = true;
        boss = true;
    }

    // Update is called once per frame
    void Update()
    {
        if ((findCloestPlayer().position -transform.position).sqrMagnitude <8f && idle && health > 0)
        {
            idle = false;
            int behaviourId = Random.Range(0, 3);
            while (lastBehaviourId == behaviourId)
            {
                behaviourId = Random.Range(0, 3);
            }
            
            lastBehaviourId = behaviourId;
            executeBehavior((BossBehaviour) behaviourId);
            Debug.LogWarning(behaviourId);
        }
    }
    
    private void executeBehavior(BossBehaviour behaviour)
    {
        var defaultColor = GetComponent<SpriteRenderer>().color;
        //GetComponent<Animator>().speed = 0;
        switch (behaviour)
        {
            case BossBehaviour.Generate:
                GetComponent<SpriteRenderer>().color = Color.blue;
                StartCoroutine(spawnDummy(10, 4, 4f, defaultColor));
                break;

            case BossBehaviour.BulletHell:
                GetComponent<SpriteRenderer>().color = Color.cyan;
                StartCoroutine(untargetedFire(4f,speed,defaultColor));
                
                break;
			
            case BossBehaviour.Shake:
                rb2d.AddForce(250 * transform.up);
                enemy_anim.SetBool("Attack", true);
                FMOD.Studio.EventInstance shakeSound = FMODUnity.RuntimeManager.CreateInstance("event:/Combat/BossHitGround");
                shakeSound.start();
                break;
        }

    }

    void shake()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetComponent<PlayerCombatController>().CurrentState != PlayerCombatState.InAir)
            {
                player.GetComponent<PlayerCombatController>().Hurt(1);
            }
        }
        var cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (var camera in cameras)
        {
            camera.GetComponent<ForwardCamera>().Shaking(0.2f,0.1f);
        }

        idle = true;
        enemy_anim.SetBool("Attack", false);
    }

    void fire(Vector2 dir)
    {
        Bullet bullet = ObjectPool.Singleton.Pop<Bullet>(1);
        bullet.isFriendly = false;
        bullet.range = 5;
        float dmg = 1.0f;
        bullet.rawDamage = (int) dmg;
        bullet.transform.rotation = Quaternion.identity;
        bullet.transform.right = dir;
        if (dir.x == 0 && dir.y == 0)
        {
            // prevent direction is zero 
            dir = transform.localScale.x < 0 ? -Vector2.right : Vector2.right;
        }

        bullet.GetComponent<LinearMovement>().initialPosition = transform.position - new Vector3(0,0.1f,0);
        bullet.GetComponent<LinearMovement>().orientation = dir;
        bullet.GetComponent<LinearMovement>().speed = 1.0f;
        
        bullet.gameObject.SetActive(true);
    }
    
    public Vector2 rotate(Vector2 v,float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
         
        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
    
    IEnumerator untargetedFire(float delay ,float _speed,Color defaultColor)
    {
        speed = 0;
        yield return new WaitForSeconds(1f);
        int counter = 0;
        int seconds = (int)Time.fixedUnscaledTime;
        int lastFiredSec = 0;
        while (counter < 2)
        {
            if (lastFiredSec != seconds)
            {
                var direction = -transform.right;
                for (float angle = 0f; angle < 180f; angle += 15f)
                {
                    fire(rotate(direction, -angle));
                    FMOD.Studio.EventInstance bossBulletSound = FMODUnity.RuntimeManager.CreateInstance("event:/Combat/BossBullet");
                    bossBulletSound.start();
                    //yield return new WaitForSeconds(0.1f);
                }
                lastFiredSec = seconds;
                counter++;
            }
            seconds = (int)Time.fixedUnscaledTime;
            yield return null;
        }
        GetComponent<SpriteRenderer>().color = defaultColor;
        GetComponent<Animator>().speed = 1;
        speed = _speed;



        yield return new WaitForSeconds(delay);
        idle = true;
    }
    
    IEnumerator spawnDummy(int total, int instant, float delay, Color defaultColor)
    {
        var temp = speed;
        speed = 0;
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < total - instant; i++)
        {
            Instantiate(dummy, transform.position,Quaternion.identity);
        }
        
        GetComponent<SpriteRenderer>().color = defaultColor;
        GetComponent<Animator>().speed = 1;
        speed = temp;
        yield return new WaitForSeconds(delay);
        idle = true;
    }
    
    
    private Transform findCloestPlayer()
    {
        float smallestDistance = 9999;
        Transform cloestPlayer = null;
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            var direction = player.transform.position - transform.position;
            // in the same level
            if (smallestDistance > direction.sqrMagnitude){
                smallestDistance = direction.sqrMagnitude;
                cloestPlayer = player.transform;
            }
        }
        return cloestPlayer;
    }
}
