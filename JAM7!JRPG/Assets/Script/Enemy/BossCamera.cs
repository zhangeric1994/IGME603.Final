using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour{
    public GameObject chessBoss;
    public GameObject Boss;

    private bool isCheck;
    void Start(){
        ForwardCamera.getInstance().setTarget(gameObject.transform);
        chessBoss.SetActive(false);
        isCheck = false;
    }

    void Update(){
        if (!Boss & !isCheck){
            chessBoss.SetActive(true);
            isCheck = true;
        }
    }
}
