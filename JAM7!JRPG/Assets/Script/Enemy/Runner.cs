using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        defaultState = EnemyState.ChaseMove;
        setState(defaultState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
