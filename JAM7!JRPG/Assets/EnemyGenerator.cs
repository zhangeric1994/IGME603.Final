using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private int num;

    // Start is called before the first frame update
    public void Active() {
        for (int i = 0; i < num; i++) {
            Instantiate(enemy, transform.position, Quaternion.identity);
        }
    }

}
