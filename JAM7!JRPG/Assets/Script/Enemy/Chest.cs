using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    private GameObject enemy;

    public void SetEnemy(GameObject _enemy){
        enemy = _enemy;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Animator anim = gameObject.GetComponent<Animator>();

        anim.SetBool("isOpened", true);

        StartCoroutine(DestroyMyself());
    }

    IEnumerator DestroyMyself(){
        yield return new WaitForSeconds(0.5f);
        Debug.Log("enemy");
        enemy.transform.position = gameObject.transform.position;
        Destroy(gameObject);
    }
}
