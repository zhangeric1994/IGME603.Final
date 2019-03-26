using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Bullet : PooledObject
{
    [Header("Config")]
    public bool isFriendly = false;
    public int numHits = 1;
    public int rawDamage = 100;
    public float range = 0;

    private int numHitsRemaining;
    private LinearMovement linear;

    protected override void OnEnable()
    {
        base.OnEnable();
        linear = GetComponent<LinearMovement>();
        numHitsRemaining = numHits;

        if (range > 0)
            StartCoroutine(RecycleAfterOutOfRange());
    }
    
    protected override void Die()
    {
        base.Die();
        float x;
        // FX
    }

    private IEnumerator RecycleAfterOutOfRange()
    {
        while (Vector3.Distance(transform.position, linear.initialPosition) < range)
            yield return null;

        Die();

        yield break;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isFriendly)
            switch (other.tag)
            {
                case "Enemy":
                    //other.GetComponent<IDamageable>().ApplyDamage(rawDamage);
                    //if (--numHitsRemaining == 0)
                    //Die();
                    break;
            }
        else
            switch (other.tag)
            {
                case "Player":
                    //other.GetComponent<IDamageable>().ApplyDamage(rawDamage);
                    //if (--numHitsRemaining == 0)
                    //Die();
                    break;
            }
    }
}
