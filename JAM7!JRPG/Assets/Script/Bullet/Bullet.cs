using System.Collections;
using UnityEngine;
//[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Bullet : PooledObject
{
    [Header("Config")]
    public bool isFriendly = false;
    public int numHits = 1;
    public int rawDamage;
    public float range = 0;

    private int numHitsRemaining;
    private LinearMovement linear;
    [SerializeField] private GameObject FX;

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
                    other.GetComponentInParent<Enemy>().Hurt(rawDamage);
                    Vector2 dir = (transform.position - other.transform.position).normalized;
                    var temp = Instantiate(FX,transform.position,Quaternion.identity);
                    temp.transform.right = -dir;
                    Destroy(temp,0.5f);
                    Die();
                    break;
                

                
                case "Ground":
                    Vector2 dir1 = (transform.position - other.transform.position).normalized;
                    var temp1 = Instantiate(FX,transform.position,Quaternion.identity);
                    temp1.transform.right = -dir1;
                    Destroy(temp1,0.5f);
                    Die();
                    break;
            }
        else
            switch (other.tag)
            {
                case "PlayerHitBox":
                    //other.GetComponent<IDamageable>().ApplyDamage(rawDamage);
                    //if (--numHitsRemaining == 0)
                    other.GetComponentInParent<PlayerCombatController>().Hurt(1);
                    Vector2 dir = (transform.position - other.transform.position).normalized;
                    var temp = Instantiate(FX,transform.position,Quaternion.identity);
                    temp.transform.right = -dir;
                    Destroy(temp,0.5f);
                    Die();
  
                    break;
                
                case "Ground":
                    Vector2 dir1 = (transform.position - other.transform.position).normalized;
                    var temp1 = Instantiate(FX,transform.position,Quaternion.identity);
                    temp1.transform.right = -dir1;
                    Destroy(temp1,0.5f);
                    Die();

                    break;
                
                case "Shield":
                    Vector2 dir2 = (transform.position - other.transform.position).normalized;
                    var temp2 = Instantiate(FX,transform.position,Quaternion.identity);
                    temp2.transform.right = -dir2;
                    Destroy(temp2,0.5f);
                    Die();
                    break;
            }
    }

}
