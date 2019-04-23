using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class GrassSoundTrigger : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter emitter;
    // Start is called before the first frame update
    void Start()
    {     
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grass")
        {
            emitter.SetParameter("Grass", 1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grass")
        {
            emitter.SetParameter("Grass", 1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grass")
        {
            emitter.SetParameter("Grass", 0);
        }
    }
}
