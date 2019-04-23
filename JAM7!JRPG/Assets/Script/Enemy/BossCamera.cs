using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour{
    void Start(){
        ForwardCamera.getInstance().setTarget(gameObject.transform);
    }
}
