using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPoint : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float force;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector3 getJumpForce()
    {
        return direction * force;
    }
}
