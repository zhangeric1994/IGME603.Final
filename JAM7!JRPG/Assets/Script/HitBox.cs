using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private Enemy self;
    // Start is called before the first frame update
    void Start()
    {
        self = transform.parent.GetComponent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.tag == "PlayerHitBox")
            {
                self.setIdle(0.5f);
                var direction = (other.transform.position - self.transform.position).normalized;
                transform.parent.GetComponent<Rigidbody2D>().AddForce(-direction * 20f);
                transform.parent.GetComponent<Rigidbody2D>().AddForce(transform.up * 40f);
                // do dmg here
                other.GetComponentInParent<PlayerCombatController>().Hurt();
            }
        else if (other.tag == "Shield")
        {
            self.setIdle(0.5f);
            var direction = (other.transform.position - self.transform.position).normalized;
            transform.parent.GetComponent<Rigidbody2D>().AddForce(-direction * 40f);
            transform.parent.GetComponent<Rigidbody2D>().AddForce(transform.up * 40f);
        }
    }
}
