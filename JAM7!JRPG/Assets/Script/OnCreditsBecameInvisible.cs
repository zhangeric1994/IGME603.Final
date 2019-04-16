using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCreditsBecameInvisible : MonoBehaviour
{
    [HideInInspector]
    public bool rolling = true;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        rolling = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnBecameInvisible()
    {
        if (timer < 3.0f) return;
        rolling = false;
    }
}
