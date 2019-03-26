using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFx : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(killAfterTime(1));
    }

    private IEnumerator killAfterTime(int x)
    {
        yield return new WaitForSeconds(x);
        Destroy(this.gameObject);
    }
}
