using System.Collections;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public int id = -1;
    public float lifeSpan = 0;

    protected virtual void OnEnable()
    {
        if (lifeSpan > 0)
            StartCoroutine(RecycleAfter(lifeSpan));
    }

    protected virtual void Die()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        ObjectPool.Singleton.Push(this);
    }

    private IEnumerator RecycleAfter(float t)
    {
        yield return new WaitForSeconds(t);
        Die();
    }
}
