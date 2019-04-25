using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EvilBehaviour : int
{
    Idle = 0,
    Generate,
    Dash,
    Shake
}

public class EvilBoss : Enemy
{
    private int lastBehaviourId;
    
    private bool idle;
    
    public GameObject bossObj;

    [SerializeField] private GameObject dummy;
    private bool triggered;
    // Start is called before the first frame update
    void Start()
    {
        defaultState = EnemyState.ChaseMove;
        setState(defaultState);
        health = health * 1;
        health = 15;
        idle = true;
        boss = true;
        triggered = false;
        executeBehavior(EvilBehaviour.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        if ((findClosestPlayer().position -transform.position).sqrMagnitude <8f && idle)
        {
            idle = false;
            int behaviourId = Random.Range(0, 4);
            while (lastBehaviourId == behaviourId)
            {
                behaviourId = Random.Range(0, 4);
            }
            
            lastBehaviourId = behaviourId;
            executeBehavior((EvilBehaviour) behaviourId);
        }

        if (health < 100 && ! triggered)
        {
            triggered = true;
            StartCoroutine(SpawnBoss());
        }
    }
    
    private void executeBehavior(EvilBehaviour behaviour)
    {
        var defaultColor = GetComponent<SpriteRenderer>().color;
        //GetComponent<Animator>().speed = 0;
        switch (behaviour)
        {
            case EvilBehaviour.Idle:
                StartCoroutine(Idle());
                break;
            case EvilBehaviour.Generate:
                GetComponent<SpriteRenderer>().color = Color.blue;
                StartCoroutine(spawnDummy(4f, defaultColor));
                break;

            case EvilBehaviour.Dash:
                GetComponent<SpriteRenderer>().color = Color.cyan;
                StartCoroutine(dash(defaultColor));
                break;
			
            case EvilBehaviour.Shake:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                rb2d.AddForce(250 * transform.up);
                enemy_anim.SetBool("Attack", true);
                break;
        }

    }
    
    private IEnumerator Idle()
    {
        idle = false; 
        var temp = speed;
        speed = 0;
        yield return new WaitForSeconds(1.0f);
        speed = temp;
        yield return new WaitForSeconds(2f);
        idle = true;
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
        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.GetComponent<ForwardCamera>().Shaking(0.2f,0.1f);
        idle = true;
        enemy_anim.SetBool("Attack", false);
    }

    private IEnumerator dash(Color defaultColor)
    {
        var temp = speed;
        speed = 0;
        yield return new WaitForSeconds(2f);
        var direction = findClosestPlayer().position - transform.position;
        direction = direction.x < 0 ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0); 
        damage = 2;
        rb2d.AddForce(250 * direction);
        yield return new WaitForSeconds(1.0f);
        damage = 1;
        GetComponent<SpriteRenderer>().color = defaultColor;
        speed = temp;
        yield return new WaitForSeconds(4f);
        idle = true;
    }
    //spawn boss after animation finished 
    IEnumerator SpawnBoss(){
        //destroy this one
        //small animation here
        gameObject.GetComponent<SpriteRenderer>().color -= new Color(0,0,0,0.2f);
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<SpriteRenderer>().color -= new Color(0,0,0,0.2f);
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<SpriteRenderer>().color -= new Color(0,0,0,0.2f);
        yield return new WaitForSeconds(0.5f);
        //GameObject _boss = Instantiate(boss, gameObject.transform.position, Quaternion.identity);
    }
    

    IEnumerator spawnDummy(float delay, Color defaultColor)
    {
        var temp = speed;
        speed = 0;
        yield return new WaitForSeconds(1f);
        Instantiate(dummy, transform.position,Quaternion.identity);
        GetComponent<SpriteRenderer>().color = defaultColor;
        GetComponent<Animator>().speed = 1;
        speed = temp;
        yield return new WaitForSeconds(delay);
        idle = true;
    }
    
    
    private Transform findClosestPlayer()
    {
        float smallestDistance = 9999;
        Transform closestPlayer = null;
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            var direction = player.transform.position - transform.position;
            // in the same level
            if (smallestDistance > direction.sqrMagnitude){
                smallestDistance = direction.sqrMagnitude;
                closestPlayer = player.transform;
            }
        }
        return closestPlayer;
    }
}
